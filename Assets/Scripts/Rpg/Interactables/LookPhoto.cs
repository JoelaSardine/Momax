using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace rpg
{
    public class LookPhoto : Interactable
    {
        PlayerManager player;

        public GameObject photoCanvas;

        public string speech1 = "Ainsi donc voici ma récompense pour avoir terminé ce jeu !";
        public string speech2 = "Alors, voyons voir. Tiens, une photo.";

        protected override void Interact()
        {
            player = RpgManager.Player;

            animator.SetTrigger("Exit");
            state = State.Idle;

            Interaction_1();
        }

        private void Interaction_1()
        {
            RpgManager.CurrentStory.StopMusic();
            RpgManager.PlayEndMusic();
            RpgManager.Instance.gameState = RpgManager.GameState.End;

            player.Talk(speech1, Interaction_2);
        }
        private void Interaction_2()
        {
            player.Talk(speech2, ShowPhoto);
        }

        private void ShowPhoto()
        {
            player.EndTalk();

            player.movementEnabled = false;

            photoCanvas.gameObject.SetActive(true);
        }

        public void ResetInteraction()
        {
            
        }
    }
}