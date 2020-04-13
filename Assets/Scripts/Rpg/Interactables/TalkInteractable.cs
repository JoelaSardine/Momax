using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rpg
{
    public class TalkInteractable : Interactable
    {
        PlayerManager player;

        public List<string> talks;
        private int currentTalk = 0;

        protected override void Interact()
        {
            player = RpgManager.Player;

            animator.SetTrigger("Exit");
            state = State.Idle;

            DoTalk();
        }

        private void DoTalk()
        {
            if (currentTalk < talks.Count)
            {
                player.Talk(talks[currentTalk], DoTalk);
                currentTalk++;
            }
            else
            {
                player.EndTalk();
                currentTalk = 0;
            }
        }
    }
}