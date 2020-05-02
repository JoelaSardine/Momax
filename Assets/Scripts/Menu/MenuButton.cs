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

    public virtual void Validate()
    {
        switch (type)
        {
            case MenuButtonType.NewGame:
                RpgManager.LoadScene("NeuillyPlaisance", "Entrance");
                RpgManager.Instance.gameState = RpgManager.GameState.Rpg;
                RpgManager.Data = new GameData();
                RpgManager.Instance.dataDebug.SetData(RpgManager.Data);
                break;
            case MenuButtonType.Continue:
                RpgManager.Data = GameData.LoadFromFile();
                RpgManager.Instance.dataDebug.SetData(RpgManager.Data);
                RpgManager.LoadScene(RpgManager.Data.scene, RpgManager.Data.place);
                break;
            case MenuButtonType.Hints:
                //FindObjectOfType<MainMenuStory>().GetComponent<Animator>().SetBool("Hints", true);
                break;
            case MenuButtonType.Quit:
                Application.Quit();
                break;
            case MenuButtonType.BackToMenu:
                RpgManager.Player.EndTalk();
                RpgManager.Instance.menu.gameObject.SetActive(false);
                RpgManager.Player.enabled = true;
                RpgManager.Instance.gameState = RpgManager.GameState.MainMenu;
                RpgManager.LoadScene("MainMenu", null);
                break;
            case MenuButtonType.BackToGame:
                RpgManager.Instance.ToggleMenu();
                break;
            default:
                break;
        }
    }
}
