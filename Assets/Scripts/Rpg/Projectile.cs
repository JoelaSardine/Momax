using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rpg
{
    public class Projectile : MonoBehaviour
    {
        public int damage = 1;

        public void Destruct()
        {
            GameObject.Destroy(gameObject);
        }
    }
}