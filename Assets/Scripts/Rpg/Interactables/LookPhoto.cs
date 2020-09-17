using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace rpg
{
    public class LookPhoto : Interactable
    {
        PlayerManager player;

        public string speech1 = "cool un pc";
        public string speech2 = "ah fb ouvert";

        protected override void Interact()
        {
            player = RpgManager.Player;

            animator.SetTrigger("Exit");
            state = State.Idle;

            Interaction_1();
        }

        private void Interaction_1()
        {
            player.Talk(speech1, Interaction_2);
        }
        private void Interaction_2()
        {
            player.Talk(speech2, LaunchFacebookConversation);
        }

        private void LaunchFacebookConversation()
        {
            RpgManager.Instance.gameState = RpgManager.Instance.gameState ^ RpgManager.GameState.Minigame;

            player.EndTalk();

            player.movementEnabled = false;
            SceneManager.LoadScene("FacebookConversation", LoadSceneMode.Additive);
            RpgManager.CameraManager.ChangeCameraOutputSize(0.5f);
        }

        public void ResetInteraction()
        {
            
        }
    }
}