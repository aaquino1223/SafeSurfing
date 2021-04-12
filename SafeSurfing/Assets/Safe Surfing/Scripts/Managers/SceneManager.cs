using SafeSurfing.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SafeSurfing
{
    public class SceneManager : MonoBehaviour
    {
        [Header("Start Menu")]
        public GameObject StartMenu;
        public float TimeAfterMenu = 1.75f;
        public AudioClip StartMenuClip;

        [Header("Game")]
        public GameObject PlayableGame;
        public float TimeAfterGame = 6f;

        [Header("Victory")]
        public GameObject VictoryScreen;
        public float TimeAfterVictory;
        public AudioClip VictoryClip;

        [Header("Lost")]
        public GameObject LostScreen;
        public float TimeAfterLost;
        public AudioClip LostClip;

        private GameObject _Current;
        private float _CurrentTimeAfter = 0f;

        public static SceneManager Instance;

        public AudioPlayer AudioPlayer;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
        }

        // Start is called before the first frame update
        void Start()
        {
            _Current = StartMenu;
            _CurrentTimeAfter = TimeAfterMenu;
            AudioPlayer.PlayBackground(StartMenuClip);
        }

        public void UpdateScene(string sceneName)
        {
            Animator animator;

            var previous = _Current;
            var previousTimeAfter = _CurrentTimeAfter;

            AudioClip audioClip = null;
            AudioClip oneShotAudioClip = null;
            switch (sceneName)
            {
                case "Menu":
                    _Current = StartMenu;
                    _CurrentTimeAfter = TimeAfterMenu;
                    audioClip = StartMenuClip;
                    break;
                case "Game":
                    _Current = PlayableGame;
                    _CurrentTimeAfter = TimeAfterGame;
                    break;
                case "Victory":
                    _Current = VictoryScreen;
                    _CurrentTimeAfter = TimeAfterVictory;
                    oneShotAudioClip = VictoryClip;
                    break;
                case "Lost":
                    _Current = LostScreen;
                    _CurrentTimeAfter = TimeAfterLost;
                    oneShotAudioClip = LostClip;
                    break;
            }

            if (previous == _Current)
                return;

            if (previous != null)
            {
                if (previous.TryGetComponent(out animator))
                    animator.SetBool("Open", false);

                StartCoroutine(Util.TimedAction(null, () => previous.SetActive(false), previousTimeAfter));
            }

            StartCoroutine(Util.TimedAction(null, () =>
            {
                _Current.SetActive(true);
                if (_Current.TryGetComponent(out animator))
                    animator.SetBool("Open", true);

                AudioPlayer.StopBackground();
                if (audioClip != null)
                    AudioPlayer.PlayBackground(audioClip);
                else if (oneShotAudioClip != null)
                    AudioPlayer.PlayOneShot(oneShotAudioClip);
            },
            previousTimeAfter));


        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}