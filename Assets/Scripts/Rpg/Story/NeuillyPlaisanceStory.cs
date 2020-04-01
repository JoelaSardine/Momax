using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rpg
{
    public class NeuillyPlaisanceStory : MonoBehaviour
    {
        public Interactable entrance;

        private void Start()
        {
            //RpgManager.Player.movementEnabled = false;
            entrance.active = false;
        }
    }
}