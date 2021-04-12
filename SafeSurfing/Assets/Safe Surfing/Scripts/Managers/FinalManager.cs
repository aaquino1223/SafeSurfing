using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace SafeSurfing
{
    public class FinalManager : MonoBehaviour
    {
        public GameManager GameManager;
        public TextMeshProUGUI WinLossText;
        public TextMeshProUGUI ScoreText;

        // Start is called before the first frame update
        void OnEnable()
        {
            ScoreText.text = "Final Score: " + GameManager.Score;

            WinLossText.text = GameManager.GameWon ? "You Won!" : "You Lost...";
        }
    }
}