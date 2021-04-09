using SafeSurfing.Common.Enums;
using SafeSurfing.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SafeSurfing.Common.Interfaces;
using System;

namespace SafeSurfing
{
    public class PickUpController : MonoBehaviour, ICanSpawnEnemy
    {
        public PickUpType PickUpType;
        public float EffectDuration;
        public GameObject TrojanPrefab;
        public float FallSpeed = 2.5f;

        public event EventHandler<IEnumerable<EnemyController>> SpawnedEnemies;

        //private SpriteRenderer _SpriteRenderer;

        //private CircleCollider2D _Collider;

        // Start is called before the first frame update
        void Start()
        {
            //_SpriteRenderer = GetComponent<SpriteRenderer>();
            //_Collider = GetComponent<CircleCollider2D>();
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
}