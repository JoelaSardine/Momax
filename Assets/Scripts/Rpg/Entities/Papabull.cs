using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rpg
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Papabull : MonoBehaviour
    {
        private CreatureController creatureController;
        private new Rigidbody2D rigidbody;

        public AudioClip sfx_chase;

        public float startSpeed = 1f;
        public float speedIncrement = 0.01f;
        private float normalSpeed;

        public float speed = 5.0f;
        public float stoppingDistance = 0.01f;
        public float stoppingDuration = 0.5f;

        public PapaBullInteractable guardZone;
        public Vector3 homeTarget;
        public Vector3 firstTarget;
        public Vector3 currentTarget;
        private bool isChasingPlayer = false;
        private bool isReturning = false;

        public List<Vector3> waypoints = new List<Vector3>();
        public int currentWP = 0;

        private bool isStopping = false;

        public Collider2D playerBlockerCollider;

        private void Awake()
        {
            normalSpeed = speed;

            isStopping = true;
            homeTarget = transform.position;

            creatureController = GetComponent<CreatureController>();
            rigidbody = GetComponent<Rigidbody2D>();

            rigidbody.mass = 100;
            creatureController.onHit = OnHit;

            guardZone.onEndInteraction += onPlayerSightViewed;

            waypoints.Insert(0, transform.position);
        }

        private void onPlayerSightViewed()
        {
            if (creatureController.isSpeeping)
            {
                return;
            }

            creatureController.PlaySFX(sfx_chase);

            isStopping = false;
            isChasingPlayer = true;
            speed = startSpeed;
        }

        private void OnHit(bool isSleeping)
        {
            if (isSleeping)
            {
                rigidbody.mass = 5;
            }
        }

        private void Update()
        {
            if (isChasingPlayer)
            {
                if (guardZone.isPlayerIn == false)
                {
                    isChasingPlayer = false;
                    isReturning = true;
                    StartCoroutine(waitCoroutine());
                    return;
                }

                currentTarget = RpgManager.Player.transform.position;
            }
        }

        private void FixedUpdate()
        {
            if (creatureController.isSpeeping || creatureController.isJustHit)
            {
                rigidbody.velocity = Vector2.zero;
                return;
            }

            if (isStopping)
            {
                rigidbody.velocity = Vector2.zero;
                return;
            }

            if (isChasingPlayer)
            {
                if (speed < 7)
                {
                    speed += speed * speedIncrement;
                }

                rigidbody.velocity = (currentTarget - transform.position).normalized * speed;
            }
            else if (isReturning)
            {
                if (Vector3.Distance(transform.position, homeTarget) <= stoppingDistance)
                {
                    isReturning = false;
                    isStopping = true;
                    rigidbody.velocity = Vector3.down;
                }
                else
                {
                    rigidbody.velocity = (homeTarget - transform.position).normalized * speed;
                }
            }

            /*if (Vector3.Distance(transform.position, waypoints[currentWP]) < stoppingDistance)
            {
                rigidbody.velocity = Vector2.zero;
                StartCoroutine(waitCoroutine());
            }
            else
            {
                rigidbody.velocity = (waypoints[currentWP] - transform.position).normalized * speed;
                //Debug.Log(rigidbody.velocity);
            }*/
        }

        private IEnumerator waitCoroutine()
        {
            isStopping = true;
            yield return new WaitForSeconds(stoppingDuration);
            currentWP++;
            if (currentWP >= waypoints.Count)
            {
                currentWP = 0;
            }
            isStopping = false;
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawLine(transform.position, firstTarget);
            Gizmos.DrawSphere(firstTarget, 0.2f);

            /*for (int i = 0; i < waypoints.Count; i++)
            {
                Gizmos.DrawSphere(waypoints[i], 0.2f);

                if (i == 0)
                    Gizmos.DrawLine(transform.position, waypoints[i]);
                else
                    Gizmos.DrawLine(waypoints[i - 1], waypoints[i]);
            }

            if (waypoints.Count > 0)
            {
                Gizmos.DrawLine(waypoints[waypoints.Count - 1], transform.position);
            }*/
        }

    }
}