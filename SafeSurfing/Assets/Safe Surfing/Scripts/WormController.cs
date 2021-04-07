using SafeSurfing.Common.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SafeSurfing
{
    public class WormController : EnemyController
    {
        public Transform PlayerTransform;
        private Vector3 _LastDirection;

        protected override void Initialize()
        {
            base.Initialize();

            if (PlayerTransform == null)
                PlayerTransform = GameObject.FindGameObjectWithTag("Player")?
                    .GetComponent<Transform>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        protected override IEnumerable<Vector3> CreateMovementPattern()
        {
            var pattern = new List<Vector3>();
            switch (State)
            {
                case EnemyState.Spawned:
                    pattern.Add(new Vector3(transform.localPosition.x, _YMax - 2f, 0));
                    break;
                case EnemyState.Normal:
                    var direction = PlayerTransform.localPosition - transform.localPosition;
                    var normDirection = direction.normalized;
                    
                    if (normDirection.y >= 0) //If below player keep going in last direction
                        normDirection = _LastDirection;

                    //Gets the angle of rotation to face the direction it is going
                    var angle = Vector3.SignedAngle(-Vector3.up, normDirection, Vector3.forward);

                    transform.localRotation = Quaternion.Euler(new Vector3(0, 0, angle));

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