using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace rpg
{
    public class Computer : Interactable
    {
        protected override void Interact()
        {
            RpgManager.Instance.player.movementEnabled = false;
            GameManager.CameraManager.ChangeCameraOutputSize(0.5f);

            animator.SetTrigger("Exit");
            state = State.Idle;
            SceneManager.LoadScene("FacebookConversation", LoadSceneMode.Additive);
        }

        public void ResetInteraction()
        {
            
        }
    }
}