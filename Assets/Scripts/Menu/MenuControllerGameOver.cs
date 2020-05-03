using rpg;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuControllerGameOver : MenuController
{
    public GameOverStory story;
    public CanvasGroup canvasGroup;

    protected override void OnEnable()
    {
        base.OnEnable();

        StartCoroutine(fadeIn())
;    }

    private IEnumerator fadeIn()
    {
        canvasGroup.alpha = 0;
        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += Time.deltaTime;
            yield return null;
        }
    }

    protected override void SelectButton()
    {
        StartCoroutine(SelectButtonCoroutine());
        enabled = false;
    }

    private IEnumerator SelectButtonCoroutine()
    {
        story.StopMusic();

        int index = RpgManager.Instance.GetGameOverCommentId(false, story.lastComment.Count);
        AudioClip clip = story.lastComment[index];
        RpgManager.PlaySFX(clip);
        yield return new WaitForSeconds(clip.length);

        RpgManager.Player.Revive();

        menuButtons[currentButtonId].Validate();
    }
}
