using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rpg
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Animator))]
    public class CreatureController : MonoBehaviour
    {
        private const string INPUT_AXIS_HORIZONTAL = "Horizontal";
        private const string INPUT_AXIS_VERTICAL = "Vertical";

        private new Rigidbody2D rigidbody;
        private Animator animator;

        private bool moving = false;
        public Vector2 lookingDirection = Vector2.zero;
        public Vector2 movingDirection = Vector2.zero;

        public int damageOnHit = 1;

        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            movingDirection = rigidbody.velocity;
            moving = movingDirection.sqrMagnitude > 0.1f;
            
            if (moving)
            {
                lookingDirection = movingDirection.normalized;
            }

            animator.SetBool("Moving", moving);
            animator.SetFloat("Horizontal", lookingDirection.x);
            animator.SetFloat("Vertical", lookingDirection.y);
        }
    }
}