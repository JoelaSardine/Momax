using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossExplosion : MonoBehaviour
{
    private Animator animator;
    //private AudioSource audioSource;

    
    private void Awake()
    {
        animator = GetComponent<Animator>();

        animator.SetTrigger("Grow");
        //audioSource = GetComponent<AudioSource>();
    }
}
