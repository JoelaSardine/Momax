using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class FighterBattle
{
    public RectTransform avatarPanel;
    public RectTransform statsPanel;

    private Image hpBar;
    private Text hpText;

    private int hp = 100;
    private int Hp {
        get {
            return hp;
        }
        set {
            hp = Mathf.Clamp(value, 0, 100);
            if (hpText) hpText.text = hp + " / " + "100";
            hpBar.fillAmount = hp / 100f;
            hpBar.color =
                hpBar.fillAmount < BattleConsts.I.hpCritTreshold ?
                BattleConsts.I.hpCritColor : hpBar.fillAmount < BattleConsts.I.hpLowTreshold ?
                BattleConsts.I.hpLowColor : BattleConsts.I.hpBaseColor;
        }
    }

    public void Init()
    {
        hpBar = statsPanel.Find("HpBar").Find("Image").GetComponent<Image>();
        Transform textTransform = statsPanel.Find("HPText");
        if (textTransform) hpText = statsPanel.Find("HPText").GetComponent<Text>();
    }

    public IEnumerator SetHpCoroutine(int target, Action callback = null)
    {
        float time = BattleConsts.I.hpModifDuration;
        float timer = 0;
        int baseValue = hp;
        float value = hp;

        while (timer <= time)
        {
            timer += Time.deltaTime;
            value = Mathf.Lerp(baseValue, target, timer / time);
            Hp = Mathf.RoundToInt(value);
            yield return null;
        }

        callback?.Invoke();
    }
}
