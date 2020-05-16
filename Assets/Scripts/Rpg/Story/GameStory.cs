using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rpg
{
    public class GameStory : MonoBehaviour
    {
        protected AudioSource audioSource;

        protected PlayerManager player;

        protected virtual IEnumerator Start()
        {
            if (!audioSource)
            {
                audioSource = GetComponent<AudioSource>();
            }

            if (!RpgManager.Instance)
            {
                yield return new WaitForEndOfFrame();
            }

            RpgManager.CurrentStory = this;
            RpgManager.Instance.gameState = RpgManager.GameState.Rpg;
            player = RpgManager.Player;
        }

        protected virtual IEnumerator InitializeScene(bool waitFrame)
        {
            yield return null;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1) || Input.GetKeyDown(KeyCode.F2)
                 || Input.GetKeyDown(KeyCode.F3) || Input.GetKeyDown(KeyCode.F4)
                  || Input.GetKeyDown(KeyCode.F5) || Input.GetKeyDown(KeyCode.F6)
                   || Input.GetKeyDown(KeyCode.F7) || Input.GetKeyDown(KeyCode.F8)
                    || Input.GetKeyDown(KeyCode.F9) || Input.GetKeyDown(KeyCode.F10)
                     || Input.GetKeyDown(KeyCode.F11) || Input.GetKeyDown(KeyCode.F12))
            {
                StartCoroutine(InitializeScene(true));
            }
        }

        public void StopMusic()
        {
            audioSource.Stop();
        }

        public float GetMusicVolume()
        {
            return audioSource.volume;
        }

        public void SetMusicVolume(float v)
        {
            audioSource.volume = v;
        }

        public void SetMusicPitch(float v)
        {
            audioSource.pitch = v;
        }
    }
}