using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SafeSurfing
{
    public class EnemySpawner : MonoBehaviour
    {
        public GameObject Screen;

        private float _XMax;
        private float _YMax;
        // Start is called before the first frame update
        void Start()
        {
            var collider = Screen.GetComponent<EdgeCollider2D>();

            var extents = collider.bounds.extents;
            _XMax = extents.x;
            _YMax = extents.y;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}