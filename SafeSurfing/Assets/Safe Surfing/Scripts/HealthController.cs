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
        public bool IsIgnoringBullets { get; private set; }

        public void SetIgnoreBullets(float ignoreTime)
        {
            StartCoroutine(Util.TimedAction(
                () => IsIgnoringBullets = true,
                () => IsIgnoringBullets = false,
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
            if (collision.CompareTag("Bullet") && !IsIgnoringBullets)
            {
                //Prevent friendly fire
                {
                    var bulletController = collision.gameObject.GetComponent<BulletController>();

                    if (bulletController.Parent.CompareTag(tag))
                        return;
                }

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
}