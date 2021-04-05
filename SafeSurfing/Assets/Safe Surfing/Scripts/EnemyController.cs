using SafeSurfing.Common.Enums;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace SafeSurfing
{
    [RequireComponent(typeof(HealthController))]
    public abstract class EnemyController : MonoBehaviour
    {
        public float Speed = 5f;

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
                    _Pattern = CreateMovementPattern();
                }
            }
        }

        public GameObject Screen;

        protected float _XMax;
        protected float _YMax;

        private HealthController _HealthController;

        public UnityEvent<int> Destroying;
        public UnityEvent Destroyed;
        // Start is called before the first frame update
        void Start()
        {
            Initialize();
        }

        protected virtual void Initialize()
        {
            var collider = Screen.GetComponent<EdgeCollider2D>();

            _XMax = collider.points.Max(point => point.x);
            _YMax = collider.points.Max(point => point.y);

            _Pattern = CreateMovementPattern();
            _Current = 0;

            _HealthController = GetComponent<HealthController>();
            _HealthController.LifeLost.AddListener(OnLifeLost);
        }

        private void OnLifeLost()
        {
            if (_HealthController.IsDead)
            {
                Destroying?.Invoke(0);
                //Maybe play some animation
                Destroyed?.Invoke();
                Destroy(gameObject);
            }

        }


        // Update is called once per frame
        void Update()
        {

        }

        private void FixedUpdate()
        {
            VirtualFixedUpdate();
        }

        protected virtual void VirtualFixedUpdate()
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

        protected abstract IEnumerable<Vector3> CreateMovementPattern();
    }
}

