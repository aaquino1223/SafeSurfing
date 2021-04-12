using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using SafeSurfing.Common;
using UnityEngine.Events;
using SafeSurfing.Common.Enums;
using System;
using SafeSurfing.Common.Interfaces;

namespace SafeSurfing
{
    public class GameManager : MonoBehaviour
    {
        public GameObject Screen;

        public GameObject PickUpPrefab;

        private float _XMax;
        private float _YMax;

        public UnityEvent EnemySpawned;
        public UnityEvent WaveChanged;
        public UnityEvent LevelChanged;
        public UnityEvent ScoreChanged;

        public LevelBehavior[] Levels;
        public int WaveIndex { get; private set; } = -1;
        public int LevelIndex { get; private set; } = -1;
        public float WaveSpawnDelay = 3f;

        private int _EnemyDestroyed = 0;
        private int _ExpectedSpawnCount = 0;

        private PickUpType _PickUpType;
        private Coroutine _PickUpCoroutine;

        public int Score { get; private set; } = 0;

        private Dictionary<JITType, bool> _JITFlags;
        public float MinTimeBeforeSpawn = 10f;
        public float MaxTimeBeforeSpawn = 15f;

        // Start is called before the first frame update
        void Start()
        {
            var collider = Screen.GetComponent<EdgeCollider2D>();

            //Using points array because values are not affected by the rotation of parent object
            _XMax = collider.points.Max(point => point.x);
            _YMax = collider.points.Max(point => point.y);

            NextLevel();
        }

        private void NextLevel()
        {
            WaveIndex = -1;
            LevelIndex++;
            LevelChanged?.Invoke();

            if (Levels == null || LevelIndex >= Levels.Count())
                return;

            _PickUpType = Levels[LevelIndex].PickUpType;

            NextWave();

            if (_PickUpCoroutine != null)
                StopCoroutine(_PickUpCoroutine);

            StartCoroutine(Util.TimedAction(null, SpawnPickupRecursive, WaveSpawnDelay));
        }
        private void NextWave()
        {
            // ONLY FOR TESTING PURPOSES
            //if(WaveIndex > -1){
            //    JITInstructionManager.Instance.UpdateJITController("Hello temp", "sample instructions");
            //    JITInstructionManager.Instance.OpenJIT();
            //}
            
            WaveIndex++;
            WaveChanged?.Invoke();

            var waves = Levels[LevelIndex]?.Waves;
            if (waves == null || WaveIndex >= waves.Count())
                return;

            _EnemyDestroyed = 0;

            _ExpectedSpawnCount = waves[WaveIndex].SpawnPoints.Count();
            foreach (var spawnPoint in waves[WaveIndex].SpawnPoints)
            {
                UnityAction action = () =>
                {
                    var xPos = UnityEngine.Random.Range(-_XMax + 1, _XMax - 1);

                    var spawnPosition = Quaternion.Euler(transform.rotation.eulerAngles) * new Vector3(xPos, _YMax + 1, 0);

                    var enemyClone = Instantiate(spawnPoint.EnemyPrefab, spawnPosition, transform.rotation, transform);

                    var enemyController = enemyClone.GetComponent<EnemyController>();
                    enemyController.Screen = Screen;
                    enemyController.Destroyed += EnemyDestroyed;
                    

                    var spawningController = enemyController as ICanSpawnEnemy;
                    if (spawningController != null)
                        spawningController.SpawnedEnemies += SpawnedEnemies;

                    if (spawnPoint.JustInTime != null)
                        enemyController.StateChanged += (s, e) =>
                        {
                            if (e == EnemyState.Normal)
                            {
                                JITInstructionManager.Instance.UpdateJITController(spawnPoint.JustInTime);
                                JITInstructionManager.Instance.OpenJIT();
                            }
                        };
                    
                    EnemySpawned?.Invoke();
                };

                StartCoroutine(Util.TimedAction(null, action, spawnPoint.Time + WaveSpawnDelay));
            }
        }

        private void SpawnPickupRecursive()
        {
            var possiblePickUpTypes = new List<PickUpType>();

            foreach (var pickUpType in Enum.GetValues(typeof(PickUpType)).Cast<PickUpType>())
            {
                if (_PickUpType.HasFlag(pickUpType))
                    possiblePickUpTypes.Add(pickUpType);
            }

            //pick random wait time before spawning pickup
            var timeBeforeSpawn = UnityEngine.Random.Range(MinTimeBeforeSpawn, MaxTimeBeforeSpawn);

            UnityAction spawn = () =>
            {
                // pick random index to know which pickup to spawn
                var index = UnityEngine.Random.Range(0, possiblePickUpTypes.Count);
                var pickupToSpawn = possiblePickUpTypes[index];

                var xPos = UnityEngine.Random.Range(-_XMax + 1, _XMax - 1);

                var spawnPosition = Quaternion.Euler(transform.rotation.eulerAngles) * new Vector3(xPos, _YMax + 1, 0);

                var pickUpClone = Instantiate(PickUpPrefab, spawnPosition, transform.rotation, transform);

                var pickupController = pickUpClone.GetComponent<PickUpController>();
                pickupController.PickUpType = pickupToSpawn;
                if (pickupToSpawn == PickUpType.Trojan)
                    pickupController.SpawnedEnemies += SpawnedEnemies;
                //pickupController.Destroyed += EnemyDestroyed;

                SpawnPickupRecursive();
            };

            _PickUpCoroutine = StartCoroutine(Util.TimedAction(
                null,
                spawn,
                timeBeforeSpawn
                ));
        }

        private void SpawnedEnemies(object sender, IEnumerable<EnemyController> enemies)
        {
            EnemySpawned?.Invoke();
            _ExpectedSpawnCount += enemies.Count();
            foreach (var enemy in enemies)
            {
                enemy.Destroyed += EnemyDestroyed;
                var spawningController = enemy as ICanSpawnEnemy;
                if (spawningController != null)
                    spawningController.SpawnedEnemies += SpawnedEnemies;
            }
        }

        private void EnemyDestroyed(object sender, int points)
        {
            _EnemyDestroyed++;

            Score += points;
            ScoreChanged?.Invoke();

            if (_EnemyDestroyed == _ExpectedSpawnCount)
            {
                if (WaveIndex < Levels[LevelIndex].Waves.Count() - 1)
                    NextWave();
                else if (LevelIndex < Levels.Count() - 1)
                    NextLevel(); //TODO: Else they won, go to final screen
            }
        }

        


    }
}