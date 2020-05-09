using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rpg
{
    public class GameOverStory : GameStory
    {
        private Animator animator;
        public MenuController menuController;

        public List<AudioClip> firstComment;
        public List<AudioClip> lastComment;
        private List<int> commentAvailableIds;

        public bool skipIntro = false;

        private void Awake()
        {
            animator = GetComponent<Animator>();

            menuController.gameObject.SetActive(false);
        }

        protected override IEnumerator Start()
        {
            yield return StartCoroutine(base.Start());
        }

        public void OnIntroanimationEnded()
        {
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