using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class Textshadow : MonoBehaviour
{
    public string text = "text";
    public int size = 20;

    public Text realText;
    private Text myText;

    private void Awake()
    {
        myText = GetComponent<Text>();
    }

    private void Set(string newText)
    {
        text = newText;
        myText.text = text;
        if (realText)
        {
            realText.text = text;
        }
    }
    
    private void OnValidate()
    {
        myText = GetComponent<Text>();
        myText.text = text;
        myText.fontSize = size;
        if (realText)
        {
            realText.text = text;
            realText.fontSize = size;
        }
    }

    public void SetText(string newText, Action callback = null)
    {
        StartCoroutine(SetTextCoroutine(newText, callback));
    }

    private IEnumerator SetTextCoroutine(string newText, Action callback)
    {
        Set("");
        for (int i = 0; i < newText.Length; i++)
        {
            Set(text + newText[i]);
            yield return new WaitForSeconds(BattleConsts.I.dialTextDelay);
        }

        callback?.Invoke();
    }
}
