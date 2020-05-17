using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rpg
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Bull : MonoBehaviour
    {
        private CreatureController creatureController;
        private new Rigidbody2D rigidbody;
        private AudioSource audioSource;

        public AudioClip sfx_seePlayer;

        public float speedRoam = 2.0f;
        public float maxSpeedChase = 5.0f;
        public float speedChaseIncrement = 0.1f;
        public float stoppingDistance = 0.01f;
        public float stoppingDuration = 0.5f;
        
        public Rect roamZone = new Rect(0, 0, 2, 2);
        private Vector3 target;

        private float speed = 1.0f;
        private bool isChasing = false;
        private bool isStopping = false;
        private bool justHitPlayer = false;

        private void Awake()
        {
            creatureController = GetComponent<CreatureController>();
            rigidbody = GetComponent<Rigidbody2D>();
            audioSource = GetComponent<AudioSource>();

            target = transform.position;

            rigidbody.mass = Random.Range(2.0f, 3.0f);
        }

        private void FixedUpdate()
        {
            if (creatureController.isSpeeping || creatureController.isJustHit)
            {
                rigidbody.velocity = Vector2.zero;
                return;
            }

            Vector3 playerPos = RpgManager.Player.transform.position;
            bool wasChasing = isChasing;
            isChasing = isPlayerInRoamZone(playerPos);

            if (isChasing && !justHitPlayer)
            {
                if (!wasChasing)
                {
                    RpgManager.PlaySFX(sfx_seePlayer);
                }

                speed = Mathf.Clamp(speed + speedChaseIncrement, speedRoam, maxSpeedChase);
                target = playerPos;
                isStopping = false;
            } 
            else if (isStopping)
            {
                speed = speedRoam;
                rigidbody.velocity = Vector2.zero;
                return;
            }

            if (Vector3.Distance(transform.position, target) < stoppingDistance)
            {
                rigidbody.velocity = Vector2.zero;
                StartCoroutine(waitCoroutine());
            }
            else
            {
                rigidbody.velocity = (target - transform.position).normalized * speed;
                //Debug.Log(rigidbody.velocity);
            }
        }

        public void PlayerIsHit() {
            justHitPlayer = true;

            speed = speedRoam;
            target = new Vector3(Random.Range(roamZone.xMin, roamZone.xMax), Random.Range(roamZone.yMin, roamZone.yMax));
            isStopping = false;
        }

        private IEnumerator waitCoroutine()
        {
            justHitPlayer = false;
            isStopping = true;
            yield return new WaitForSeconds(Random.Range(0, stoppingDuration));

            target = new Vector3(Random.Range(roamZone.xMin, roamZone.xMax), Random.Range(roamZone.yMin, roamZone.yMax));
            isStopping = false;
        }

        private bool isPlayerInRoamZone(Vector3 playerPos)
        {
            return (playerPos.x > roamZone.xMin && playerPos.x < roamZone.xMax
                && playerPos.y > roamZone.yMin && playerPos.y < roamZone.yMax);
        }

        private void OnDrawGizmos()
        {
            Vector3 bottomLeft = new Vector3(roamZone.xMin, roamZone.yMin);
            Vector3 topLeft = new Vector3(roamZone.xMin, roamZone.yMax);
            Vector3 topRight = new Vector3(roamZone.xMax, roamZone.yMax);
            Vector3 bottomRight = new Vector3(roamZone.xMax, roamZone.yMin);

            Gizmos.DrawSphere(bottomLeft, 0.2f);
            Gizmos.DrawSphere(topLeft, 0.2f);
            Gizmos.DrawSphere(topRight, 0.2f);
            Gizmos.DrawSphere(bottomRight, 0.2f);
            Gizmos.DrawLine(bottomLeft, topLeft);
            Gizmos.DrawLine(topLeft, topRight);
            Gizmos.DrawLine(topRight, bottomRight);
            Gizmos.DrawLine(bottomRight, bottomLeft);

            if (target != Vector3.zero)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, target);
                Gizmos.DrawSphere(target, 0.2f);
            }
        }
    }
}