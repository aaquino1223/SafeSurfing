using SafeSurfing.Common;
using SafeSurfing.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace SafeSurfing
{
    public abstract class EnemyController : HealthController
    {
        public float Speed = 5f;
        public int Points;

        private IEnumerable<Vector3> _Pattern;
        private int _Current;

        //State get/set manages private _State. In set, if value is different, _Pattern is refreshed
        private EnemyState _State;
        public EnemyState State
        {
            get { return _State; }
            protected set
            {
                if (_State != value)
                {
                    _State = value;
                    SetPattern();
                }
            }
        }

        public GameObject Screen;

        protected float _XMax;
        protected float _YMax;

        //private HealthController _HealthController;

        public event EventHandler<int> Destroyed;
        // Start is called before the first frame update
        void Start()
        {
            Initialize();
        }

        protected virtual void Initialize()
        {
            if (Screen == null)
                Screen = GameObject.FindGameObjectWithTag("Bounds");

            var collider = Screen.GetComponent<EdgeCollider2D>();

            _XMax = collider.points.Max(point => point.x);
            _YMax = collider.points.Max(point => point.y);

            _Pattern = CreateMovementPattern();
            _Current = 0;

            //_HealthController = GetComponent<HealthController>();
            AddLifeLostListener(OnLifeLost, true);
        }

        private void OnLifeLost()
        {
            if (IsDead)
                OnDestroyed();
        }


        // Update is called once per frame
        void Update()
        {

        }

        private void FixedUpdate()
        {
            if (_Pattern == null || _Pattern.Count() == 0)
                return;

            var deltaTime = Time.deltaTime;
            var target = _Pattern.ElementAt(_Current);

            transform.localPosition = Vector3.MoveTowards(transform.localPosition, target, Speed * deltaTime);

            //Update _Current plus 1 mod count. When _Current equals count, mod returns 0
            if (transform.localPosition == target)
            {
                _Current = (_Current + 1) % _Pattern.Count();

                if (_Current == 0)
                    OnPatternCompleted();
            }
        }

        protected virtual void OnPatternCompleted()
        {

        }

        protected void SetPattern()
        {
            _Pattern = CreateMovementPattern();
        }

        protected abstract IEnumerable<Vector3> CreateMovementPattern();

        protected override void OnTriggerCollison(Collider2D collision)
        {
            base.OnTriggerCollison(collision);

            if (State != EnemyState.Normal)
                return;

            if (collision.CompareTag("Bounds"))
            {
                Points = 0;
                StartCoroutine(Util.TimedAction(null, OnDestroyed, 0.5f));
            }
        }
        protected virtual void OnDestroyed()
        {
            Destroyed?.Invoke(this, Points);
            Destroy(gameObject);
        }
    }
}

