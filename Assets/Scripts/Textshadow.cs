using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace pokemonBattle
{
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

        public void SetTxt(string newText)
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

        public void Shutup()
        {
            SetTxt("");
        }

        public void Display(string newText, Action callback = null)
        {
            StartCoroutine(SetTextCoroutine(newText, callback));
        }

        private IEnumerator SetTextCoroutine(string newText, Action callback)
        {
            SetTxt("");
            for (int i = 0; i < newText.Length; i++)
            {
                SetTxt(text + newText[i]);
                yield return new WaitForSeconds(BattleConsts.I.dialTextDelay);
            }

            callback?.Invoke();
        }
    }
}