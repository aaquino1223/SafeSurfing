using SafeSurfing.Common.Enums;
using SafeSurfing.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SafeSurfing
{
    public class PickUpController : MonoBehaviour
    {
        public bool IsTrojan;
        public PickUpType PickUpType;
        public float EffectDuration;

        private SpriteRenderer _SpriteRenderer;

        private CircleCollider2D _Collider;

        // Start is called before the first frame update
        void Start()
        {
            _SpriteRenderer = GetComponent<SpriteRenderer>();
            _Collider = GetComponent<CircleCollider2D>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter2D(Collider2D collision){
            if (collision.CompareTag("Player")){
                 // interpret the type of pickup
                 // get effect duration
                 // Apply abilty of pickup for set duration

                var Player = collision.gameObject.GetComponent<PlayerController>();
                Debug.Log(Player);
                ApplyPickupEffect(Player);
             }
        }

        private void ApplyPickupEffect(PlayerController player){
            _SpriteRenderer.color = new Color(0,0,0,0);
            _Collider.enabled = false;

            switch(PickUpType){
                case PickUpType.FiringRate:
                    StartCoroutine(Util.TimedAction(() => player.SetFiringRate(0.25f), () => player.SetFiringRate(0.5f), 5f));
                    StartCoroutine(Util.TimedAction(null, () => Destroy(gameObject), 5f));
                    break;
                case PickUpType.BulletSpeed:
                    break;
                case PickUpType.MoveSpeed:
                    break;
                case PickUpType.Special:
                    break;
            }
        }


    }
}