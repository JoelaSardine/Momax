using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rpg
{
    public class Autointeractable : MonoBehaviour
    {
        public Action onEndInteraction;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.name == "Morgane")
            {
                TriggerEnter(collision);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.name == "Morgane")
            {
                TriggerExit(collision);
            }
        }

        protected virtual void TriggerEnter(Collider2D collider) { }
        protected virtual void TriggerExit(Collider2D collider) { }
    }
}