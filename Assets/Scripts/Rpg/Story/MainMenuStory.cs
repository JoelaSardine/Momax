using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rpg
{
    public class MainMenuStory : GameStory
    {
        private Animator animator;
        [HideInInspector]
        public MenuController mainmenuController;

        public string triggerOnLaunch = "Launch";
        public string triggerOnBack = "BackFromGame";

        public bool skipIntro = false;

        private void Awake()
        {
            animator = GetComponent<Animator>();
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
                audioSource.Play();
                animator.SetTrigger(triggerOnBack);
            }
            else
            {
                RpgManager.Instance.gameState = RpgManager.GameState.MainMenu;
                mainmenuController.enabled = false;

                // Game just launched
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                audioSource.Play();
                animator.SetTrigger(triggerOnLaunch);
            }
        }

        public void OnIntroanimationEnded()
        {
            mainmenuController.enabled = true;
        }
    }
}