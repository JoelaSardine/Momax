using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using rpg;

public class EndPhotoReturnToMenu : MonoBehaviour
{
    void Update()
    {
        if (Input.anyKeyDown)
        {
            RpgManager.Player.enabled = true;
            RpgManager.LoadScene("MainMenu", null);
        }
    }
}
