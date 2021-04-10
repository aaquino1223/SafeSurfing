using SafeSurfing.Common;
using SafeSurfing.Common.Enums;
using SafeSurfing.Common.Interfaces;
using System;
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
        private Dictionary<PullController, Vector3> _PullVectorDictionary;

        private float _Horizontal;
        private float _Vertical;

        private BulletSpawner _BulletSpawner;
        private Dictionary<PickUpType, Coroutine> _PickUpCoroutineDictionary;

        public SpriteRenderer _Shield;
        //private HealthController _HealthController;

        // Start is called before the first frame update
        void Start()
        {
            _PickUpCoroutineDictionary = new Dictionary<PickUpType, Coroutine>();
            _PullVectorDictionary = new Dictionary<PullController, Vector3>();
            _BulletSpawner = GetComponent<BulletSpawner>();

            //_HealthController = GetComponent<HealthController>();

            AddLifeLostListener(OnLifeLost);
            AddLifeGainedListener(OnLifeGained);
        }

        private void OnLifeLost()
        {
            SetIgnoreBullets(3f, false);
        }

        private void OnLifeGained()
        {
            AddLife();
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
            var newPosition = Vector3.zero;
            var deltaTime = Time.deltaTime;
            var localX = transform.localPosition.x;
            var localY = transform.localPosition.y;
            if (IsMoving)
                newPosition = new Vector3(localX + _Horizontal * Speed * deltaTime, localY + _Vertical * Speed * deltaTime, 0);
            else
                newPosition = new Vector3(localX, localY - FallSpeed * deltaTime, 0);

            var pullOffset = Vector3.zero;

            _PullVectorDictionary.Values.ToList().ForEach(x => pullOffset += x);

            transform.localPosition = newPosition + pullOffset * deltaTime;
        }

        public void SetFiringRate(float firingRate){
            _BulletSpawner.FiringRate = firingRate;
        }

        public void SetBulletSpeed(float bulletSpeed){
            _BulletSpawner.BulletSpeed = bulletSpeed;
        }
        public void ActivateShield(bool state){
            _Shield.enabled = state;
            if (state)
                SetIgnoreBullets(5f, true);
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
                        coroutine = StartCoroutine(Util.TimedAction(() => SetBulletSpeed(20f), () => SetBulletSpeed(10f), pickUp.EffectDuration));
                        break;
                    case PickUpType.MoveSpeed:
                        coroutine = StartCoroutine(Util.TimedAction(() => Speed = 10f, () => Speed = 5f, pickUp.EffectDuration));
                        break;
                    case PickUpType.Special:
                        break;
                    case PickUpType.Shield:
                        coroutine = StartCoroutine(Util.TimedAction(() => ActivateShield(true), () => ActivateShield(false), pickUp.EffectDuration));
                        break;
                    case PickUpType.ExtraLife:
                        coroutine = StartCoroutine(Util.TimedAction(null, () => {
                        if (Lives < 3)
                        {
                            LifeGained?.Invoke();
                        }
                        }, 0f));
                        break;
                }

                _PickUpCoroutineDictionary[pickUpType] = coroutine;

                pickUp.Consumed();
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.CompareTag("Trap"))
            {
                PullController pullController;

                if (collision.gameObject.TryGetComponent(out pullController))
                {
                    var pullVector = pullController.GetEffect(gameObject);

                    _PullVectorDictionary[pullController] = pullVector;
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Trap"))
            {
                PullController pullController;

                if (collision.gameObject.TryGetComponent(out pullController))
                    if (_PullVectorDictionary.ContainsKey(pullController))
                    _PullVectorDictionary.Remove(pullController);
            }
        }
    }

    
}