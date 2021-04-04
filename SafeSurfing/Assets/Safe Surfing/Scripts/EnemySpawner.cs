using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace SafeSurfing
{
    public class EnemySpawner : MonoBehaviour
    {
        public GameObject Screen;

        private float _XMax;
        private float _YMax;

        private float _StartTime;
        private float _ElapsedTime;
        private float _EnemySpawnInterval = 3f;

        public GameObject EnemyPrefab;
        // Start is called before the first frame update
        void Start()
        {
            var collider = Screen.GetComponent<EdgeCollider2D>();

            //Using points array because values are not affected by the rotation of parent object
            _XMax = collider.points.Max(point => point.x);
            _YMax = collider.points.Max(point => point.y);

            _StartTime = Time.fixedTime;
        }

        // Update is called once per frame
        void Update()
        {
            var time = _ElapsedTime - _StartTime;
            if (time > _EnemySpawnInterval)
            {
                _StartTime = _ElapsedTime;

                var xPos = Random.Range(-_XMax + 1, _XMax - 1);

                var spawnPosition = Quaternion.Euler(transform.rotation.eulerAngles) * new Vector3(xPos, _YMax + 1, 0);

                
                var enemyClone = Instantiate(EnemyPrefab, spawnPosition, transform.rotation, transform);

                

                var enemyController = enemyClone.GetComponent<EnemyController>();
                enemyController.Screen = Screen;
            }
        }

        private void FixedUpdate()
        {
            _ElapsedTime = Time.fixedTime;
        }
    }
}