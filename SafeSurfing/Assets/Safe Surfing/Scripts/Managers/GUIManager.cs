using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SafeSurfing
{
    public class GUIManager : MonoBehaviour
    {
        public Image[] Lives;
        public GameObject Player;
        private HealthController _HealthController;
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
        }

        private void OnPlayerLifeLost()
        {
            Lives[_HealthController.Lives].enabled = false;
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}