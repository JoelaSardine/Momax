using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rpg
{
    public class WorldStory : GameStory
    {
        public PapaBullInteractable papaBullEvent;
        public Papabull papaBullCreature;
        public TalkInteractableAuto snakeBefore;
        public TalkInteractableAuto snakeAfter;
        public Companion altea;
        public TalkInteractableAuto entreeMontgeron;
        public TalkInteractable maisonMontgeron;
        public Teleporter teleporterMontgeron;

        private void Awake()
        {
            papaBullEvent.onEndInteraction += () => {
                if (RpgManager.GetKey(SaveKey.seenBull) != 1)
                {
                    RpgManager.SetKey(SaveKey.seenBull, 1);
                    snakeBefore.gameObject.SetActive(false);
                    bool b = RpgManager.GetKey(SaveKey.seenSnake) != 1 && RpgManager.GetKey(SaveKey.metAltea) == 0;
                    snakeAfter.gameObject.SetActive(b);
                }
            };

            snakeBefore.onEndInteraction += () => {
                RpgManager.SetKey(SaveKey.seenSnake, -1);
                snakeBefore.gameObject.SetActive(false);
                snakeAfter.gameObject.SetActive(false);
            };
            snakeAfter.onEndInteraction += () => {
                RpgManager.SetKey(SaveKey.seenSnake, 1);
                snakeBefore.gameObject.SetActive(false);
                snakeAfter.gameObject.SetActive(false);
            };

            entreeMontgeron.onEndInteraction += () => {
                RpgManager.SetKey(SaveKey.seenMontgeron, 1);
                entreeMontgeron.gameObject.SetActive(false);
                papaBullCreature.gameObject.SetActive(false);
                RpgManager.SaveGame("Montgeron");
            };

            maisonMontgeron.onEndInteraction += () => {
                RpgManager.SetKey(SaveKey.seenHouse, 1);
                maisonMontgeron.gameObject.SetActive(false);
                teleporterMontgeron.gameObject.SetActive(true);
            };
        }

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

            player.attackEnabled = RpgManager.GetKey(SaveKey.metOrion) == 1;

            papaBullEvent.isDialogue =  (RpgManager.GetKey(SaveKey.metOrion) != 1);
            snakeBefore.gameObject.SetActive(RpgManager.GetKey(SaveKey.metAltea) != 1 
                && RpgManager.GetKey(SaveKey.seenBull) == 0 && RpgManager.GetKey(SaveKey.seenSnake) == 0);
            snakeAfter.gameObject.SetActive(RpgManager.GetKey(SaveKey.metAltea) != 1 
                && RpgManager.GetKey(SaveKey.seenBull) == 1 && RpgManager.GetKey(SaveKey.seenSnake) != 1);

            altea.gameObject.SetActive(RpgManager.GetKey(SaveKey.metAltea) != 1);

            entreeMontgeron.gameObject.SetActive(RpgManager.GetKey(SaveKey.seenMontgeron) != 1);

            maisonMontgeron.gameObject.SetActive(RpgManager.GetKey(SaveKey.seenMontgeron) != 1);
            teleporterMontgeron.gameObject.SetActive(RpgManager.GetKey(SaveKey.seenMontgeron) == 1);

            papaBullCreature.gameObject.SetActive(RpgManager.GetKey(SaveKey.seenMontgeron) != 1);
        }
    }
}