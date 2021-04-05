using SafeSurfing.Common.Enums;
using SafeSurfing.Common.Interfaces;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SafeSurfing
{
    [RequireComponent(typeof(BulletSpawner))]
    public class VirusController : EnemyController, IHeading
    {
        public Vector3 Heading => -transform.up;

        //private float _StartTime;
        //private float _ElapsedTime;
        //private float _BulletSpawnInterval = 2f;

        private BulletSpawner _BulletSpawner;

        protected override void Initialize()
        {
            base.Initialize();
            //_StartTime = Time.fixedTime;
            _BulletSpawner = GetComponent<BulletSpawner>();
        }

        // Update is called once per frame
        void Update()
        {
            if (State == EnemyState.Normal)
                _BulletSpawner.Shoot();
            //var time = _ElapsedTime - _StartTime;
            //if (time > _BulletSpawnInterval)
            //{
            //    _StartTime = _ElapsedTime;

            //    _BulletSpawner.Shoot();
            //}
        }

        //protected override void VirtualFixedUpdate()
        //{
        //    _ElapsedTime = Time.fixedTime;
        //    base.VirtualFixedUpdate();
        //}

        protected override IEnumerable<Vector3> CreateMovementPattern()
        {
            var pattern = new List<Vector3>();
            switch (State)
            {
                case EnemyState.Spawned:
                    pattern.Add(new Vector3(transform.localPosition.x, _YMax - 2f, 0));
                    break;
                case EnemyState.Normal:
                    pattern.Add(new Vector3(-_XMax + 1f, _YMax - 2f, 0));
                    pattern.Add(new Vector3(_XMax - 1f, _YMax - 2f, 0));
                    break;
            }

            return pattern;
        }

        protected override void OnPatternCompleted()
        {
            base.OnPatternCompleted();

            if (State == EnemyState.Spawned)
                State = EnemyState.Normal;
        }
    }
}