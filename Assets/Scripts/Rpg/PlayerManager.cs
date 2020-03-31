using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rpg
{
    public class PlayerManager : MonoBehaviour
    {
        private const string INPUT_AXIS_HORIZONTAL = "Horizontal";
        private const string INPUT_AXIS_VERTICAL = "Vertical";
        private const string INPUT_FIRE = "Fire";
        private const string INPUT_INTERACT = "Interact";

        private const string ANIMATOR_VERTICAL = "Horizontal";
        private const string ANIMATOR_HORIZONTAL = "Vertical";
        private const string ANIMATOR_SPEED = "Speed";

        private const string INTERACTION_COLLIDER = "InteractionCollider";
        
        private ProjectilesManager projectilesManager;

        private Animator animator;
        private new Rigidbody2D rigidbody;
        private Collider2D interactionCollider;

        [Header("Movement")]
        [SerializeField] private bool _movementEnabled = true;
        public float speed = 5.0f;
        private Vector2 velocity = Vector2.zero;
        private Vector2 movingDirection = Vector2.zero;
        private Vector2 lookingDirection = Vector2.zero;
        public bool movementEnabled {
            get { return _movementEnabled; }
            set {
                animator.SetBool("Moving", false);
                _movementEnabled = value;
            }
        }

        [Header("Interaction")]
        public float interactionRange = 1.0f;

        [Header("Life & Hit")]
        public int pv = 3;
        public float hitDelay = 1.0f;
        private bool isHitInCooldown = false;

        [Header("Attack")]
        public bool attackEnabled = true;
        public float attackDelay = 0.5f;
        private bool isAttackInCooldown = false;
        
        
        public void Init(ProjectilesManager pm)
        {
            rigidbody = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();

            interactionCollider = transform.Find(INTERACTION_COLLIDER).GetComponent<Collider2D>();
            interactionRange = interactionCollider.transform.localPosition.magnitude;

            projectilesManager = pm;

            RpgManager.HUD.UpdateHearts(pv, 3);
        }

        private void Update()
        {
            if (Input.GetButtonDown(INPUT_FIRE))
            {
                Fire();
            }
        }

        private void FixedUpdate()
        {
            if (movementEnabled)
            {
                Vector2 input = new Vector2(Input.GetAxisRaw(INPUT_AXIS_HORIZONTAL), Input.GetAxisRaw(INPUT_AXIS_VERTICAL));
                input.Normalize();

                if (input == Vector2.zero)
                {
                    animator.SetBool("Moving", false);
                }
                else
                {
                    lookingDirection = input;
                    animator.SetBool("Moving", true);

                    animator.SetFloat("Horizontal", input.x);
                    animator.SetFloat("Vertical", input.y);
                }

                movingDirection = input;

                Move();
            }
        }

        private void Move()
        {
            rigidbody.velocity = movingDirection * speed;
            interactionCollider.transform.localPosition = lookingDirection * interactionRange;
        }

        private void Fire()
        {
            if (isAttackInCooldown)
                return;

            projectilesManager.PlayerFire(transform.position, lookingDirection);

            StartCoroutine(FireWaitCoroutine());
        }

        private IEnumerator FireWaitCoroutine()
        {
            isAttackInCooldown = true;
            yield return new WaitForSeconds(attackDelay);
            isAttackInCooldown = false;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position + new Vector3(0, 0.5f), velocity);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var creature = collision.gameObject.GetComponent<CreatureController>();

            if (creature != null && !isHitInCooldown && !creature.isSpeeping)
            {
                StartCoroutine(GetHit());
            }
        }

        private IEnumerator GetHit()
        {
            pv -= 1;
            animator.SetTrigger("Hit");
            RpgManager.HUD.UpdateHearts(pv, 3);

            isHitInCooldown = true;
            yield return new WaitForSeconds(hitDelay);
            isHitInCooldown = false;
        }
    }
}