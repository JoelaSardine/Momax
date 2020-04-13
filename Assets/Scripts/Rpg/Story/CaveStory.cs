using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rpg
{
    public class CaveStory : GameStory
    {
        public GameObject blackVeil;
        public GameObject snakeToHide;
        public Teleporter entrance1;
        public Teleporter entrance2;

        public string tb_toodark_1 = "Ouh, il fait tout noir là-dedans !";
        public string tb_toodark_2 = "J'y vois rien, je ressors.";

        protected override IEnumerator Start()
        {
            yield return StartCoroutine(base.Start());

            player.attackEnabled = RpgManager.Instance.key_orion;

            if (RpgManager.Instance.key_altea)
            {
                blackVeil.SetActive(false);
            }
            else {
                snakeToHide.SetActive(false);
                player.movementEnabled = false;
                ItsTooDark_1();
            }
        }

        private void ItsTooDark_1()
        {
            player.Talk(tb_toodark_1, ItsTooDark_2);
        }
        private void ItsTooDark_2()
        {
            player.Talk(tb_toodark_2, ItsTooDark_3);
        }
        private void ItsTooDark_3()
        {
            player.EndTalk();

            float dist1 = Vector3.Distance(entrance1.transform.position, player.transform.position);
            float dist2 = Vector3.Distance(entrance2.transform.position, player.transform.position);
            Teleporter nearest = dist2 < dist1 ? entrance2 : entrance1;

            RpgManager.LoadScene(nearest.targetScene, nearest.targetSpawnPoint);
        }
    }
}