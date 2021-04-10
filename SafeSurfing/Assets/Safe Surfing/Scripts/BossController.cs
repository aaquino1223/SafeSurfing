using SafeSurfing.Common.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SafeSurfing 
{
    public class BossController : VirusController
    {
        public float Frequency = 1f;
        public float Amplitude = 1f;

        private float _YZero;
        private float _Direction = 1;

        protected override void Initialize()
        {
            base.Initialize();

            IgnoreBounds = true;
        }

        protected override IEnumerable<Vector3> CreateMovementPattern()
        {
            var pattern = new List<Vector3>();
            switch (State)
            {
                case EnemyState.Spawned:
                    _YZero = _YMax - 3f;
                    pattern.Add(new Vector3(transform.localPosition.x, _YZero, 0));
                    break;
                case EnemyState.Normal:
                    var currentX = transform.localPosition.x;

                    if (currentX <= -_XMax + 1f || currentX >= _XMax - 1f)
                        _Direction = -_Direction;
                    var x = transform.localPosition.x + _Direction * 0.25f;

                    var y = _YZero + Mathf.Sin(x * Frequency) * Amplitude;

                    pattern.Add(new Vector3(x, y, 0));
                    break;
            }

            return pattern;
        }

        protected override void OnPatternCompleted()
        {
            base.OnPatternCompleted();

            if (State == EnemyState.Normal)
                SetPattern();
        }

        protected override void OnTriggerCollison(Collider2D collision)
        {
            base.OnTriggerCollison(collision);

        }
    }
}