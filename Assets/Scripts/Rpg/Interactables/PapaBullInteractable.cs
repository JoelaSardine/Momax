using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rpg
{
    public class PapaBullInteractable : TalkInteractableAuto
    {
        public bool isPlayerIn = false;

        protected override void TriggerEnter(Collider2D collider)
        {
            isPlayerIn = true;

            base.TriggerEnter(collider);
        }

        protected override void TriggerExit(Collider2D collider)
        {
            base.TriggerExit(collider);

            isPlayerIn = false;
        }
    }
}