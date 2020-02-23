using System;
using UnityEngine;
using UnityEngine.UI;

namespace facebook
{
    public class FacebookPhraseObject : MonoBehaviour
    {
        public GameObject picture;
        public Text text;

        public void SetText(string t)
        {
            text.text = t;
        }

        public void ShowPicture(bool v)
        {
            picture.SetActive(v);
        }
    }
}