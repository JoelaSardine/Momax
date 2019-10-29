﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace pokemonBattle
{
    public class BattleChoice : MonoBehaviour
    {
        private GameObject selectBox;

        public bool isBattleChoice = false;

        public int damage = 10;
        public string message = "Attaque !";
        public string resultMessage = "";
        public int basePp = 10;
        public int currentPp = 10;
        public string type = "";

        private void Awake()
        {
            selectBox = transform.Find("Select").gameObject;
            currentPp = basePp;
        }

        private void Start()
        {
            selectBox.SetActive(false);
        }

        public void Select(bool state)
        {
            if (selectBox)
            {
                selectBox.SetActive(state);
            }
        }

    }
}