using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private const string INPUT_AXIS_HORIZONTAL = "Horizontal";
    private const string INPUT_AXIS_VERTICAL = "Vertical";

    private new Rigidbody2D rigidbody;
    private Animator animator;

    public float speed = 1.0f;

    private Vector2 animatorParams = new Vector2();

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
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
    }
}
