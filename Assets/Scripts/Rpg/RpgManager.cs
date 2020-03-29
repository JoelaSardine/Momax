using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace rpg
{
    public class RpgManager : MonoBehaviour
    { 
        public EnnemiesManager ennemies;
        public PlayerManager player;
        public ProjectilesManager projectiles;
        public InteractionManager interaction;
        public PlayerHUD hud;

        public CanvasGroup blackScreen;

        public static RpgManager Instance;
        public static EnnemiesManager Ennemies;
        public static PlayerManager Player;
        public static ProjectilesManager Projectiles;
        public static InteractionManager Interaction;
        public static PlayerHUD HUD;

        private void Awake()
        {
            if (Instance == null)
            {
                RpgManager.Instance = this;
                RpgManager.Ennemies = ennemies;
                RpgManager.Player = player;
                RpgManager.Projectiles = projectiles;
                RpgManager.Interaction = interaction;
                RpgManager.HUD = hud;

                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            player.Init(projectiles);
        }
    }
}