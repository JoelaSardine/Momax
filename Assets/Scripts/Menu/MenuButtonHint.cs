using rpg;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButtonHint : MenuButton
{
    public Animator mainMenuStoryAnimator;
    public MenuController menuController;
    public bool isShowingHints = false;

    /*private void Awake()
    {
        animator = GetComponent<Animator>();   
    }*/
    

    public override void Validate()
    {
        RpgManager.StopEndMusic();

        isShowingHints = !isShowingHints;
        mainMenuStoryAnimator.SetBool("Hints", isShowingHints);

        menuController.enabled = !isShowingHints;
        mainMenuStoryAnimator.GetComponent<AudioSource>().volume = isShowingHints ? 0 : 1;
    }

    private void Update()
    {
        if (isShowingHints && Input.anyKeyDown)
        {
            Validate();
        }
    }
}
