using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static SafeSurfing.Common.Constants.PlayerInput;

namespace SafeSurfing
{
    public class JITInstructionManager : MonoBehaviour
    {
        // Start is called before the first frame update
        public TextMeshProUGUI TitleText;
        public TextMeshProUGUI TextToShow;

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
            if (IsPressingSpace)
            {
                CloseJIT();
            }

        }

        public void UpdateJITController(JustInTimeInstruction jit)
        {
            TitleText.text = jit.Title;
            TextToShow.text = jit.Text;
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
