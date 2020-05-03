﻿using rpg;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip sfx_Move;
    public AudioClip sfx_Select;

    public int defaultButtonId = 0;
    public List<MenuButton> menuButtons;

    protected int currentButtonId = 0;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        currentButtonId = defaultButtonId;
    }

    private void OnEnable()
    {
        for (int i = 0; i < menuButtons.Count; i++)
        {
            if (menuButtons[i].isEnabled)
            {
                menuButtons[i].SetSelected(i == currentButtonId);
            }
            else
            {
                menuButtons[i].SetEnabled(false);
            }
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Vertical"))
        {
            RpgManager.PlaySFX(sfx_Move);
            menuButtons[currentButtonId].SetSelected(false);

            int increment = Input.GetAxis("Vertical") > 0 ? -1 : 1;

            for (int i = 0; i < menuButtons.Count; i++)
            {
                currentButtonId = Mathf.RoundToInt(Mathf.Repeat(currentButtonId + increment, menuButtons.Count));
                if (menuButtons[currentButtonId].isEnabled)
                    break;
            }
            menuButtons[currentButtonId].SetSelected(true);
        }
        else if (Input.GetButtonDown("Fire"))
        {
            SelectButton();
        }    
    }

    protected virtual void SelectButton()
    {
        RpgManager.PlaySFX(sfx_Select);
        menuButtons[currentButtonId].Validate();
    }
}
