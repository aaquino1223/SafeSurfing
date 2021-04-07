using SafeSurfing.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SafeSurfing
{
    [CreateAssetMenu]
    public class AudioPlayer : ScriptableObject
    {
        public void PlayOneShot(AudioClip audioClip)
        {
            var gameObject = new GameObject("Sound");
            var behavior = gameObject.AddComponent<CoroutineBehavior>();
            var audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.volume = 1f;

            behavior.StartCoroutine(Util.TimedAction(
                () => audioSource.PlayOneShot(audioClip),
                () => Destroy(gameObject),
                audioClip.length
                ));
        }

        public class CoroutineBehavior : MonoBehaviour
        {

        }

    }

}