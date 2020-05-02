using rpg;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour
{
    public enum MenuButtonType
    {
        None,
        NewGame,
        Continue,
        Hints,
        Quit,
        BackToMenu,
        BackToGame
    }

    public Animator animator;

    public MenuButtonType type = MenuButtonType.None;

    public bool isSelected = false;
    public bool isEnabled = true;

    /*private void Awake()
    {
        animator = GetComponent<Animator>();   
    }*/

    public void SetSelected(bool value)
    {
        isSelected = value;
        animator.SetTrigger(value ? "Highlighted" : "Normal");
    }

    public void SetEnabled(bool value)
    {
        isEnabled = value;
        if (value)
        {
            SetSelected(isSelected);
        }
        else
        {
            animator.SetTrigger("Disabled");
        }
    }

    public void Validate()
    {
        switch (type)
        {
            case MenuButtonType.NewGame:
                RpgManager.LoadScene("NeuillyPlaisance", "Entrance");
                break;
            case MenuButtonType.Continue:
                break;
            case MenuButtonType.Hints:
                break;
            case MenuButtonType.Quit:
                Application.Quit();
                break;
            case MenuButtonType.BackToMenu:
                break;
            case MenuButtonType.BackToGame:
                break;
            default:
                break;
        }
    }
}
