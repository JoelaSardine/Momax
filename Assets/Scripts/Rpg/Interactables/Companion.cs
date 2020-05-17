using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rpg
{
    public class Companion : Interactable
    {
        public AudioClip sfx_jingle;

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

                StartCoroutine(EndInteraction());
            }
        }

        private IEnumerator EndInteraction()
        {
            player.movementEnabled = false;

            MovableEntity movable = GetComponentInChildren<MovableEntity>();
            var rb = movable.GetComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Dynamic;
            movable.MoveTo(RpgManager.Player.transform.position);

            yield return new WaitWhile(() => movable.isMoving);
            character.SetActive(false);

            if (isAltea)
            {
                RpgManager.SetKey(SaveKey.metAltea, 1);
            }
            else
            {
                RpgManager.SetKey(SaveKey.metOrion, 1);
                player.attackEnabled = true;
            }

            RpgManager.SaveGame(isAltea ? "SPA" : "Orion");

            float oldvolume = RpgManager.CurrentStory.GetMusicVolume();
            RpgManager.CurrentStory.SetMusicVolume(0);
            RpgManager.PlaySFX(sfx_jingle);
            bool wait = true;
            player.Talk((isAltea ? "Altea" : "Orion") + " rejoint l'equipe !", () => wait = false);
            yield return new WaitForSeconds(sfx_jingle.length - 1);
            yield return new WaitWhile(() => wait);

            gameObject.SetActive(false);

            player.EndTalk();
            RpgManager.CurrentStory.SetMusicVolume(oldvolume);
        }
    }
}