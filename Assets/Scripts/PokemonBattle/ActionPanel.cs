﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace pokemonBattle
{
    public class ActionPanel : MonoBehaviour
    {
        public int selected = 0;

        private List<BattleChoice> choices = new List<BattleChoice>();

        private Textshadow ppTxt;
        private Textshadow typeTxt;

        private void Awake()
        {
            foreach (Transform child in transform)
            {
                choices.Add(child.GetComponent<BattleChoice>());
            }
        }

        private void OnEnable()
        {
            //choices[0].Select(true);
            //selected = 0;
        }

        private void Start()
        {
            Reselect();
        }

        public void Init(Textshadow pp, Textshadow type)
        {
            ppTxt = pp;
            typeTxt = type;
        }

        private void Unselect()
        {
            choices[selected].Select(false);
        }
        private void Reselect()
        {
            choices[selected].Select(true);
        }

        public void Move(string direction)
        {
            choices[selected].Select(false);
            switch (direction)
            {
                case "up":
                    if (selected == 2) selected = 0;
                    else if (selected == 3) selected = 1;
                    break;
                case "left":
                    if (selected == 1) selected = 0;
                    else if (selected == 3) selected = 2;
                    break;
                case "right":
                    if (selected == 0) selected = 1;
                    else if (selected == 2) selected = 3;
                    break;
                case "down":
                    if (selected == 0) selected = 2;
                    else if (selected == 1) selected = 3;
                    break;
                default: break;
            }
            choices[selected].Select(true);
            ppTxt.SetTxt("PP " + choices[selected].currentPp + "/" + choices[selected].basePp);
            typeTxt.SetTxt(choices[selected].type);
        }

        public BattleChoice GetSelectedItem()
        {
            return choices[selected];
        }
    }
}