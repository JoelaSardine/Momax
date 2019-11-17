using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCollider : MonoBehaviour
{
    public float fireSpeed = 1.0f;

    Vector2 direction = new Vector2();

    public void Fire(Vector2 normalized)
    {
        direction = normalized;
    }

    private void Update()
    {
        transform.Translate(direction * Time.deltaTime * fireSpeed);
    }
}
