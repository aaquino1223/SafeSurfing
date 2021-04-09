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

    

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsPressingSpace){
            CloseJIT();
        }
                
    }

    public void UpdateJITController(string title, string text){
        TitleText.text = title;
        TextToShow.text = text;
    }

    public void OpenJIT(){
        gameObject.SetActive(true);
        //pause game
        // Time.timeScale = 0f;
    }

    public void CloseJIT(){
        gameObject.SetActive(false);
    }


    }

}
