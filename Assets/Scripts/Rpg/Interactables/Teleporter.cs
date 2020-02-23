using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rpg
{
    public class Teleporter : Interactable
    {
        public Transform target;
        public string targetScene;

        protected override void Init()
        {
            base.Init();
        }

        protected override void CollisionEnter(Collision2D collision)
        {
            if (collision.collider.name == "Morgane")
            {
                StartCoroutine(Teleport(collision.gameObject));
            }
        }

        protected override void CollisionExit(Collision2D collision)
        {
        }

        private IEnumerator Teleport(GameObject go)
        {
            PlayerController pc = go.GetComponent<PlayerController>();
            pc.movementEnabled = false;

            yield return StartCoroutine(GameManager.CameraManager.FadeInCoroutine());

            if (target != null)
            {
                go.transform.position = target.position;
            }

            yield return StartCoroutine(GameManager.CameraManager.FadeOutCoroutine());
            pc.movementEnabled = true;
        }
    }
}