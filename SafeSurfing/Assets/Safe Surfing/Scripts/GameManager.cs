using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using SafeSurfing.Common;
using UnityEngine.Events;
using SafeSurfing.Common.Enums;
using System;

namespace SafeSurfing
{
    public class GameManager : MonoBehaviour
    {
        public GameObject Screen;

        private float _XMax;
        private float _YMax;

        public UnityEvent EnemySpawned;
        public UnityEvent WaveChanged;
        public UnityEvent LevelChanged;
        public UnityEvent ScoreChanged;

        public LevelBehavior[] Levels;
        public int WaveIndex { get; private set; } = -1;
        public int LevelIndex { get; private set; } = -1;

        private int _EnemyDestroyed = 0;
        private int _ExpectedSpawnCount = 0;

        private PickUpType _PickUpType;

        public int Score { get; private set; } = 0;

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
            LevelIndex++;
            LevelChanged?.Invoke();

            if (Levels == null || LevelIndex >= Levels.Count())
                return;

            _PickUpType = Levels[LevelIndex].PickUpType;

            NextWave();
        }

        private void NextWave()
        {
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

                    EnemySpawned?.Invoke();
                };

                StartCoroutine(Util.TimedAction(null, action, spawnPoint.Time));
            }
        }

        //private IEnumerator SpawnPickups()
        //{
        //    var possiblePickUpTypes = new List<PickUpType>();

        //    foreach (var pickUpType in Enum.GetValues(typeof(PickUpType)).Cast<PickUpType>())
        //        if (_PickUpType.HasFlag(pickUpType))
        //            possiblePickUpTypes.Add(pickUpType);

        //    //float currentTime = 0;
        //    //float start = audioSource.volume;

        //    //while (currentTime < duration)
        //    //{
        //    //    currentTime += Time.deltaTime;
        //    //    audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
        //    //    yield return null;
        //    //}
        //    //yield break;
        //}
       
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