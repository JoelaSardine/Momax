using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rpg
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Animator))]
    public class PlayerController : MonoBehaviour
    {
        private const string INPUT_AXIS_HORIZONTAL = "Horizontal";
        private const string INPUT_AXIS_VERTICAL = "Vertical";
        private const string INTERACTION_COLLIDER = "InteractionCollider";

        private bool _movementEnabled = true;
        public bool movementEnabled {
            get { return _movementEnabled; }
            set {
                animator.SetBool("Moving", false);
                _movementEnabled = value;
            }
        }

        public float speed = 5.0f;
        public Vector2 velocity = Vector2.zero;

        public Vector2 movingDirection = Vector2.zero;
        public Vector2 lookingDirection = Vector2.zero;
        
        private new Rigidbody2D rigidbody;
        private Animator animator;
        private float interactionRange;
        private Collider2D interactionCollider;

        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();

            interactionCollider = transform.Find(INTERACTION_COLLIDER).GetComponent<Collider2D>();
            interactionRange = interactionCollider.transform.localPosition.magnitude;
        }

        private void Update()
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
    }
}