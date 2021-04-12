using SafeSurfing.Common.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SafeSurfing {
    public abstract class FollowPlayerController : EnemyController
    {

        public Transform PlayerTransform;
        private Vector3 _LastDirection;
        public bool CanRotate { get; protected set; } = true;

        protected override void Initialize()
        {
            base.Initialize();

            if (PlayerTransform == null)
                PlayerTransform = GameObject.FindGameObjectWithTag("Player")?
                    .GetComponent<Transform>();
        }

        protected abstract IEnumerable<Vector3> SpawnBehavior();

        protected override IEnumerable<Vector3> CreateMovementPattern()
        {
            var pattern = new List<Vector3>();
            switch (State)
            {
                case EnemyState.Spawned:
                    pattern.AddRange(SpawnBehavior());
                    break;
                case EnemyState.Normal:
                    Vector3 normDirection = Vector3.zero;
                    if (PlayerTransform != null)
                    {
                        var direction = PlayerTransform.localPosition - transform.localPosition;
                        normDirection = direction.normalized;

                        if (normDirection.y >= 0) //If below player keep going in last direction
                            normDirection = _LastDirection;
                    }
                    else
                        normDirection = _LastDirection;


                    if (CanRotate)
                    {
                        //Gets the angle of rotation to face the direction it is going
                        var angle = Vector3.SignedAngle(-Vector3.up, normDirection, Vector3.forward);

                        transform.localRotation = Quaternion.Euler(new Vector3(0, 0, angle));
                    }

                    pattern.Add(transform.localPosition + normDirection);
                    _LastDirection = normDirection; //Store last direction followed
                    break;
            }

            return pattern;
        }
        protected override void OnPatternCompleted()
        {
            if (State == EnemyState.Spawned)
                State = EnemyState.Normal;
            else if (State == EnemyState.Normal)
                SetPattern();
        }
    }
}