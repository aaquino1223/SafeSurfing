using SafeSurfing.Common;
using SafeSurfing.Common.Enums;
using SafeSurfing.Common.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace SafeSurfing 
{
    public class BossController : VirusController, ICanSpawnEnemy
    {
        public float Frequency = 1f;
        public float Amplitude = 1f;

        private float _YZero;
        private float _Direction = 1;

        public LevelBehavior Level;

        public event EventHandler<IEnumerable<EnemyController>> SpawnedEnemies;

        private int _WaveIndex = -1;
        private int _WaveIncrements;

        protected override void Initialize()
        {
            base.Initialize();

            IgnoreBounds = true;

            var waveCount = Level.Waves.Count();

            _WaveIncrements = Lives / (waveCount + 1);

            AddLifeLostListener(OnBossLifeLost);
        }

        private void OnBossLifeLost()
        {
            if (Lives % _WaveIncrements == 0)
                NextWave();
        }

        private void NextWave()
        {
            _WaveIndex++;

            var waves = Level.Waves;
            if (waves == null || _WaveIndex >= waves.Count())
                return;

            var wave = waves[_WaveIndex];

            foreach (var spawnPoint in wave.SpawnPoints)
            {
                UnityAction action = () =>
                {
                    var xPos = UnityEngine.Random.Range(-_XMax + 1, _XMax - 1);

                    var spawnPosition = Quaternion.Euler(transform.rotation.eulerAngles) * new Vector3(xPos, _YMax + 1, 0);

                    var enemyClone = Instantiate(spawnPoint.EnemyPrefab, spawnPosition, transform.rotation, transform.parent);

                    var enemyController = enemyClone.GetComponent<EnemyController>();

                    var spawningController = enemyController as ICanSpawnEnemy;
                    if (spawningController != null)
                        spawningController.SpawnedEnemies += (s, e) => SpawnedEnemies?.Invoke(s, e);

                    SpawnedEnemies?.Invoke(this, new List<EnemyController>() { enemyController });
                };

                StartCoroutine(Util.TimedAction(null, action, spawnPoint.Time));
            }
        }

        protected override IEnumerable<Vector3> CreateMovementPattern()
        {
            var pattern = new List<Vector3>();
            switch (State)
            {
                case EnemyState.Spawned:
                    _YZero = _YMax - 3f;
                    pattern.Add(new Vector3(transform.localPosition.x, _YZero, 0));
                    break;
                case EnemyState.Normal:
                    var currentX = transform.localPosition.x;

                    if (currentX <= -_XMax + 1f || currentX >= _XMax - 1f)
                        _Direction = -_Direction;
                    var x = transform.localPosition.x + _Direction * 0.25f;

                    var y = _YZero + Mathf.Sin(x * Frequency) * Amplitude;

                    pattern.Add(new Vector3(x, y, 0));
                    break;
            }

            return pattern;
        }

        protected override void OnPatternCompleted()
        {
            base.OnPatternCompleted();

            if (State == EnemyState.Normal)
                SetPattern();
        }

        protected override void OnTriggerCollison(Collider2D collision)
        {
            base.OnTriggerCollison(collision);

        }
    }
}