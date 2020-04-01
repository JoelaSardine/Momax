﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace rpg
{
    public class RpgManager : MonoBehaviour
    {
        public CameraManager cameraManager;
        public EnnemiesManager ennemies;
        public PlayerManager player;
        public ProjectilesManager projectiles;
        public InteractionManager interaction;
        public PlayerHUD hud;

        public static CameraManager CameraManager;
        public static RpgManager Instance;
        public static EnnemiesManager Ennemies;
        public static PlayerManager Player;
        public static ProjectilesManager Projectiles;
        public static InteractionManager Interaction;
        public static PlayerHUD HUD;

        public static string Spawn;
        public static bool SceneJustLoaded = false;

        private void Awake()
        {
            if (Instance == null)
            {
                RpgManager.Instance = this;

                RpgManager.CameraManager = cameraManager;
                RpgManager.Ennemies = ennemies;
                RpgManager.Player = player;
                RpgManager.Projectiles = projectiles;
                RpgManager.Interaction = interaction;
                RpgManager.HUD = hud;

                DontDestroyOnLoad(gameObject);
            }
            else
            {
                //RpgManager.SceneJustLoaded = true;
                RpgManager.Instance.StartCoroutine(RpgManager.Instance.FinishLoadSceneCoroutine(RpgManager.Spawn));
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            player.Init(projectiles);
        }

        public static void LoadScene(string scene, string spawn)
        {
            RpgManager.Spawn = spawn;
            Instance.StartCoroutine(Instance.BeginLoadSceneCoroutine(scene));
        }

        private IEnumerator LoadSceneCoroutine(string scene)
        {
            player.movementEnabled = false;
            yield return StartCoroutine(cameraManager.FadeInCoroutine());
            SceneManager.LoadScene(scene);
            yield return StartCoroutine(cameraManager.FadeOutCoroutine());
            player.movementEnabled = true;
        }

        private IEnumerator BeginLoadSceneCoroutine(string scene)
        {
            player.movementEnabled = false;
            player.GetComponent<Collider2D>().enabled = false;
            yield return StartCoroutine(cameraManager.FadeInCoroutine());
            SceneManager.LoadScene(scene);
        }
        private IEnumerator FinishLoadSceneCoroutine(string spawn)
        {
            Transform target = GameObject.Find("SpawnPoints").transform.Find(spawn);
            player.transform.position = target.position;

            yield return null; StartCoroutine(cameraManager.FadeOutCoroutine());
            player.GetComponent<Collider2D>().enabled = true;
            player.movementEnabled = true;
        }
    }
}