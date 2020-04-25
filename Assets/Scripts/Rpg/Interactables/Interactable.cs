using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rpg
{
    public class Interactable : MonoBehaviour
    {
        public Action onEndInteraction;

        [System.Flags]
        public enum State {
            Idle = 1,
            Triggered = 2,
            Collided = 4,
            Activated = 8
        }

        [SerializeField]
        protected State state = State.Idle;
        protected Animator animator;

        public bool active = true;

        private void Awake()
        {
            animator = GetComponent<Animator>();

            Init();
        }

        private void Update()
        {
            if (active && Input.GetButtonDown("Fire") && state == (state | State.Triggered))
            {
                Interact();
            }

            DoUpdate();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!active) return;

            state = state | State.Collided;
            CollisionEnter(collision);
        }
        private void OnCollisionExit2D(Collision2D collision)
        {
            if (!active) return;

            state = (state | State.Collided) ^ State.Collided;
            CollisionExit(collision);
        }
        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (!active) return;

            state = state | State.Triggered;
            if (animator != null)
            {
                animator.ResetTrigger("Exit");
                animator.SetTrigger("Enter");
            }

            TriggerEnter(collider);
        }
        private void OnTriggerExit2D(Collider2D collider)
        {
            if (!active) return;

            state = (state | State.Triggered) ^ State.Triggered;
            if (animator != null)
            {
                animator.ResetTrigger("Enter");
                animator.SetTrigger("Exit");
            }

            TriggerExit(collider);
        }


        protected virtual void Init() {}
        protected virtual void DoUpdate() {}

        protected virtual void CollisionEnter(Collision2D collision) {}
        protected virtual void CollisionExit(Collision2D collision) {}

        protected virtual void TriggerEnter(Collider2D collider) {}
        protected virtual void TriggerExit(Collider2D collider) {}

        protected virtual void Interact() { }
    }
}
