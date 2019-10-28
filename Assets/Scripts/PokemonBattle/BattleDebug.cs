using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace pokemonBattle
{
    public class BattleDebug : MonoBehaviour
    {
        public BattleManager manager;

        public Text textPV;

        private int pv = 0;

        public void SetPv(float value)
        {
            pv = Mathf.RoundToInt(value * 100f);
            textPV.text = "" + pv;
        }

        public void UpdatePv()
        {
            StartCoroutine(manager.player.SetHpCoroutine(pv));
        }
    }
}