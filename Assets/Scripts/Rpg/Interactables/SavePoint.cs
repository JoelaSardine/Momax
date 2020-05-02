using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace rpg
{
    public class SavePoint : Interactable
    {
        PlayerManager player;

        public Text bubbleLabel;
        public Image icon;

        public string placeID = "PLACE_ID";
        public string defaultLabel = "Sauvegarder";
        public string endLabel = "Sauvegarde effectuée !";

        protected override void Interact()
        {
            player = RpgManager.Player;

            //animator.SetTrigger("Exit");
            state = State.Idle;

            RpgManager.Data.scene = SceneManager.GetActiveScene().name;
            RpgManager.Data.place = placeID;
            RpgManager.Instance.dataDebug.SetData(RpgManager.Data);
            GameData.SaveToFile(RpgManager.Data);

            bubbleLabel.text = endLabel;
            icon.gameObject.SetActive(false);
        }

        protected override void TriggerExit(Collider2D collider)
        {
            base.TriggerExit(collider);

            icon.gameObject.SetActive(true);
            bubbleLabel.text = defaultLabel;
        }

    }
}