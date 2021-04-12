﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static SafeSurfing.Common.Constants.PlayerInput;

namespace SafeSurfing
{
    public class JITInstructionManager : MonoBehaviour
    {
        // Start is called before the first frame update
        public TextMeshProUGUI TitleText;
        public TextMeshProUGUI TextToShow;
        public Image JITImage;
        public GameObject Background;

        public static JITInstructionManager Instance;
        

        void Awake()
        {
            if (Instance == null)
                Instance = this;
        }

        private void Start()
        {
            CloseJIT();
        }

        // Update is called once per frame
        void Update()
        {
            if (IsPressingInteract)
            {
                CloseJIT();
            }
        }

        public void UpdateJITController(JustInTimeInstruction jit, Sprite sprite = null)
        {
            TitleText.text = jit.Title;
            TextToShow.text = jit.Text;
            if (sprite == null)
                JITImage.enabled = false;
            else
            {
                JITImage.enabled = true;
                JITImage.sprite = sprite;
            }
        }

        public void OpenJIT()
        {
            gameObject.SetActive(true);
            //pause game
            Time.timeScale = 0f;
        }

        public void CloseJIT()
        {
            if (gameObject.activeInHierarchy)
            {
                gameObject.SetActive(false);
                Time.timeScale = 1f;
            }
        }
    }
}
