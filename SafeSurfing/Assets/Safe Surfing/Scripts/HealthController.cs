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

        public UnityEvent LifeLost;
        public UnityEvent AllLivesLost;

        public bool IsDead => Lives == 0;
        public bool IsIgnoringDamage { get; private set; }

        public void SetIgnoreBullets(float ignoreTime)
        {
            StartCoroutine(Util.TimedAction(
                () => IsIgnoringDamage = true,
                () => IsIgnoringDamage = false,
                ignoreTime
                ));
        }

        public void AddLifeLostListener(UnityAction unityAction, bool includeAllLivesLost = false)
        {
            LifeLost.AddListener(unityAction);
            if (includeAllLivesLost)
                AllLivesLost.AddListener(unityAction);
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
                LifeLost?.Invoke();

                if (IsDead)
                    AllLivesLost?.Invoke();
                else
                    LifeLost?.Invoke();
            }
        }
    }
}