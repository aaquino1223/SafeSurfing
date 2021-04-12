using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using SafeSurfing.Common;
using System.Linq;

namespace SafeSurfing
{
    public class GUIManager : MonoBehaviour
    {
        public Image[] Lives;
        private HealthController _HealthController;

        public GameManager GameManager;

        //Level variable
        //Score variable
        public TextMeshProUGUI WaveText;
        public TextMeshProUGUI LevelText;
        public TextMeshProUGUI ScoreText;

        private void Awake()
        {
            if (GameManager != null)
            {
                GameManager.WaveChanged.AddListener(OnWaveChanged);
                GameManager.LevelChanged.AddListener(OnLevelChanged);
                GameManager.ScoreChanged.AddListener(OnScoreChanged);
                GameManager.PlayerInstantiated += GameManager_PlayerInstantiated;
            }

            SetLevelTextActive(false);
            SetWaveTextActive(false);
        }

        private void GameManager_PlayerInstantiated(object sender, PlayerController playerController)
        {
            Lives.ToList().ForEach(x => x.enabled = true);
            OnScoreChanged();

            _HealthController = playerController;
            if (_HealthController != null)
            {
                _HealthController.AddLifeLostListener(OnPlayerLifeLost, true);
                _HealthController.AddLifeGainedListener(OnPlayerLifeGained);
            }
        }

        private void SetLevelTextActive(bool value) => LevelText.gameObject.SetActive(value);
        private void SetWaveTextActive(bool value) => WaveText.gameObject.SetActive(value);

        private void OnPlayerLifeLost()
        {
            Lives[_HealthController.Lives].enabled = false;
        }

        private void OnPlayerLifeGained()
        {
            Lives[_HealthController.Lives - 1].enabled = true;
        }

        private void OnLevelChanged()
        {
            var levelNum = GameManager.LevelIndex + 1;
            LevelText.text = "Level " + levelNum;

            StartCoroutine(Util.TimedAction(() => SetLevelTextActive(true), () => SetLevelTextActive(false), GameManager.WaveSpawnDelay));
        }

        private void OnWaveChanged()
        {
            //comes from enemyspawner
            var waveNum = GameManager.WaveIndex + 1;
            WaveText.text = "Wave " + waveNum;
            StartCoroutine(Util.TimedAction(() => SetWaveTextActive(true), () => SetWaveTextActive(false), GameManager.WaveSpawnDelay));
        }

        private void OnScoreChanged()
        {
            // Add Score variable
           ScoreText.text = "Score: " + GameManager.Score;
        }


        // Update is called once per frame
        void Update()
        {

        }
    }
}