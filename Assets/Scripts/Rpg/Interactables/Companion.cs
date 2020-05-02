using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rpg
{
    public class Companion : Interactable
    {
        PlayerManager player;

        public GameObject character;

        [TextArea]
        public List<string> talks;
        private int currentTalk = 0;

        public Sprite sprite;
        public bool isAltea; // or Orion

        protected override void Interact()
        {
            player = RpgManager.Player;

            animator.SetTrigger("Exit");
            state = State.Idle;

            RpgManager.Instance.discussionInterface.SetImage(false, sprite);

            DoTalk();
        }
               
        private void DoTalk()
        {
            if (currentTalk < talks.Count)
            {
                string t = talks[currentTalk];
                player.Dialog(t.Split(':')[0] == "Morgane ", t, DoTalk);
                currentTalk++;
            }
            else
            {
                player.EndTalk();
                currentTalk = 0;

                EndInteraction();
            }
        }

        private void EndInteraction()
        {
            if (isAltea)
            {
                RpgManager.SetKey(SaveKey.metAltea, 1);
            }
            else
            {
                RpgManager.SetKey(SaveKey.metOrion, 1);
                player.attackEnabled = true;
            }

            character.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}