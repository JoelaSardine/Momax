using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rpg
{
    public class ProjectilesManager : MonoBehaviour
    {
        [Header("Prefabs")]
        public GameObject playerProjectilePrefab;

        [Header("Containers")]
        public Transform ProjectilesContainer;
        public List<Projectile> playerProjectiles;
        public List<Projectile> ennemyPojectiles;

        private void Awake()
        {
            
        }

        public void PlayerFire(Vector3 position, Vector2 lookingDirection)
        {
            Projectile projectile = Instantiate(playerProjectilePrefab, position, Quaternion.identity, ProjectilesContainer).GetComponent<Projectile>();
            projectile.Init(lookingDirection);
        }
    }
}