using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCanvas : MonoBehaviour
{
    public enum Canvastype { titre, buttons }

    public Canvastype canvastype;

    public MenuManager menumanager;

    public void OnAnimationEnd()
    {
        if (canvastype == Canvastype.titre)
        {
            menumanager.OnCanvasTitreIntroEnd();
        }
        else
        {
            menumanager.OnCanvasButtonsIntroEnd();
        }
    }
}
