using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace pokemonBattle
{
    public class BattleChoice : MonoBehaviour
    {
        private GameObject selectBox;

        private void Awake()
        {
            selectBox = transform.Find("Select").gameObject;
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