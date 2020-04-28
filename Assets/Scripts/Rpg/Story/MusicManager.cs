using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rpg
{
    public class MusicManager : MonoBehaviour
    {
        public float startAt = 0.0f;
        public float fadeDuration = 1.0f;

        private IEnumerator Start()
        {
            AudioSource audioSource = GetComponent<AudioSource>();
            audioSource.time = startAt;
            
            if (fadeDuration == 0.0f)
                yield break;

            float targetVolume = audioSource.volume;
            float step = targetVolume / fadeDuration;
            audioSource.volume = 0;

            while (audioSource.volume < targetVolume)
            {
                audioSource.volume = Mathf.Min(targetVolume, audioSource.volume + step * Time.deltaTime);
                yield return null;
            }
        }


    }
}