using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rpg
{
    public class TalkInteractable : Interactable
    {
        public enum TalkType { bubble, dialogue }
        
        public TalkType talkType = TalkType.bubble;
        PlayerManager player;

        public List<string> talks;
        private int currentTalk = 0;

        public Sprite spriteOrion;
        public Sprite spriteAltea;
        public Sprite spriteMax;
        public Sprite spriteLucky;

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

                if (talkType == TalkType.dialogue)
                {
                    string t = talks[currentTalk];
                    string talker = t.Split(':')[0];

                    if (talker == "Altéa ")
                        RpgManager.Instance.discussionInterface.SetImage(false, spriteAltea);
                    else if (talker == "Orion ")
                        RpgManager.Instance.discussionInterface.SetImage(false, spriteOrion);
                    else if (talker == "Max ")
                        RpgManager.Instance.discussionInterface.SetImage(false, spriteMax);
                    else if (talker == "Lucky ")
                        RpgManager.Instance.discussionInterface.SetImage(false, spriteLucky);

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