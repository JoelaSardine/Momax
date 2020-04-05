﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace rpg
{
    public class Computer : Interactable
    {
        PlayerManager player;

        public string speech1 = "cool un pc";
        public string speech2 = "ah fb ouvert";

        protected override void Interact()
        {
            player = RpgManager.Player;

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
            player.EndTalk();

            player.movementEnabled = false;
            RpgManager.CameraManager.ChangeCameraOutputSize(0.5f);

            animator.SetTrigger("Exit");
            state = State.Idle;

            SceneManager.LoadScene("FacebookConversation", LoadSceneMode.Additive);
        }

        public void ResetInteraction()
        {
            
        }
    }
}