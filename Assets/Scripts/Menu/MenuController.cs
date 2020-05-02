using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public int defaultButtonId = 0;
    public List<MenuButton> menuButtons;

    private int currentButtonId = 0;

    private void Awake()
    {
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
        if (Input.GetButtonDown("Fire"))
        {
            menuButtons[currentButtonId].Validate();
        }    
    }
}
