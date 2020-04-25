using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rpg
{
    public class TalkInteractableAuto : Autointeractable
    {
        PlayerManager player;

        public List<string> talks;
        private int currentTalk = 0;

        protected override void TriggerEnter(Collider2D collider)
        {
            player = RpgManager.Player;
            
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

                if (onEndInteraction != null)
                {
                    onEndInteraction();
                }
            }
        }
    }
}