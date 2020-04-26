using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rpg
{
    public class WorldStory : GameStory
    {
        public Papabull papaBullCreature;
        public PapaBullInteractable papaBullEvent;
        public TalkInteractableAuto snakeBefore;
        public TalkInteractableAuto snakeAfter;
        public Companion altea;
        public TalkInteractableAuto entreeMontgeron;


        private void Awake()
        {
            papaBullEvent.onEndInteraction += () => {
                if (!RpgManager.Instance.key_blockedRoad)
                {
                    RpgManager.Instance.key_blockedRoad = true;
                    snakeBefore.gameObject.SetActive(false);
                    snakeAfter.gameObject.SetActive(RpgManager.Instance.key_seenSnake == -1 && !RpgManager.Instance.key_altea);
                }
            };

            snakeBefore.onEndInteraction += () => {
                RpgManager.Instance.key_seenSnake = -1;
                snakeBefore.gameObject.SetActive(false);
                snakeAfter.gameObject.SetActive(false);
            };
            snakeAfter.onEndInteraction += () => {
                RpgManager.Instance.key_seenSnake = 1;
                snakeBefore.gameObject.SetActive(false);
                snakeAfter.gameObject.SetActive(false);
            };

            entreeMontgeron.onEndInteraction += () =>
            {
                RpgManager.Instance.key_montgeron = true;
                entreeMontgeron.gameObject.SetActive(false);
            };
        }

        protected override IEnumerator Start()
        {
            yield return StartCoroutine(base.Start());

            player.attackEnabled = RpgManager.Instance.key_orion;

            papaBullEvent.gameObject.SetActive(!RpgManager.Instance.key_orion);
            snakeBefore.gameObject.SetActive(RpgManager.Instance.key_seenSnake == 0 && !RpgManager.Instance.key_blockedRoad);
            snakeAfter.gameObject.SetActive(RpgManager.Instance.key_seenSnake == 0 && RpgManager.Instance.key_blockedRoad);

            entreeMontgeron.gameObject.SetActive(!RpgManager.Instance.key_montgeron);

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