using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCollider : MonoBehaviour
{
    public float fireSpeed = 1.0f;
    public float oscillationSpeed = 1.0f;
    public float oscillationAmplitude = 1.0f;

    Vector2 direction = new Vector2();
    Vector2 normal = new Vector2();

    public void Fire(Vector2 normalized)
    {
        direction = normalized;
        normal = new Vector2(direction.y, direction.x);
    }

    private void Update()
    {
        float osci = oscillationAmplitude * Mathf.Cos(Time.time * oscillationSpeed);
        Vector3 newDirection = direction + osci * normal;

        transform.Translate(newDirection * Time.deltaTime * fireSpeed);
    }
}
