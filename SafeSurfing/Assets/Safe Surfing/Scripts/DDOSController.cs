using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SafeSurfing
{
    public class DDOSController : FollowPlayerController
    {
        public GameObject DDOSZone;

        protected override void Initialize()
        {
            base.Initialize();
            Lives = 1;
        }

        protected override IEnumerable<Vector3> SpawnBehavior()
        {
            return new List<Vector3>() { new Vector3(transform.localPosition.x, _YMax - 2f, 0) };
        }

        private void Update()
        {
            if (PlayerTransform != null && PlayerTransform.localPosition.y >= transform.localPosition.y)
                Detonate();
        }

        protected override void OnTriggerCollison(Collider2D collision)
        {
            if (collision.CompareTag("Player") || collision.CompareTag("Special"))
                Detonate();

            base.OnTriggerCollison(collision);
        }

        private void Detonate()
        {
            Instantiate(DDOSZone, transform.position, transform.rotation, transform.parent);
            Points = 0;
            OnDamaged();
        }
    }
}