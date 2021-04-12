using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using static SafeSurfing.Common.Constants.PlayerInput;

namespace SafeSurfing
{
    public class MenuManager : MonoBehaviour
    {
        public string NextSceneWithSpace;
        public string NextSceneWithX;
        //private AudioSource _TitleTheme;
        //public GameObject PlayableGame;
        //private float _TimeBeforeStart = 1.75f;
        //private Animator _Transition;
        // Start is called before the first frame update
        //void Start()
        //{
        //    _TitleTheme = GetComponent<AudioSource>();
        //    _Transition = GetComponent<Animator>();
        //    _TitleTheme.volume = 0.5f;
        //    _TitleTheme.Play();
        //}

        // Update is called once per frame
        void Update()
        {
            if (IsPressingSpace && !string.IsNullOrWhiteSpace(NextSceneWithSpace))
            {
                //StartCoroutine(FadeAudioSource.StartFade(_TitleTheme, _TimeBeforeStart, 0f));
                //_Transition.SetBool("Open", false);
                //StartCoroutine(StartGame());
                SceneManager.Instance.UpdateScene(NextSceneWithSpace);
            }
            if (IsPressingInteract && !string.IsNullOrWhiteSpace(NextSceneWithX))
            {
                SceneManager.Instance.UpdateScene(NextSceneWithX);
            }

        }

        //private IEnumerator StartGame(){

        //    yield return new WaitForSeconds(_TimeBeforeStart);

        //    PlayableGame.SetActive(true);
        //}
    }
}