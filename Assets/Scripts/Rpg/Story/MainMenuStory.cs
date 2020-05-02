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
            if (RpgManager.Instance != null && RpgManager.Instance.gameState != RpgManager.GameState.Undefined)
            {
                skipIntro = true;
            }

            yield return StartCoroutine(base.Start());

            MenuButton continueBtn = mainmenuController.menuButtons.Find(x => x.type == MenuButton.MenuButtonType.Continue);
            continueBtn.isEnabled = GameData.CheckFile();

            player.movementEnabled = false;
            player.attackEnabled = false;

            if (skipIntro)
            {
                // Player go to menu from game
                animator.SetTrigger(triggerOnBack);
                audioSource.Play();
            }
            else
            {
                RpgManager.Instance.gameState = RpgManager.GameState.MainMenu;
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