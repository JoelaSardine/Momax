using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rpg
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Horse : MonoBehaviour
    {
        private CreatureController creatureController;
        private new Rigidbody2D rigidbody;
        
        public float speed = 5.0f;
        public float stoppingDistance = 0.01f;
        public float stoppingDuration = 0.5f;

        public Rect roamZone = new Rect(0, 0, 2, 2);
        private Vector3 target;


        private bool isStopping = false;

        private void Awake()
        {
            creatureController = GetComponent<CreatureController>();
            rigidbody = GetComponent<Rigidbody2D>();

            target = transform.position;
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

        private IEnumerator waitCoroutine()
        {
            isStopping = true;
            yield return new WaitForSeconds(Random.Range(0, stoppingDuration));

            target = new Vector3(Random.Range(roamZone.xMin, roamZone.xMax), Random.Range(roamZone.yMin, roamZone.yMax));
            isStopping = false;
        }

        private void OnDrawGizmos()
        {
            Vector3 bottomLeft = new Vector3(roamZone.xMin, roamZone.yMin);
            Vector3 topLeft = new Vector3(roamZone.xMin, roamZone.yMax);
            Vector3 topRight = new Vector3(roamZone.xMax, roamZone.yMax);
            Vector3 bottomRight = new Vector3(roamZone.xMax, roamZone.yMin);

            Gizmos.DrawSphere(bottomLeft,   0.2f);
            Gizmos.DrawSphere(topLeft,      0.2f);
            Gizmos.DrawSphere(topRight,     0.2f);
            Gizmos.DrawSphere(bottomRight,  0.2f);
            Gizmos.DrawLine(bottomLeft, topLeft);
            Gizmos.DrawLine(topLeft, topRight);
            Gizmos.DrawLine(topRight, bottomRight);
            Gizmos.DrawLine(bottomRight, bottomLeft);
        }
    }
}