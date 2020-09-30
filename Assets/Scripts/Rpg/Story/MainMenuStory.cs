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
        public bool isEnd = false;

        public GameObject cheatPanel;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            mainmenuController = GetComponent<MenuController>();

            cheatPanel.SetActive(false);
        }

        protected override IEnumerator Start()
        {
            if (RpgManager.Instance != null)
            {
                skipIntro = RpgManager.Instance.gameState != RpgManager.GameState.Undefined;
                isEnd = RpgManager.Instance.gameState == RpgManager.GameState.End;
            }

            yield return StartCoroutine(base.Start());

            MenuButton continueBtn = mainmenuController.menuButtons.Find(x => x.type == MenuButton.MenuButtonType.Continue);
            continueBtn.isEnabled = GameData.CheckFile();

            player.movementEnabled = false;
            player.attackEnabled = false;

            RpgManager.Instance.gameState = RpgManager.GameState.MainMenu;

            if (isEnd)
            {
                animator.SetTrigger(triggerOnBack);
            }
            else if (skipIntro)
            {
                // Player go to menu from game
                audioSource.Play();
                animator.SetTrigger(triggerOnBack);
            }
            else
            {
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

            if (RpgManager.GetKey(SaveKey.defeatedCerberus) == 1)
            {
                cheatPanel.SetActive(true);
            }
        }
    }
}