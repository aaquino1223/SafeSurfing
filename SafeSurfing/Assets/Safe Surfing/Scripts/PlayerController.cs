using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static SafeSurfing.Common.Constants.PlayerInput;

namespace SafeSurfing
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        public float Speed = 5f;
        public float FallSpeed = 2.5f;

        private Rigidbody2D _RigidBody;

        public bool IsMoving { get { return _Horizontal != 0 || _Vertical != 0; } }
        private float _Horizontal;
        private float _Vertical;
        private float _InitialGravityScale;

        // Start is called before the first frame update
        void Start()
        {
            _RigidBody = GetComponent<Rigidbody2D>();
            _InitialGravityScale = _RigidBody.gravityScale;
        }

        // Update is called once per frame
        void Update()
        {
            _Horizontal = IsPressingLeft ? -1 : (IsPressingRight ? 1 : 0);

            _Vertical = IsPressingDown ? -1 : ((IsPressingUp || IsPressingSpace) ? 1 : 0);
        }

        private void FixedUpdate()
        {
            if (IsMoving)
            {
                _RigidBody.velocity = new Vector2(_Horizontal * Speed, _Vertical * Speed);
                _RigidBody.gravityScale = 0;
            }
            else
            {
                _RigidBody.velocity = new Vector2(0f, -FallSpeed);
                _RigidBody.gravityScale = _InitialGravityScale;
            }
        }
    }

    
}