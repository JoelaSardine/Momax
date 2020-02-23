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

        public float speed = 25.0f;
        public float interactionRange = 1.0f;

        public void Init(ProjectilesManager pm)
        {
            projectilesManager = pm;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position + new Vector3(0, 0.5f), playerController.velocity);
        }
    }
}