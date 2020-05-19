using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace rpg
{
    public class Teleporter : Interactable
    {
        public Transform target;
        public string targetScene;
        public string targetSpawnPoint;
        public AudioClip sfx;
            
        protected override void Init()
        {
            base.Init();
        }

        protected override void CollisionEnter(Collision2D collision)
        {
            if (collision.collider.name == "Morgane")
            {
                if (target != null)
                {
                    StartCoroutine(Teleport(collision.gameObject));
                }
                else
                {
                    RpgManager.LoadScene(targetScene, targetSpawnPoint);
                }
            }
        }

        protected override void CollisionExit(Collision2D collision)
        {
        }

        private IEnumerator Teleport(GameObject go)
        {
            if (sfx)
            {
                RpgManager.PlaySFX(sfx);
            }

            RpgManager.Player.movementEnabled = false;
            yield return StartCoroutine(RpgManager.CameraManager.FadeInCoroutine());
            go.transform.position = target.position;
            yield return StartCoroutine(RpgManager.CameraManager.FadeOutCoroutine());
            RpgManager.Player.movementEnabled = true;
        }
    }
}