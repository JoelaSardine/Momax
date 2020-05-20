using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rpg
{
    public class MovableEntity : MonoBehaviour
    {
        private new Rigidbody2D rigidbody;
        private Animator animator;

        public float speed = 5.0f;
        public float stoppingDistance = 0.1f;
        public Vector3 target;
        public Vector3 lookTarget;

        public bool isMoving;

        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
        }

        private void FixedUpdate()
        {
            if (isMoving)
            {
                if (Vector3.Distance(transform.position, target) <= stoppingDistance)
                {
                    isMoving = false;
                    rigidbody.velocity = Vector2.zero;
                }
                else
                {
                    rigidbody.velocity = (target - transform.position).normalized * speed;
                    animator.SetFloat("Horizontal", rigidbody.velocity.x);
                    animator.SetFloat("Vertical", rigidbody.velocity.y);
                }
            }

            animator.SetBool("Moving", isMoving);
        }

        public void LookTowards(Vector3 dir)
        {
            animator.SetFloat("Horizontal", dir.x);
            animator.SetFloat("Vertical", dir.y);
        }
        public void LookAt(Vector3 targ)
        {
            Vector3 dir = targ - transform.position;
            LookTowards(dir);
        }

        public void MoveTo(Vector3 targ)
        {
            target = targ;
            isMoving = true;
        }

        public void MoveTo(Vector3 targ, float newSpeed)
        {
            speed = newSpeed;
            MoveTo(targ);
        }

        public void Hit()
        {
            animator.SetTrigger("Hit");
        }

        public void Sleep(bool state = true)
        {
            animator.SetBool("Sleeping", state);
        }
    }
}