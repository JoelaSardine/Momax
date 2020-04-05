using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rpg
{
    public class NeuillyPlaisanceStory : GameStory
    {
        private PlayerManager player;

        public Interactable entrance;
        public Interactable computer;

        public string tb_beginning_1 = "Ah, ça fait du bien de rentrer chez soi...";
        public string tb_beginning_2 = "...après une bonne journée de travail !";

        private IEnumerator Start()
        {
            if (!RpgManager.Instance)
            {
                yield return new WaitForEndOfFrame();
            }

            RpgManager.currentStory = this;
            player = RpgManager.Player;

            player.attackEnabled = false;

            if (RpgManager.Instance.key_fb)
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
            RpgManager.Instance.key_fb = true;

            computer.active = false;
            player.Talk("Je dois retrouver Maxime !", player.EndTalk);
            entrance.active = true;
        }
    }
}