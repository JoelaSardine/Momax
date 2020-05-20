using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace rpg
{
    public class Platine : Interactable
    {
        PlayerManager player;

        public List<AudioClip> audioClips;
        public List<string> audioNames;
        public int counter;

        protected override void Interact()
        {
            player = RpgManager.Player;

            animator.SetTrigger("Exit");
            state = State.Idle;

            Interaction();
        }

        private void Interaction()
        {
            counter++;
            if (counter >= audioClips.Count)
            {
                counter = 0;
            }

            RpgManager.CurrentStory.SetMusic(audioClips[counter]);

            player.Talk(audioNames[counter], () => player.EndTalk());
        }
    }
}