using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace rpg
{
    public class Projectile : MonoBehaviour
    {
        public List<Sprite> sprites;
        public List<AudioClip> clips;

        private new SpriteRenderer renderer;

        public bool isPlayerProjectile = false;

        public int damage = 1;
        public float lifeTime = 2.0f;
        public float speed = 0.2f;

        public float oscillationAmplitude = 0.2f;
        public float oscillationSpeed = 1.0f;
        private float timer = 0.0f;

        private Vector3 position;
        public Vector3 direction;
        public Vector3 normal;

        private float baseScale;

        private void Awake()
        {
            renderer = GetComponent<SpriteRenderer>();

            int rdm = Random.Range(0, sprites.Count);
            renderer.sprite = sprites[rdm];

            baseScale = transform.localScale.x;
        }

        public void Init(Vector3 direction)
        {
            this.direction = direction.normalized;
            normal = new Vector3(-direction.y, direction.x);
            position = transform.position;
        }

        private void Update()
        {
            if (lifeTime <= 0)
            {
                Destruct();
                return;
            }

            lifeTime -= Time.deltaTime;
            timer += Time.deltaTime * oscillationSpeed;

            transform.localScale = Vector3.one * Mathf.Min(baseScale, lifeTime / 2);

            Move();
        }

        private void Move()
        {
            position += speed * direction;
            Vector3 lateral = normal * oscillationAmplitude * Mathf.Cos(timer);

            transform.position = position + lateral;
        }

        public void Destruct()
        {
            GameObject.Destroy(gameObject);
        }
    }
}