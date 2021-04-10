using SafeSurfing.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SafeSurfing
{
    public class HealthController : MonoBehaviour
    {
        public int Lives = 3;

        public UnityEvent LifeGained;
        public UnityEvent LifeLost;
        public UnityEvent AllLivesLost;

        public GameObject ExplosionPrefab;

        public bool IsDead => Lives == 0;
        public bool IsIgnoringDamage { get; private set; }

        public void SetIgnoreBullets(float ignoreTime, bool hasShield)
        {
            StartCoroutine(Util.TimedAction(
                () => { IsIgnoringDamage = true; if (!hasShield) StartCoroutine(FlashOnDamage()); },
                () => IsIgnoringDamage = false,
                ignoreTime
                ));
        }

        public void AddLife()
        {
            Lives++;
        }
        public void AddLifeLostListener(UnityAction unityAction, bool includeAllLivesLost = false)
        {
            LifeLost.AddListener(unityAction);
            if (includeAllLivesLost)
                AllLivesLost.AddListener(unityAction);
        }

        public void AddLifeGainedListener(UnityAction unityAction)
        {
            LifeGained.AddListener(unityAction);
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            OnTriggerCollison(collision);
        }

        protected virtual void OnTriggerCollison(Collider2D collision)
        {
            if (collision.CompareTag("Bullet") && !IsIgnoringDamage)
            {
                //Prevent friendly fire
                {
                    var bulletController = collision.gameObject.GetComponent<BulletController>();

                    if (bulletController.ParentTag == tag)
                        return;
                }

                OnDamaged();
            }
        }

        protected void OnDamaged()
        {
            if (!IsDead)
            {
                Lives--;

                if (IsDead)
                {
                    var explosion = Instantiate(ExplosionPrefab, transform.position, transform.rotation, transform.parent);
                    Destroy(explosion, 0.7f);
                    AllLivesLost?.Invoke();
                }
                else
                    LifeLost?.Invoke();
            }
        }

        public IEnumerator FlashOnDamage(){
            var spriteRenderer = GetComponent<SpriteRenderer>();
            var spriteColor = spriteRenderer.color;

            while(IsIgnoringDamage){
                spriteRenderer.color = Color.clear;
                yield return new WaitForSeconds(0.1f);
                spriteRenderer.color = spriteColor;
                yield return new WaitForSeconds(0.1f);
            }
        }


    }
}