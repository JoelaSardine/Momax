using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rpg
{
    public class NeuillyPlaisanceStory : MonoBehaviour
    {
        private PlayerManager player;

        public Interactable entrance;
        
        public string tb_beginning_1 = "Ah, ça fait du bien de rentrer chez soi...";
        public string tb_beginning_2 = "...après une bonne journée de travail !";

        private IEnumerator Start()
        {
            //RpgManager.Player.movementEnabled = false;
            entrance.active = false;

            yield return new WaitForEndOfFrame();
            player = RpgManager.Player;

            Beginning_1();
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
    }
}