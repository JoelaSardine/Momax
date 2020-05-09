using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace pokemonBattle
{
    public class EnnemyAttacks : MonoBehaviour
    {
        public List<EnnemyAttack> attacks;

        public EnnemyAttack GetRandomAttack()
        {
            int rdm = Random.Range(0, attacks.Count);
            return attacks[rdm];
        }
    }

    [System.Serializable]
    public class EnnemyAttack
    {
        public string label;
        public int damage;
        public int heal;
        public AudioClip sfx;
    }
}