using SafeSurfing.Common.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SafeSurfing
{
    public class TrojanController : FollowPlayerController
    {
        private Collider2D _Collider;

        protected override void Initialize()
        {
            base.Initialize();

            _Collider = GetComponent<Collider2D>();
            _Collider.enabled = false;

            CanRotate = false;
        }

        protected override IEnumerable<Vector3> SpawnBehavior()
        {
            return new List<Vector3>() { new Vector3(transform.localPosition.x, transform.localPosition.y + 3f, 0f) };
        }

        protected override void OnPatternCompleted()
        {
            base.OnPatternCompleted();

            if (State == EnemyState.Normal)
                _Collider.enabled = true;
        }
    }
}