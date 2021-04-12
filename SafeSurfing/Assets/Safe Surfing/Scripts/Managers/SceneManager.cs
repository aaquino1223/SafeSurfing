﻿using SafeSurfing.Common;
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

        [Header("Credits")]
        public GameObject CreditsScreen;
        public float TimeAfterCredits;
        public AudioClip CreditsClip;

        private GameObject _Current;
        private float _CurrentTimeAfter = 0f;

        private GameObject _LastFinish;
        private float _LastTimeAfterFinish;
        private AudioClip _LastFinishAudio;

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
                    _LastFinish = _Current = VictoryScreen;
                    _LastTimeAfterFinish = _CurrentTimeAfter = TimeAfterVictory;
                    _LastFinishAudio = oneShotAudioClip = VictoryClip;
                    break;
                case "Lost":
                    _LastFinish = _Current = LostScreen;
                    _LastTimeAfterFinish = _CurrentTimeAfter = TimeAfterLost;
                    _LastFinishAudio = oneShotAudioClip = LostClip;
                    break;
                case "Finish":
                    _Current = _LastFinish;
                    _CurrentTimeAfter = _LastTimeAfterFinish;
                    oneShotAudioClip = _LastFinishAudio;
                    break;
                case "Credits":
                    _Current = CreditsScreen;
                    _CurrentTimeAfter = TimeAfterCredits;
                    audioClip = CreditsClip;
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

                if (audioClip != null)
                    AudioPlayer.PlayBackground(audioClip);
                else if (oneShotAudioClip != null)
                {
                    AudioPlayer.StopBackground();
                    AudioPlayer.PlayOneShot(oneShotAudioClip);
                }
            },
            previousTimeAfter));


        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}