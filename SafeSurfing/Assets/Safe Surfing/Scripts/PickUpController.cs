using SafeSurfing.Common.Enums;
using SafeSurfing.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SafeSurfing.Common.Interfaces;
using System;
using System.Linq;

namespace SafeSurfing
{
    public class PickUpController : MonoBehaviour, ICanSpawnEnemy
    {
        private PickUpType _PickUpType;
        public PickUpType PickUpType
        {
            get { return _PickUpType; }
            set
            {
                if (_PickUpType != value)
                {
                    _PickUpType = value;

                    Sprite pickUpSprite = null;
                    if (_PickUpType == PickUpType.Trojan)
                    {
                        var index = UnityEngine.Random.Range(0, PickUpSprites.Count());
                        pickUpSprite = PickUpSprites.ElementAtOrDefault(index)?.Sprite;
                    }
                    else
                        pickUpSprite = PickUpSprites.FirstOrDefault(x => x.PickUpType == _PickUpType)?.Sprite;

                    if (pickUpSprite != null)
                    {
                        GetComponent<SpriteRenderer>().sprite = pickUpSprite;
                        var collider = gameObject.AddComponent<PolygonCollider2D>();
                        collider.isTrigger = true;
                    }
                }
            }
        }

        public PickUpSprite[] PickUpSprites;

        public float EffectDuration;
        public GameObject TrojanPrefab;
        public float FallSpeed = 2.5f;

        public event EventHandler<IEnumerable<EnemyController>> SpawnedEnemies;

        private static HashSet<PickUpType> _JITOpened;
        static PickUpController() 
        {
            _JITOpened = new HashSet<PickUpType>();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void FixedUpdate()
        {
            transform.localPosition = 
                new Vector3(transform.localPosition.x, 
                transform.localPosition.y - FallSpeed * Time.deltaTime, 
                0);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Bounds"))
            {

            }
        }

        public void Consumed()
        {
            //Trojan spawn here

            if (PickUpType == PickUpType.Trojan)
            {
                var trojanPrefab = Instantiate(TrojanPrefab, transform.position, Quaternion.identity, transform.parent);
                SpawnedEnemies?.Invoke(this, new List<EnemyController>() { trojanPrefab.GetComponent<EnemyController>() });

            }
            if (!_JITOpened.Contains(PickUpType))
            {

                var pickUpSprite = PickUpSprites.FirstOrDefault(x => x.PickUpType == _PickUpType);
                var JIT = pickUpSprite?.JustInTime;
                var sprite = pickUpSprite?.Sprite;

                if (JIT != null)
                {
                    JITInstructionManager.Instance.UpdateJITController(JIT, sprite);
                    JITInstructionManager.Instance.OpenJIT();
                }
                _JITOpened.Add(PickUpType);
            }

            Destroy(gameObject);
        }
        //private void ApplyPickupEffect(PlayerController player){
        //    _SpriteRenderer.color = new Color(0,0,0,0);
        //    _Collider.enabled = false;

        //    switch(PickUpType){
        //        case PickUpType.FiringRate:
        //            StartCoroutine(Util.TimedAction(() => player.SetFiringRate(0.25f), () => player.SetFiringRate(0.5f), EffectDuration));
        //            StartCoroutine(Util.TimedAction(null, () => Destroy(gameObject), EffectDuration));
        //            break;
        //        case PickUpType.BulletSpeed:
        //            break;
        //        case PickUpType.MoveSpeed:
        //            break;
        //        case PickUpType.Special:
        //            break;
        //    }
        //}


    }

    [Serializable]
    public class PickUpSprite
    {
        public PickUpType PickUpType;
        public Sprite Sprite;
        public JustInTimeInstruction JustInTime;
    }
}