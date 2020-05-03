using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rpg
{
    public class GameOverStory : GameStory
    {
        private Animator animator;
        private AudioSource audioSource;
        public MenuController menuController;

        public List<AudioClip> firstComment;
        public List<AudioClip> lastComment;
        private List<int> commentAvailableIds;

        public bool skipIntro = false;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();

            menuController.gameObject.SetActive(false);
        }

        protected override IEnumerator Start()
        {
            yield return StartCoroutine(base.Start());
            //RpgManager.Player.gameObject.SetActive(false);

            /*
            if (RpgManager.Instance != null && RpgManager.Instance.gameState != RpgManager.GameState.Undefined)
            {
                skipIntro = true;
            }


            MenuButton continueBtn = mainmenuController.menuButtons.Find(x => x.type == MenuButton.MenuButtonType.Continue);
            continueBtn.isEnabled = GameData.CheckFile();

            player.movementEnabled = false;
            player.attackEnabled = false;

            if (skipIntro)
            {
                // Player go to menu from game
                //animator.SetTrigger(triggerOnBack);
            }
            else
            {
                RpgManager.Instance.gameState = RpgManager.GameState.MainMenu;
                mainmenuController.enabled = false;

                // Game just launched
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                //animator.SetTrigger(triggerOnLaunch);
                audioSource.Play();
            }
            */
        }

        public void OnIntroanimationEnded()
        {
            RpgManager.Player.Revive();

            StartCoroutine(OnIntroAnimEndCoroutine());
        }

        private IEnumerator OnIntroAnimEndCoroutine()
        {
            int index = RpgManager.Instance.GetGameOverCommentId(true, firstComment.Count);
            AudioClip clip = firstComment[index];
            RpgManager.PlaySFX(clip);
            yield return new WaitForSeconds(clip.length);

            menuController.gameObject.SetActive(true);
        }
    }
}