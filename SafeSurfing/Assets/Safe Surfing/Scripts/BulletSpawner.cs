using SafeSurfing.Common.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SafeSurfing 
{
    public class BulletSpawner : MonoBehaviour
    {
        public GameObject BulletPrefab;
        public float BulletSpeed = 10f;
        public float FiringRate;

        private IHeading _Heading;

        // Start is called before the first frame update
        void Start()
        {
            _Heading = GetComponent<IHeading>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Shoot()
        {
            if (_Heading == null)
                return;

            var bulletClone = Instantiate(BulletPrefab, transform.position + _Heading.Heading, transform.rotation, transform.parent);
            bulletClone.tag = "Bullet"; // add Bullet tag to bullet clone so player collision can check for it (we can assign different enemies different bullet types perhaps)

            var direction = bulletClone.transform.localPosition - transform.localPosition;

            var bulletController = bulletClone.GetComponent<BulletController>();
            bulletController.Parent = gameObject;
            bulletController.Speed = BulletSpeed;
            bulletController.Direction = direction;
        }
    }
}