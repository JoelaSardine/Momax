using pokemonBattle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace rpg
{
    public class DiscussionInterface : MonoBehaviour
    {
        public Image left_image;
        public Textshadow left_text;
        public Image right_image;
        public Textshadow right_text;

        public void SetImage(bool isLeft, Sprite sprite)
        {
            if (isLeft) 
            {
                left_image.sprite = sprite;
            }
            else
            {
                right_image.sprite = sprite;
            }
        }

        public Textshadow TalkLeft(string text)
        {
            this.gameObject.SetActive(true);

            left_image.gameObject.SetActive(true);
            left_text.gameObject.SetActive(true);
            right_image.gameObject.SetActive(false);
            right_text.gameObject.SetActive(false);

            left_text.Display(text, true);

            return left_text;
        }

        public Textshadow TalkRight(string text)
        {
            this.gameObject.SetActive(true);

            left_image.gameObject.SetActive(false);
            left_text.gameObject.SetActive(false);
            right_image.gameObject.SetActive(true);
            right_text.gameObject.SetActive(true);
            
            right_text.Display(text, true);

            return right_text;
        }

        public void EndTalk()
        {
            gameObject.SetActive(false);
        }
    }
}