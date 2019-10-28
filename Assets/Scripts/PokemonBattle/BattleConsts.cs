using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace pokemonBattle
{
    public class BattleConsts : MonoBehaviour
    {
        public static BattleConsts I;

        private void Awake()
        {
            I = this;
        }

        [Header("Intro")]
        public float introAvatarDuration = 2.0f;
        public float introStatsDuration = 1.0f;

        [Header("Hp bar")]
        public float hpModifDuration = 2.0f;
        public Color hpBaseColor = Color.green;
        [Range(0, 1)] public float hpLowTreshold = 0.4f;
        public Color hpLowColor = Color.yellow;
        [Range(0, 1)] public float hpCritTreshold = 0.2f;
        public Color hpCritColor = Color.red;

        [Header("Dialog")]
        public float dialTextDelay = 0.02f;
        public string choiceText = "Que veux-tu faire ?";
        public string fleeText = "Tu ne peux pas t'enfuir du combat !";
        public string bagText = "Ton sac est vide !";
        public string winText = "Bravo ! Tu as gagné !";
        public string loseText = "Bouh ! Tu as perdu !";
    }
}