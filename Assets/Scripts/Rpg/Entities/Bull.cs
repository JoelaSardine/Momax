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
        
        public float speed = 5.0f;
        public float stoppingDistance = 0.01f;
        public float stoppingDuration = 0.5f;

        public List<Vector3> waypoints = new List<Vector3>();
        public int currentWP = 0;

        private bool isStopping = false;

        private void Awake()
        {
            creatureController = GetComponent<CreatureController>();
            rigidbody = GetComponent<Rigidbody2D>();

            waypoints.Insert(0, transform.position);
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

            if (Vector3.Distance(transform.position, waypoints[currentWP]) < stoppingDistance)
            {
                rigidbody.velocity = Vector2.zero;
                StartCoroutine(waitCoroutine());
            }
            else
            {
                rigidbody.velocity = (waypoints[currentWP] - transform.position).normalized * speed;
                //Debug.Log(rigidbody.velocity);
            }
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
            for (int i = 0; i < waypoints.Count; i++)
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
            }
        }
    }
}