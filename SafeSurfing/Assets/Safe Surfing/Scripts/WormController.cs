using SafeSurfing.Common.Enums;
using SafeSurfing.Common.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SafeSurfing
{
    public class WormController : FollowPlayerController, ICanSpawnEnemy
    {
        public GameObject WormPrefab;

        private bool _SpawnedEnemies;
        public int SplitCount = 2;
        private int _SplitDirection;
        
        public event EventHandler<IEnumerable<EnemyController>> SpawnedEnemies;

        protected override IEnumerable<Vector3> SpawnBehavior()
        {
            var pattern = new List<Vector3>();
            if (_SplitDirection != 0)
            {
                var splitSpawnPosition = new Vector3(transform.localPosition.x + _SplitDirection, transform.localPosition.y + 1f, 0);
                pattern.Add(splitSpawnPosition);
                var angle = Vector3.SignedAngle(-Vector3.up, (splitSpawnPosition - transform.localPosition).normalized, Vector3.forward);

                transform.localRotation = Quaternion.Euler(new Vector3(0, 0, angle));
            }
            else
                pattern.Add(new Vector3(transform.localPosition.x, _YMax - 2f, 0));

            return pattern;
        }

        protected override void OnDestroyed()
        {
            if (Points > 0)
            {
                if (SplitCount > 0)
                {
                    var worm1 = Instantiate(WormPrefab, transform.position, transform.rotation, transform.parent);
                    var worm2 = Instantiate(WormPrefab, transform.position, transform.rotation, transform.parent);

                    var worm1Controller = worm1.GetComponent<WormController>();
                    var worm2Controller = worm2.GetComponent<WormController>();

                    //Ensure it does not reference itself
                    worm1Controller.WormPrefab = worm2Controller.WormPrefab = WormPrefab;

                    worm1Controller._SplitDirection = -1;
                    worm2Controller._SplitDirection = 1;

                    worm1Controller.SplitCount = worm2Controller.SplitCount = SplitCount - 1;

                    SpawnedEnemies?.Invoke(this, new List<EnemyController>() { worm1Controller, worm2Controller });
                }
            }

            base.OnDestroyed();
        }
    }
}