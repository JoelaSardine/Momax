using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rpg
{
    public class MainMenuStory : GameStory
    {
        private Animator animator;
        private AudioSource audioSource;
        [HideInInspector]
        public MenuController mainmenuController;

        public string triggerOnLaunch = "Launch";
        public string triggerOnBack = "BackFromGame";

        public bool skipIntro = false;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
            mainmenuController = GetComponent<MenuController>();
        }

        protected override IEnumerator Start()
        {
            yield return StartCoroutine(base.Start());

            player.movementEnabled = false;
            player.attackEnabled = false;

            if (skipIntro)
            {
                // Player go to menu from game
                animator.SetTrigger(triggerOnBack);
            }
            else
            {
                mainmenuController.enabled = false;

                // Game just launched
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                animator.SetTrigger(triggerOnLaunch);
                audioSource.Play();
            }
        }

        public void OnIntroanimationEnded()
        {
            mainmenuController.enabled = true;
        }
    }
}