using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rpg
{
    public class NeuillyPlaisanceStory : GameStory
    {
        public AudioClip musicAfterFb;

        public Interactable entrance;
        public Interactable computer;

        public string tb_beginning_1 = "Ah, ça fait du bien de rentrer chez soi...";
        public string tb_beginning_2 = "...après une bonne journée de travail !";

        public string tb_afterFB_1 = "Je dois retrouver Maxime ! ...";
        public string tb_afterFB_2 = "Allez, direction Limoges !";

        protected override IEnumerator Start()
        {
            yield return StartCoroutine(base.Start());

            yield return StartCoroutine(InitializeScene(false));
        }

        protected override IEnumerator InitializeScene(bool waitFrame)
        {
            if (waitFrame)
            {
                yield return new WaitForEndOfFrame();
            }

            player.attackEnabled = false;

            computer.active = RpgManager.GetKey(SaveKey.facebookDone) != 1;
            entrance.active = RpgManager.GetKey(SaveKey.facebookDone) == 1;

            if (RpgManager.GetKey(SaveKey.facebookDone) == 0)
            {
                player.movementEnabled = false;
                entrance.active = false;
                yield return StartCoroutine(Beginning());
            }
        }

        private IEnumerator Beginning()
        {
            yield return new WaitForSeconds(1);

            bool wait = true;
            player.Talk(tb_beginning_1, () => wait = false);
            yield return new WaitWhile(() => wait == true);

            wait = true;
            player.Talk(tb_beginning_2, () => wait = false);
            yield return new WaitWhile(() => wait == true);

            player.EndTalk();
            RpgManager.SetKey(SaveKey.facebookDone, -1);
        }

        public void OnEndFacebook()
        {
            RpgManager.SetKey(SaveKey.facebookDone, 1);

            computer.active = false;
            player.Talk(tb_afterFB_1, OnEndFacebook2);
        }

        private void OnEndFacebook2()
        {
            player.Talk(tb_afterFB_2, player.EndTalk);
            entrance.active = true;
        }
    }
}