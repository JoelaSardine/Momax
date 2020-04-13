using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public Animator CanvasButtonsAnimator;

    public Button buttonPlay;
    public Button buttonHints;
    public Button buttonQuit;

    private int selectedBtn = -1;

    private AudioSource audioSource;

    private IEnumerator Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        audioSource = GetComponent<AudioSource>();

        yield return null;
    }

    public void OnCanvasTitreIntroEnd()
    {
        CanvasButtonsAnimator.SetTrigger("Play");
    }

    public void OnCanvasButtonsIntroEnd()
    {
        selectedBtn = 1;
        buttonHints.Select();
    }

    public void Update()
    {
        if (selectedBtn == 10 && (Input.GetButtonDown("Vertical") || Input.GetButtonDown("Fire")))
        {
            CanvasButtonsAnimator.SetBool("Hints", false);
            selectedBtn = 1;
            buttonHints.Select();
            audioSource.volume = 1;
        }
        else if (selectedBtn >= 0 && Input.GetButtonDown("Vertical"))
        {
            if (Input.GetAxis("Vertical") > 0)
            {
                switch (selectedBtn)
                {
                    case 0:
                        buttonQuit.Select();
                        selectedBtn = 2;
                        break;
                    case 1:
                        buttonPlay.Select();
                        selectedBtn = 0;
                        break;
                    case 2:
                        buttonHints.Select();
                        selectedBtn = 1;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (selectedBtn)
                {
                    case 0:
                        buttonHints.Select();
                        selectedBtn = 1;
                        break;
                    case 1:
                        buttonQuit.Select();
                        selectedBtn = 2;
                        break;
                    case 2:
                        buttonPlay.Select();
                        selectedBtn = 0;
                        break;
                    default:
                        break;
                }
            }
        }
        else if (Input.GetButtonDown("Fire"))
        {
            switch (selectedBtn)
            {
                case 0:
                    audioSource.volume = 0;
                    CanvasButtonsAnimator.SetTrigger("Black");
                    SceneManager.LoadScene("NeuillyPlaisance");
                    break;
                case 1:
                    CanvasButtonsAnimator.SetBool("Hints", true);
                    selectedBtn = 10;
                    audioSource.volume = 0;
                    break;
                case 2:
                    Application.Quit();
                    break;
                default:
                    break;
            }
        }
    }
}
