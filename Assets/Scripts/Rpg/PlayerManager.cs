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

        private const string PLAYER_COLLIDER_INTERACTION = "InteractionCollider";
        private const string PLAYER_COLLIDER_FIRE = "FireCollider";
        private const string FIRE_CONTAINER = "FireContainer";

        private ProjectilesManager projectilesManager;
        public PlayerController playerController;
        private Animator animator;

        public float speed = 25.0f;
        public float interactionRange = 1.0f;

        public int pv = 3;
        public float hitDelay = 1.0f;

        private bool justHit = false;

        public void Init(ProjectilesManager pm)
        {
            animator = GetComponent<Animator>();
            projectilesManager = pm;

            RpgManager.HUD.UpdateHearts(pv, 3);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position + new Vector3(0, 0.5f), playerController.velocity);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var creature = collision.gameObject.GetComponent<CreatureController>();

            if (creature != null && !justHit)
            {
                StartCoroutine(GetHit());
            }
        }

        private IEnumerator GetHit()
        {
            pv -= 1;
            animator.SetTrigger("Hit");
            RpgManager.HUD.UpdateHearts(pv, 3);

            justHit = true;
            yield return new WaitForSeconds(hitDelay);
            justHit = false;
        }
    }
}