using SafeSurfing.Common.Interfaces;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static SafeSurfing.Common.Constants.PlayerInput;

namespace SafeSurfing
{
    [RequireComponent(typeof(BulletSpawner))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour, IHeading
    {
        public float Speed = 5f;
        public float FallSpeed = 2.5f;

        public Vector3 Heading => transform.up;

        public bool IsMoving { get { return _Horizontal != 0 || _Vertical != 0; } }


        private float _Horizontal;
        private float _Vertical;

        private BulletSpawner _BulletSpawner;

        // Start is called before the first frame update
        void Start()
        {
            _BulletSpawner = GetComponent<BulletSpawner>();
        }

        // Update is called once per frame
        void Update()
        {
            _Horizontal = IsPressingLeft ? -1 : (IsPressingRight ? 1 : 0);

            _Vertical = IsPressingDown ? -1 : (IsPressingUp ? 1 : 0);

            if (Input.GetKeyDown(KeyCode.Space))
                _BulletSpawner.Shoot();
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
    }

    
}