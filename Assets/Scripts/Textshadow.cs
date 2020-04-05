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
        private Coroutine currentCoroutine = null;
        public bool isWriting = false;
        private string targetText;

        public string text = "text";
        public int size = 20;

        public Text placeholderText;
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
        
        // for inspector
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
            targetText = newText;
            currentCoroutine = StartCoroutine(SetTextCoroutine(showImage, callback));
        }

        private IEnumerator SetTextCoroutine(bool showImage, Action callback)
        {
            isWriting = true;
            if (placeholderText)
            {
                placeholderText.text = targetText;
            }
            ShowImg(false);
            SetTxt("");
            for (int i = 0; i < targetText.Length; i++)
            {
                SetTxt(text + targetText[i]);
                if (BattleConsts.I)
                {
                    yield return new WaitForSeconds(BattleConsts.I.dialTextDelay);
                }
                else
                {
                    yield return new WaitForSeconds(0.03f);
                }
            }

            ShowImg(showImage);
            isWriting = false;

            callback?.Invoke();
        }

        public void ShowImg(bool state)
        {
            if (endTxtImage)
            {
                endTxtImage.gameObject.SetActive(state);
            }
        }

        public void EndSetTextCoroutine()
        {
            if (isWriting)
            {
                StopCoroutine(currentCoroutine);
                isWriting = false;

                SetTxt(targetText);
            }
            ShowImg(true);
        }
    }
}