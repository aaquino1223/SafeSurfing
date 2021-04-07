using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAudioManager : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource StageMusic;
    void Start()
    {
        if (StageMusic != null)
        {
            StageMusic.volume = 0f;
            StartCoroutine(FadeAudioSource.StartFade(StageMusic, 1f, 0.5f));
        }
    }

}
