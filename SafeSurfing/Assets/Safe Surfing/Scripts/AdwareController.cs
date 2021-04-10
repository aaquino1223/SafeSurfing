using SafeSurfing.Common.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SafeSurfing
{
    public class AdwareController : EnemyController
    {
        private GameObject _GravityTrap;

        protected override void Initialize()
        {
            base.Initialize();

            _GravityTrap = transform.GetChild(0).gameObject;
            _GravityTrap.SetActive(false);
        }

        protected override IEnumerable<Vector3> CreateMovementPattern()
        {
            var pattern = new List<Vector3>();

            switch (State)
            {
                case EnemyState.Spawned:
                    var yOffset = Random.Range(-2f, 2f);
                    pattern.Add(new Vector3(transform.localPosition.x, yOffset, 0));
                    break;
                case EnemyState.Normal:
                    //pattern.Add(new Vector3(transform.localPosition.x, -_YMax - 2f, 0));
                    break;
            }

            return pattern;
        }

        protected override void OnPatternCompleted()
        {
            base.OnPatternCompleted();

            if (State == EnemyState.Spawned)
                State = EnemyState.Normal;

            _GravityTrap.SetActive(true);
        }
    }
}