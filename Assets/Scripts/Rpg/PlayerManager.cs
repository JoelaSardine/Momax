using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

        private const string INTERACTION_COLLIDER = "InteractionCollider";
        
        private ProjectilesManager projectilesManager;

        private Animator animator;
        private new Rigidbody2D rigidbody;
        private Collider2D interactionCollider;

        private GameObject talkCanvas;
        private pokemonBattle.Textshadow talkText;

        private Action onEndTalk;
        private pokemonBattle.Textshadow dialogText = null;
        private bool isCanvasOpen = false;

        private bool sameframe = false;

        public AudioClip sfx_hit;
        public AudioClip sfx_talk;

        [Header("Movement")]
        [SerializeField] private bool _movementEnabled = true;
        public float speed = 5.0f;
        private Vector2 velocity = Vector2.zero;
        private Vector2 movingDirection = Vector2.zero;
        private Vector2 lookingDirection = Vector2.zero;
        public bool movementEnabled {
            get { return _movementEnabled; }
            set {
                if (animator)
                {
                    animator.SetBool("Moving", false);
                }                
                _movementEnabled = value;
            }
        }
        public bool overrideMovement = false;

        [Header("Interaction")]
        public float interactionRange = 1.0f;

        [Header("Life & Hit")]
        public int pv = 3;
        public float hitDelay = 1.0f;
        private bool isHitInCooldown = false;

        [Header("Attack")]
        public bool attackEnabled = true;
        public float attackDelay = 0.5f;
        private bool isAttackInCooldown = false;


        private void Awake()
        {
            if (RpgManager.Player != null && RpgManager.Player != this)
            {
                Destroy(this.gameObject);
            }
        }

        public void Init(ProjectilesManager pm)
        {
            DontDestroyOnLoad(this.gameObject);

            rigidbody = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            talkCanvas = transform.Find("Canvas").gameObject;
            talkText = talkCanvas.transform.Find("Panel").Find("PlaceholderText").GetComponentInChildren<pokemonBattle.Textshadow>();

            interactionCollider = transform.Find(INTERACTION_COLLIDER).GetComponent<Collider2D>();
            interactionRange = interactionCollider.transform.localPosition.magnitude;

            projectilesManager = pm;

            RpgManager.HUD.UpdateHearts(pv, 3);
        }

        private void Update()
        {
            if (Input.GetButtonDown(INPUT_FIRE))
            {
                if (isCanvasOpen && !sameframe)
                {
                    RpgManager.PlaySFX(sfx_talk);

                    if (dialogText && dialogText.isWriting)
                    {
                        dialogText.EndSetTextCoroutine();
                    }
                    else if (talkText.isWriting)
                    {
                        talkText.EndSetTextCoroutine();
                    }
                    else if (onEndTalk != null)
                    {
                        onEndTalk();
                    }
                }
                else if (attackEnabled && !overrideMovement)
                {
                    Fire();
                }
            }

            sameframe = false;
        }

        private void FixedUpdate()
        {
            if (overrideMovement)
                return ;

            if (movementEnabled)
            {
                Vector2 input = new Vector2(Input.GetAxisRaw(INPUT_AXIS_HORIZONTAL), Input.GetAxisRaw(INPUT_AXIS_VERTICAL));
                input.Normalize();

                if (input == Vector2.zero)
                {
                    animator.SetBool("Moving", false);
                }
                else
                {
                    lookingDirection = input;
                    animator.SetBool("Moving", true);

                    animator.SetFloat("Horizontal", input.x);
                    animator.SetFloat("Vertical", input.y);
                }

                movingDirection = input;

                Move();
            }
            else
            {
                rigidbody.velocity = Vector2.zero;
            }
        }

        public void Stop()
        {
            rigidbody.velocity = Vector2.zero;
        }

        private void Move()
        {
            rigidbody.velocity = movingDirection * speed;
            interactionCollider.transform.localPosition = lookingDirection * interactionRange;
        }

        private void Fire()
        {
            if (isAttackInCooldown)
                return;

            projectilesManager.PlayerFire(transform.position, lookingDirection);

            StartCoroutine(FireWaitCoroutine());
        }

        private IEnumerator FireWaitCoroutine()
        {
            isAttackInCooldown = true;
            yield return new WaitForSeconds(attackDelay);
            isAttackInCooldown = false;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position + new Vector3(0, 0.5f), velocity);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var creature = collision.gameObject.GetComponent<CreatureController>();

            if (creature != null && !isHitInCooldown && !creature.isSpeeping && creature.damageOnAttack > 0)
            {
                StartCoroutine(GetHit());
            }
        }

        public void GetHitFunc()
        {
            StartCoroutine(GetHit());
        }

        private IEnumerator GetHit()
        {
            RpgManager.PlaySFX(sfx_hit);

            pv -= 1;
            RpgManager.HUD.UpdateHearts(pv, 3);
            animator.SetTrigger("Hit");

            isHitInCooldown = true;

            if (pv <= 0)
            {
                GetComponent<Collider2D>().enabled = false;
                animator.SetBool("GameOver", true);
                enabled = false;
                Stop();
                yield return new WaitForSeconds(1.0f);
                RpgManager.GameOver();
                yield break;
            }            

            yield return new WaitForSeconds(hitDelay);
            isHitInCooldown = false;
        }

        public void Revive()
        {
            enabled = true;
            pv = 3;
            RpgManager.HUD.UpdateHearts(pv, 3);
            animator.SetBool("GameOver", false);
            isHitInCooldown = false;
        }

        public void Dialog(bool? left, string message, Action OnEndTalk = null)
        {
            sameframe = true;

            isCanvasOpen = true;
            movementEnabled = false;

            this.onEndTalk = OnEndTalk;

            dialogText = RpgManager.DialogueTalk(left, message);
        }

        public void Talk(string message, Action OnEndTalk = null)
        {
            sameframe = true;

            isCanvasOpen = true;
            movementEnabled = false;

            this.onEndTalk = OnEndTalk;

            talkCanvas.SetActive(true);
            talkText.Display(message, true);
        }

        public void EndTalk()
        {
            dialogText = RpgManager.DialogueTalk(null);  
            talkCanvas.SetActive(false);
            isCanvasOpen = false;
            movementEnabled = true;
        }
    }
}