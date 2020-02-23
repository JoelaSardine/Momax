using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace facebook
{
    public class FacebookChoicePanel : MonoBehaviour
    {
        public GameObject p_PlayerChoice;
        public List<FacebookChoiceObject> choices;

        public int selected = 0;
        public int activeCount = 2;

        private void Awake()
        {
            foreach (Transform item in transform)
            {
                choices.Add(item.GetComponent<FacebookChoiceObject>());
            }
        }

        public void Init()
        {

        }

        public void UpdateChoices(List<FacebookChoice> newChoices)
        {
            List<FacebookChoice> tmp = new List<FacebookChoice>(newChoices);
            newChoices.Clear();
            while (tmp.Count > 0)
            {
                int rdm = UnityEngine.Random.Range(0, tmp.Count);
                newChoices.Add(tmp[rdm]);
                tmp.RemoveAt(rdm);
            }


            for (int i = 0; i < Mathf.Max(choices.Count, newChoices.Count); i++)
            {
                if (i >= newChoices.Count)
                    choices[i].gameObject.SetActive(false);
                else
                {
                    if (i >= choices.Count)
                        choices.Add(Instantiate(p_PlayerChoice, transform).GetComponent<FacebookChoiceObject>());
                    else
                        choices[i].gameObject.SetActive(true);

                    choices[i].text.text = newChoices[i].choice;
                    choices[i].SetSelected(false);
                }
            }

            activeCount = newChoices.Count;
            selected = 0;
            choices[selected].SetSelected(true);
        }

        public void Move(string direction)
        {
            choices[selected].SetSelected(false);
            switch (direction)
            {
                case "up":
                case "left":
                    --selected;
                    break;
                case "right":
                case "down":
                    ++selected;
                    break;
                default: break;
            }
            selected = Mathf.Clamp(selected, 0, activeCount - 1);

            choices[selected].SetSelected(true);
        }
    }
}