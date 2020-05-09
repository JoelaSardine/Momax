using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossExplosion : MonoBehaviour
{
    private Animator animator;

    public Action OnWhiteScreenEvent; 
    
    private void Awake()
    {
        animator = GetComponent<Animator>();

        animator.SetTrigger("Grow");
    }

    public void OnWhiteScreen()
    {
        if (OnWhiteScreenEvent != null)
        {
            OnWhiteScreenEvent();
        }
    }
}
