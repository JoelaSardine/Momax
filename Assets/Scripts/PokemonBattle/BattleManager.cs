using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public enum BATTLESTATE {
        DEFAULT = 0
    }

    private bool busy = false;

    public FighterBattle player;
    public FighterBattle ennemy;
    public Transform actionsPanel;

    public BATTLESTATE battleState;

    private Textshadow bigTxt;
    private Textshadow smallTxt;
   

    private void Awake()
    {
        player.Init();
        ennemy.Init();

        Transform dialPanel = actionsPanel.Find("DialogPanel");
        bigTxt = dialPanel.Find("DialogBig").GetComponentInChildren<Textshadow>();
        smallTxt = dialPanel.Find("DialogSmall").GetComponentInChildren<Textshadow>();
    }

    private void Start()
    {
        GameObject trGo = GameObject.FindGameObjectWithTag("Transition");
        if (trGo)
        {
            InitTransition();
            TransitionBattle trbattle = trGo.GetComponent<TransitionBattle>();
            trbattle.onOpeningFinished = delegate () { StartCoroutine(IntroTransitionCoroutine()); };
            trbattle.StartShutterCoroutine();
        }
        else {
            // No transition
            //InitTransition();
            //StartCoroutine(IntroTransitionCoroutine());
        }

        // Temp
        bigTxt.SetText("MORGANE est attaquée par ENSIL-ENSCI !", DefaultCallback);
        smallTxt.SetText("");
    }

    private void DefaultCallback()
    {
        busy = false;
    }

    private void Update()
    {
        if (!busy)
        {
            // Can do something
        }
    }

    private void InitTransition()
    {
        ennemy.avatarPanel.anchorMin += Vector2.left;
        ennemy.avatarPanel.anchorMax += Vector2.left;
        ennemy.statsPanel.anchorMin += Vector2.left;
        ennemy.statsPanel.anchorMax += Vector2.left;
        player.avatarPanel.anchorMin += Vector2.right;
        player.avatarPanel.anchorMax += Vector2.right;
        player.statsPanel.anchorMin += Vector2.right;
        player.statsPanel.anchorMax += Vector2.right;
    }

    private IEnumerator IntroTransitionCoroutine()
    {
        float time = BattleConsts.I.introAvatarDuration;
        float timer = 0;
        float value = 1; // from 1 to 0

        Vector2 ennemyMin = ennemy.avatarPanel.anchorMin;
        Vector2 ennemyMax = ennemy.avatarPanel.anchorMax;
        Vector2 playerMin = player.avatarPanel.anchorMin;
        Vector2 playerMax = player.avatarPanel.anchorMax;

        while (timer <= time)
        {
            timer += Time.deltaTime;
            value = Mathf.Clamp01((time - timer) / time);

            ennemyMin.x = 0.5f - value;
            ennemyMax.x = 1.0f - value;
            playerMin.x = 0.0f + value;
            playerMax.x = 0.5f + value;

            ennemy.avatarPanel.anchorMin = ennemyMin;
            ennemy.avatarPanel.anchorMax = ennemyMax;
            player.avatarPanel.anchorMin = playerMin;
            player.avatarPanel.anchorMax = playerMax;

            yield return null;
        }

        time = BattleConsts.I.introStatsDuration;
        timer = 0;
        value = 1;

        ennemyMin = ennemy.statsPanel.anchorMin;
        ennemyMax = ennemy.statsPanel.anchorMax;
        playerMin = player.statsPanel.anchorMin;
        playerMax = player.statsPanel.anchorMax;

        while (timer < time)
        {
            timer += Time.deltaTime;
            value = Mathf.Clamp01((time - timer) / time);

            ennemyMin.x = 0.3f - value;
            ennemyMax.x = 0.3f - value;
            playerMin.x = 0.7f + value;
            playerMax.x = 0.7f + value;

            ennemy.statsPanel.anchorMin = ennemyMin;
            ennemy.statsPanel.anchorMax = ennemyMax;
            player.statsPanel.anchorMin = playerMin;
            player.statsPanel.anchorMax = playerMax;

            yield return null;
        }
    }
}
