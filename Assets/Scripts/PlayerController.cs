using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private const string INPUT_AXIS_HORIZONTAL = "Horizontal";
    private const string INPUT_AXIS_VERTICAL = "Vertical";
    private const string PLAYER_COLLIDER_INTERACTION = "InteractionCollider";
    private const string PLAYER_COLLIDER_FIRE = "FireCollider";
    private const string FIRE_CONTAINER = "FireContainer";

    private new Rigidbody2D rigidbody;
    private Animator animator;
    private Vector2 animatorParams = new Vector2();

    private PlayerInteractionCollider interactionCollider;
    private Transform fireContainer;
    private FireCollider fireCollider;
    private List<FireCollider> fires = new List<FireCollider>();

    public float speed = 25.0f;
    public float interactionRange = 1.0f;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();

        interactionCollider = transform.Find(PLAYER_COLLIDER_INTERACTION).GetComponent<PlayerInteractionCollider>();
        interactionCollider.Init(OnInteractionEnter, OnInteractionExit, OnInteractionStay);

        fireContainer = GameObject.Find(FIRE_CONTAINER).transform;
        fireCollider = transform.Find(PLAYER_COLLIDER_FIRE).GetComponent<FireCollider>();
    }

    void Update()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw(INPUT_AXIS_HORIZONTAL), Input.GetAxisRaw(INPUT_AXIS_VERTICAL));
        rigidbody.AddForce(Vector2.ClampMagnitude(input, 1.0f) * speed);

        if (rigidbody.velocity == Vector2.zero)
        {
            //animatorParams = animatorParams / 10.0f;
        }
        else
        {
            animatorParams = rigidbody.velocity;
        }
        animator.SetFloat(INPUT_AXIS_HORIZONTAL, animatorParams.x);
        animator.SetFloat(INPUT_AXIS_VERTICAL, animatorParams.y);
        animator.SetFloat("Speed", rigidbody.velocity.magnitude);

        interactionCollider.transform.localPosition = new Vector3(0, 0.5f, 0) + (Vector3)animatorParams.normalized * interactionRange;

        if (Input.GetButtonDown("Fire"))
        {
            Fire(); 
        }
    }

    private void Fire()
    {
        var newFire = Instantiate(fireCollider.gameObject, fireContainer, true).GetComponent<FireCollider>();
        fires.Add(newFire);
        newFire.Fire(animatorParams.normalized);
    }

    private void OnInteractionEnter(Collider2D collision)
    {
        Interactable interactable = collision.GetComponent<Interactable>();
        if (interactable)
        {
            interactable.EnterInteraction();
        }
    }

    private void OnInteractionExit(Collider2D collision)
    {
        Interactable interactable = collision.GetComponent<Interactable>();
        if (interactable)
        {
            interactable.ExitInteraction();
        }
    }

    private void OnInteractionStay(Collider2D collision)
    {
        //Debug.Log("Interaction stay :    " + collision.name);
    }
}
