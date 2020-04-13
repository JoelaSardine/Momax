using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rpg
{
    public class WorldStory : GameStory
    {
        protected override IEnumerator Start()
        {
            yield return StartCoroutine(base.Start());

            player.attackEnabled = RpgManager.Instance.key_orion;
            
            //player.movementEnabled = false;

            //if (RpgManager.Instance.key_fb)
            {
            }
            //else
            {
                yield return new WaitForSeconds(1.0f);
                //Beginning_1();
            }
            
        }

/*        private void Beginning_1()
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
            player.Talk(tb_afterFB, player.EndTalk);
            entrance.active = true;
        }*/
    }
}