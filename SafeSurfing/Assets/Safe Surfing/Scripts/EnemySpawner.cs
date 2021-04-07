using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using SafeSurfing.Common;
using UnityEngine.Events;

namespace SafeSurfing
{
    public class EnemySpawner : MonoBehaviour
    {
        public GameObject Screen;

        private float _XMax;
        private float _YMax;

        public UnityEvent EnemySpawned;
        public UnityEvent WaveChanged;

        public LevelBehavior Level;
        public int WaveIndex = -1;
        private int _EnemyDestroyed = 0;

        private List<SpawnPoint> _SpawnPoints;
        // Start is called before the first frame update
        void Start()
        {
            var collider = Screen.GetComponent<EdgeCollider2D>();

            //Using points array because values are not affected by the rotation of parent object
            _XMax = collider.points.Max(point => point.x);
            _YMax = collider.points.Max(point => point.y);

            //_StartTime = Time.fixedTime;

            NextWave();
        }

        private void NextWave()
        {
            WaveIndex++;
            WaveChanged?.Invoke();

            if (Level == null || WaveIndex >= Level.Waves.Count())
                return;

            _EnemyDestroyed = 0;

            _SpawnPoints = Level.Waves[WaveIndex].SpawnPoints.ToList();
            foreach (var spawnPoint in _SpawnPoints)
            {
                UnityAction action = () =>
                {
                    var xPos = Random.Range(-_XMax + 1, _XMax - 1);

                    var spawnPosition = Quaternion.Euler(transform.rotation.eulerAngles) * new Vector3(xPos, _YMax + 1, 0);

                    var enemyClone = Instantiate(spawnPoint.EnemyPrefab, spawnPosition, transform.rotation, transform);

                    var enemyController = enemyClone.GetComponent<EnemyController>();
                    enemyController.Screen = Screen;
                    enemyController.Destroyed.AddListener(EnemyDestroyed);

                    EnemySpawned?.Invoke();
                }; 

                StartCoroutine(Util.TimedAction(null, action, spawnPoint.Time));
            }
        }


        private void EnemyDestroyed()
        {
            _EnemyDestroyed++;

            if (_EnemyDestroyed == _SpawnPoints.Count)
                NextWave();
        }
    }
}