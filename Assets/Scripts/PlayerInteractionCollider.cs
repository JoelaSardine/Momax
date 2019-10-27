using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionCollider : MonoBehaviour
{
    public delegate void OnTriggerEvent(Collider2D collision);
    private OnTriggerEvent onTriggerEnter;
    private OnTriggerEvent onTriggerExit;
    private OnTriggerEvent onTriggerStay;

    private new Collider2D collider;

    public void Init(OnTriggerEvent onTriggerEnter, OnTriggerEvent onTriggerExit, OnTriggerEvent onTriggerStay)
    {
        collider = GetComponent<Collider2D>();

        this.onTriggerEnter = onTriggerEnter;
        this.onTriggerExit = onTriggerExit;
        this.onTriggerStay = onTriggerStay;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        onTriggerEnter(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        onTriggerExit(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        onTriggerStay(collision);
    }
}
