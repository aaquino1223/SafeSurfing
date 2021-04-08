using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SafeSurfing
{
    public class BulletController : MonoBehaviour
    {
        private GameObject _Parent;
        public GameObject Parent
        {
            get { return _Parent; }
            set
            {
                if (_Parent != value)
                {
                    _Parent = value;
                    ParentTag = _Parent?.tag;
                }
            }
        }

        public string ParentTag { get; private set; }

        private float _Speed;
        public float Speed
        {
            get { return _Speed; }
            set
            {
                if (_Speed != value)
                    _Speed = value;
            }
        }

        private Vector3 _Direction;
        public Vector3 Direction
        {
            get { return _Direction; }
            set
            {
                if (_Direction != value)
                    _Direction = value;
            }
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
            var deltaTime = Time.deltaTime;
            var localX = transform.localPosition.x;
            var localY = transform.localPosition.y;

            transform.localPosition = new Vector3(localX + Direction.x * Speed * deltaTime, localY + Direction.y * Speed * deltaTime, 0);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject != Parent && !collision.CompareTag("Pickup"))
            {
                if (collision.CompareTag("Bounds"))
                    Destroy(gameObject, 0.1f);
                else if (!collision.CompareTag(tag) && !collision.CompareTag(ParentTag))
                {
                    var healthController = collision.gameObject.GetComponent<HealthController>();

                    if (healthController?.IsIgnoringDamage != true)
                        Destroy(gameObject);
                }
            }
        }
    }
}