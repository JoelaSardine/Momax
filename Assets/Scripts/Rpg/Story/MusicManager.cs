using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rpg
{
    public class MusicManager : MonoBehaviour
    {
        public float startAt = 0.0f;

        private void Start()
        {
            AudioSource audioSource = GetComponent<AudioSource>();
            audioSource.time = startAt;
        }
    }
}