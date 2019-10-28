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

        public Image endTxtImage;

        private void Awake()
        {
            myText = GetComponent<Text>();

            if (endTxtImage)
            {
                endTxtImage.gameObject.SetActive(false);
            }
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
            ShowImg(false);
        }

        public void Display(string newText, bool showImage, Action callback = null)
        {
            StartCoroutine(SetTextCoroutine(newText, showImage, callback));
        }

        private IEnumerator SetTextCoroutine(string newText, bool showImage, Action callback)
        {
            ShowImg(false);
            SetTxt("");
            for (int i = 0; i < newText.Length; i++)
            {
                SetTxt(text + newText[i]);
                yield return new WaitForSeconds(BattleConsts.I.dialTextDelay);
            }

            ShowImg(showImage);
            callback?.Invoke();
        }

        public void ShowImg(bool state)
        {
            if (endTxtImage)
            {
                endTxtImage.gameObject.SetActive(state);
            }
        }
    }
}