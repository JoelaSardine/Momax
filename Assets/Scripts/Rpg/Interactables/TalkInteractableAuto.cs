using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rpg
{
    public class TalkInteractableAuto : Autointeractable
    {
        public enum TalkType { bubble, dialogue }

        public TalkType talkType = TalkType.bubble;
        PlayerManager player;

        public List<string> talks;
        private int currentTalk = 0;

        public Sprite spriteOrion;
        public Sprite spriteAltea;

        public float delay = 0;

        protected override void TriggerEnter(Collider2D collider)
        {
            player = RpgManager.Player;

            if (delay <= 0)
            {
                DoTalk();
            }
            else
            {
                player.movementEnabled = false;
                StartCoroutine(DelayedDoTalk());
            }
        }

        private IEnumerator DelayedDoTalk()
        {
            yield return new WaitForSeconds(delay);
            DoTalk();
        }

        private void DoTalk()
        {
            if (currentTalk < talks.Count)
            {
                if (talkType == TalkType.dialogue)
                {
                    string t = talks[currentTalk];
                    string talker = t.Split(':')[0];

                    if (talker == "Altéa ")
                        RpgManager.Instance.discussionInterface.SetImage(false, spriteAltea);
                    else if (talker == "Orion ")
                        RpgManager.Instance.discussionInterface.SetImage(false, spriteOrion);

                    player.Dialog(talker == "Morgane ", t, DoTalk);
                }
                else
                {
                    player.Talk(talks[currentTalk], DoTalk);
                }
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