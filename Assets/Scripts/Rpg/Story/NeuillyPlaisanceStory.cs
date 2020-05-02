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
            
            player.attackEnabled = false;

            if (RpgManager.GetKey(SaveKey.facebookDone) == 1)
            {
                computer.active = false;
            }
            else
            {
                entrance.active = false;
                player.movementEnabled = false;
                yield return new WaitForSeconds(1.0f);
                Beginning_1();
            }
        }

        private void Beginning_1()
        {
            player.Talk(tb_beginning_1, Beginning_2);
        }
        private void Beginning_2()
        {
            player.Talk(tb_beginning_2, Beginning_3);
        }
        private void Beginning_3()
        {
            player.EndTalk();
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
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                RpgManager.SetKey(SaveKey.facebookDone, 0);

                computer.active = true;
                entrance.active = false;
                player.movementEnabled = false;

                Beginning_1();

                RpgManager.ZoneDisplayName("Cheat - Beginning");
            }
            else if (Input.GetKeyDown(KeyCode.F2))
            {
                OnEndFacebook();

                RpgManager.ZoneDisplayName("Cheat - facebookDone set to 1");
            }
        }
    }
}