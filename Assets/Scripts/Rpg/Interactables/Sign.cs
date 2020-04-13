using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rpg
{
    public class Sign : Interactable
    {
        PlayerManager player;

        protected override void Interact()
        {
            player = RpgManager.Player;

            animator.SetTrigger("Exit");
            state = State.Idle;

            Interaction_1();
        }

        private void Interaction_1()
        {
            return;
        }
    }
}