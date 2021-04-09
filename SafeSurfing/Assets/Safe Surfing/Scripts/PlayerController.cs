using SafeSurfing.Common;
using SafeSurfing.Common.Enums;
using SafeSurfing.Common.Interfaces;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using static SafeSurfing.Common.Constants.PlayerInput;

namespace SafeSurfing
{
    [RequireComponent(typeof(BulletSpawner))]
    public class PlayerController : HealthController, IHeading
    {
        public float Speed = 5f;
        public float FallSpeed = 2.5f;

        public Vector3 Heading => transform.up;

        public bool IsMoving { get { return _Horizontal != 0 || _Vertical != 0; } }


        private float _Horizontal;
        private float _Vertical;

        private BulletSpawner _BulletSpawner;
        private Dictionary<PickUpType, Coroutine> _PickUpCoroutineDictionary;
        //private HealthController _HealthController;

        // Start is called before the first frame update
        void Start()
        {
            _PickUpCoroutineDictionary = new Dictionary<PickUpType, Coroutine>();
            _BulletSpawner = GetComponent<BulletSpawner>();

            //_HealthController = GetComponent<HealthController>();

            AddLifeLostListener(OnLifeLost);
        }

        private void OnLifeLost()
        {
            SetIgnoreBullets(3f);
        }

        // Update is called once per frame
        void Update()
        {
            _Horizontal = IsPressingLeft ? -1 : (IsPressingRight ? 1 : 0);

            _Vertical = IsPressingDown ? -1 : (IsPressingUp ? 1 : 0);

            if (IsPressingSpace)
                _BulletSpawner.Shoot();
            
            //// Game Manager can probably handle this
            //if (PlayerLives <= 0) 
            //{
            //    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            //    // We can either restart level automatically or show GUI with final score + retry...
            //}
        }

        private void FixedUpdate()
        {
            var newPosition = new Vector3();
            var deltaTime = Time.deltaTime;
            var localX = transform.localPosition.x;
            var localY = transform.localPosition.y;
            if (IsMoving)
                newPosition = new Vector3(localX + _Horizontal * Speed * deltaTime, localY + _Vertical * Speed * deltaTime, 0);
            else
                newPosition = new Vector3(localX, localY - FallSpeed * deltaTime, 0);

            transform.localPosition = newPosition;
        }

        public void SetFiringRate(float firingRate){
            _BulletSpawner.FiringRate = firingRate;
        }

        public void SetBulletSpeed(float bulletSpeed){
            _BulletSpawner.BulletSpeed = bulletSpeed;
        }



        protected override void OnTriggerCollison(Collider2D collision)
        {
            base.OnTriggerCollison(collision);

            if (collision.CompareTag("Enemy") && !IsIgnoringDamage)
                OnDamaged();
            else if (collision.CompareTag("Pickup"))
            {
                var pickUp = collision.gameObject.GetComponent<PickUpController>();
                var pickUpType = pickUp.PickUpType;

                Coroutine coroutine;
                if (_PickUpCoroutineDictionary.TryGetValue(pickUpType, out coroutine))
                {
                    StopCoroutine(coroutine);
                    _PickUpCoroutineDictionary.Remove(pickUpType);
                }

                switch (pickUp.PickUpType)
                {
                    case PickUpType.FiringRate:
                        coroutine = StartCoroutine(Util.TimedAction(() => SetFiringRate(0.25f), () => SetFiringRate(0.5f), pickUp.EffectDuration));
                        break;
                    case PickUpType.BulletSpeed:
                        coroutine = StartCoroutine(Util.TimedAction(() => SetBulletSpeed(20f), () => SetFiringRate(10f), pickUp.EffectDuration));
                        break;
                    case PickUpType.MoveSpeed:
                        coroutine = StartCoroutine(Util.TimedAction(() => Speed = 10f, () => Speed = 5f, pickUp.EffectDuration));
                        break;
                    case PickUpType.Special:
                        break;
                }

                _PickUpCoroutineDictionary[pickUpType] = coroutine;

                pickUp.Consumed();
            }
        }

    }

    
}