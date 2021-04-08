using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

namespace SafeSurfing
{
    public class GUIManager : MonoBehaviour
    {
        public Image[] Lives;
        public GameObject Player;
        private HealthController _HealthController;
        public EnemySpawner Spawner;

        //Level variable
        //Score variable
        public TextMeshProUGUI WaveText;
        public TextMeshProUGUI LevelText;
        public TextMeshProUGUI ScoreText;

        // Start is called before the first frame update
        void Start()
        {
            if (Player == null)
                Player = GameObject.FindGameObjectWithTag("Player");

            if (Player == null)
                return;

            _HealthController = Player.GetComponent<HealthController>();
            if (_HealthController != null)
                _HealthController.AddLifeLostListener(OnPlayerLifeLost, true);

            if(Spawner != null)
            {
                Spawner.WaveChanged.AddListener(OnWaveChanged);
                Spawner.LevelChanged.AddListener(OnLevelChanged);
                Spawner.ScoreChanged.AddListener(OnScoreChanged);
            }
        }

        private void OnPlayerLifeLost()
        {
            Lives[_HealthController.Lives].enabled = false;
        }

        private void OnWaveChanged()
        {
            //comes from enemyspawner
            var waveNum = Spawner.WaveIndex + 1;
            WaveText.text = "Wave " + waveNum;
        }

        private void OnLevelChanged()
        {
            // Add Level variable
            // LevelText.text = "Level " + Level;
        }

        private void OnScoreChanged()
        {
            // Add Score variable
            // ScoreText.text = "Score: " + Score;
        }


        // Update is called once per frame
        void Update()
        {

        }
    }
}