using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAudioManager : MonoBehaviour
{
    // Start is called before the first frame update
    private AudioSource _StageMusic;
    void Start()
    {
        _StageMusic = GetComponent<AudioSource>();
        if(_StageMusic != null){
            _StageMusic.volume = 0f;
            StartCoroutine(FadeAudioSource.StartFade(_StageMusic, 1f, 1f));
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
