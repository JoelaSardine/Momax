using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rpg
{
    public class Ennemy : MonoBehaviour
    {
        public int life = 1;
        int currentLife = 0;

        Collider2D hitbox;
        Collider2D damageBox;

        Animator animator;

        private void Awake()
        {
            currentLife = life;

            animator = GetComponent<Animator>();

            hitbox = GetComponent<Collider2D>();
        }

        private void GetHit(Projectile projectile)
        {
            life -= projectile.damage;

            if (life <= 0)
                Die();

            projectile.Destruct();
        }

        private void Die()
        {

        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Projectile p = collision.gameObject.GetComponent<Projectile>();
            if (p)
                GetHit(p);
        }
    }
}