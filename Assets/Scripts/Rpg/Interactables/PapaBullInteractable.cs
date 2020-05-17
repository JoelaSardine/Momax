using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rpg
{
    public class PapaBullInteractable : TalkInteractableAuto
    {
        public bool isPlayerIn = false;
        public bool isDialogue = true;

        protected override void TriggerEnter(Collider2D collider)
        {
            isPlayerIn = true;

            if (isDialogue)
            {
                base.TriggerEnter(collider);
            }
            else
            {
                if (onEndInteraction != null)
                {
                    onEndInteraction();
                }
            }
        }

        protected override void TriggerExit(Collider2D collider)
        {
            if (isDialogue)
            {
                base.TriggerExit(collider);
            }

            isPlayerIn = false;
        }
    }
}