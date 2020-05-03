using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rpg
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Animator))]
    public class CreatureController : MonoBehaviour
    {
        public delegate void OnHitDelegate(bool isAsleep);
        public OnHitDelegate onHit;

        private const string INPUT_AXIS_HORIZONTAL = "Horizontal";
        private const string INPUT_AXIS_VERTICAL = "Vertical";

        private new Rigidbody2D rigidbody;
        private Animator animator;

        public AudioClip sfx_onHit;

        private bool isMoving = false;
        public Vector2 lookingDirection = Vector2.zero;
        public Vector2 movingDirection = Vector2.zero;

        public int life = 1;
        public int damageOnAttack = 1;

        public float hitDuration = 0.5f;

        public bool isJustHit = false;
        public bool isSpeeping = false;

        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            movingDirection = rigidbody.velocity;
            isMoving = movingDirection.sqrMagnitude > 0.1f;
            
            if (isMoving)

            {
                lookingDirection = movingDirection.normalized;
            }

            animator.SetBool("Moving", isMoving);
            animator.SetFloat("Horizontal", lookingDirection.x);
            animator.SetFloat("Vertical", lookingDirection.y);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Projectile projectile = collision.transform.GetComponent<Projectile>();
            if (projectile && projectile.isPlayerProjectile)
            {
                BeHitByPlayerProjectile(projectile);
            }
        }

        private void BeHitByPlayerProjectile(Projectile projectile)
        {
            projectile.Destruct();
            if (isSpeeping)
            {
                return;
            }

            life--;
            if (sfx_onHit) RpgManager.PlaySFX(sfx_onHit);

            if (life <= 0)
            {
                isSpeeping = true;
                animator.SetBool("Sleeping", true);
            }
            animator.SetTrigger("Hit");

            StartCoroutine(OnHitCoroutine());

            if (onHit != null)
            {
                onHit(isSpeeping);
            }
        }

        private IEnumerator OnHitCoroutine()
        {
            isJustHit = true;
            yield return new WaitForSeconds(hitDuration);
            isJustHit = false;
        }
    }
}