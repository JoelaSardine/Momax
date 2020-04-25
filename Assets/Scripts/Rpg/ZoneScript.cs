using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rpg
{
    public class ZoneScript : MonoBehaviour
    {
        public string zoneName = "Zone";

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (other.name == "Morgane")
            {
                RpgManager.ZoneDisplayName(zoneName);
            }
        }

        /*public void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.name == "Morgane")
            {
                Debug.Log(name);
            }
        }*/
    }
}