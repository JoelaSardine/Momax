﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace rpg
{
    public class RpgManager : MonoBehaviour
    {
        // Shortcuts : 
        // F1 : Unset all keys
        // F2 : Toggle facebookDone
        // F3 : Toggle SeenBull & SeenSnake
        // F4 : Toggle MetAltea
        // F5 : Toggle MetOrion
        // F6 : Toggle SeenMontgeron
        // F7 : Toggle SeenHouse
        // F8 : Toggle DefeatedCerberus
        // F11 : Force save
        // F12 : Toggle collisions

        [System.Flags]
        public enum GameState {
            Undefined = 0,
            MainMenu =  1,
            Rpg =       1 << 1 ,
            Minigame =  1 << 2,
            Menu =      1 << 3,
            End =       1 << 4
        }

        public static CameraManager CameraManager;
        public static RpgManager Instance;
        public static EnnemiesManager Ennemies;
        public static PlayerManager Player;
        public static ProjectilesManager Projectiles;
        public static InteractionManager Interaction;
        public static PlayerHUD HUD;

        public static GameStory CurrentStory;
        public static GameData Data;

        public static string Spawn;


        [Header("Setup")]
        public CameraManager cameraManager;
        public EnnemiesManager ennemies;
        public PlayerManager player;
        public ProjectilesManager projectiles;
        public InteractionManager interaction;
        public PlayerHUD hud;
        public GameDataDebug dataDebug;

        public MenuController menu;
        public Animator zoneBubbleAnimator;
        public DiscussionInterface discussionInterface;

        [Header("Sound FX and Music")]
        public AudioClip sfx_openMenu;
        public AudioClip sfx_closeMenu;
        public AudioClip sfx_refillHP;
        public AudioClip endMusic;
        public bool isEndMusicPlaying = false;

        [Header("Debug")]
        public GameState gameState = GameState.Undefined;

        private Text zoneBubbleText;
        private AsyncOperation unloadingFacebook = null;

        private AudioSource audioSource;
        private List<int> availableGameOverComments1 = new List<int>();
        private List<int> availableGameOverComments2 = new List<int>();

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
            audioSource = GetComponent<AudioSource>();
            RpgManager.Data = dataDebug.GetData();

            zoneBubbleText = zoneBubbleAnimator.GetComponentInChildren<Text>();

            menu.gameObject.SetActive(false);

            player.Init(projectiles);
            player.movementEnabled = false;
            player.GetComponent<Collider2D>().enabled = false;

            cameraManager.blackScreen.alpha = 1;
            StartCoroutine(FinishLoadSceneCoroutine(null));
        }

        private void Update()
        {
            // F1 : Unset all keys
            // F2 : Toggle facebookDone
            // F3 : Toggle SeenBull & SeenSnake
            // F4 : Toggle MetAltea
            // F5 : Toggle MetOrion
            // F6 : Toggle SeenMontgeron
            // F7 : Toggle SeenHouse
            // F8 : Toggle DefeatedCerberus
            // F11 : Force save
            // F12 : Toggle collisions

            if (Input.GetButtonDown("Back") && gameState != GameState.MainMenu)
            {
                ToggleMenu();
            }
            else if (Input.GetKeyDown(KeyCode.F1))
            {
                /*RpgManager.Data = new GameData();
                dataDebug.SetData(RpgManager.Data);
                RpgManager.ZoneDisplayName("Cheat \n Unset all save keys");*/
                RefillHP();
                RpgManager.ZoneDisplayName("Cheat\nHeal");
            }
            else if (Input.GetKeyDown(KeyCode.F2))
            {
                ToggleKey(SaveKey.facebookDone, true);
            }
            else if (Input.GetKeyDown(KeyCode.F3))
            {
                int value = (GetKey(SaveKey.seenBull) == 1 && GetKey(SaveKey.seenSnake) == 1) ? 0 : 1;
                SetKey(SaveKey.seenBull, value);
                SetKey(SaveKey.seenSnake, value);
                RpgManager.ZoneDisplayName("Cheat\n" + "seenBull and seenSnake\n" + " set to " + value);
                dataDebug.SetData(RpgManager.Data);
            }
            else if (Input.GetKeyDown(KeyCode.F4))
            {
                ToggleKey(SaveKey.metAltea);
            }
            else if (Input.GetKeyDown(KeyCode.F5))
            {
                ToggleKey(SaveKey.metOrion);
            }
            else if (Input.GetKeyDown(KeyCode.F6))
            {
                ToggleKey(SaveKey.seenMontgeron);
            }
            else if (Input.GetKeyDown(KeyCode.F7))
            {
                ToggleKey(SaveKey.seenHouse);
            }
            else if (Input.GetKeyDown(KeyCode.F8))
            {
                ToggleKey(SaveKey.defeatedCerberus, true);
            }
            else if (Input.GetKeyDown(KeyCode.F11))
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    RpgManager.Data = GameData.LoadFromFile();
                    RpgManager.ZoneDisplayName("Cheat\nForce load");
                    dataDebug.SetData(RpgManager.Data);
                    LoadScene(dataDebug.scene, dataDebug.place);
                }
                else
                {
                    GameData.SaveToFile(RpgManager.Data);
                    RpgManager.ZoneDisplayName("Cheat\nForce save");
                }
            }
            else if (Input.GetKeyDown(KeyCode.F12))
            {
                Collider2D c = player.GetComponent<Collider2D>();
                c.enabled = !c.enabled;
                RpgManager.ZoneDisplayName("Cheat\nCollisions " + (c.enabled ? "Activées" : "Désactivées"));
            }
        }

        public void ToggleMenu()
        {
            if (gameState == (gameState | GameState.Menu))
            {
                player.enabled = true;
                menu.Close();
                gameState ^= GameState.Menu;
                PlaySFX(sfx_closeMenu);
            }
            else if (gameState == GameState.Rpg)
            {
                player.enabled = false;
                player.Stop();
                menu.gameObject.SetActive(true);
                gameState ^= GameState.Menu;
                PlaySFX(sfx_openMenu);
            }
        }

        private void ToggleKey(SaveKey key, bool defaultIsZero = false)
        {
            if (defaultIsZero)
                SetKey(key, GetKey(key) == 1 ? 0 : 1);
            else
                SetKey(key, GetKey(key) == 0 ? 1 : 0);
            RpgManager.ZoneDisplayName("Cheat\n" + key.ToString() + " set to " + GetKey(key));
            dataDebug.SetData(RpgManager.Data);
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
            if (spawn != null && player != null)
            {
                Transform target = GameObject.Find("SpawnPoints").transform.Find(spawn);
                player.transform.position = target.position;
            }

            yield return StartCoroutine(cameraManager.FadeOutCoroutine());
            if (player && !(CurrentStory is GameOverStory))
            {
                player.GetComponent<Collider2D>().enabled = true;
                player.movementEnabled = true;
            }
        }

        #region Facebook

        public static void LoadFacebookScene()
        {
            Instance.StartCoroutine(Instance.LoadUnloadFacebookSceneCoroutine(true));
        }

        private IEnumerator LoadUnloadFacebookSceneCoroutine(bool load)
        {
            if (load)
            {
                player.movementEnabled = false;
            }

            yield return StartCoroutine(cameraManager.FadeInCoroutine());

            if (load)
            {
                SceneManager.LoadScene("FacebookConversation", LoadSceneMode.Additive);
                RpgManager.CameraManager.ChangeCameraOutputSize(0.5f);
                yield return StartCoroutine(cameraManager.FadeOutCoroutine());
            }
            else
            {
                AsyncOperation ao = Instance.unloadingFacebook = SceneManager.UnloadSceneAsync("FacebookConversation");
                yield return new WaitUntil(() => ao.isDone);

                CameraManager.ChangeCameraOutputSize(1.0f);
                yield return StartCoroutine(cameraManager.FadeOutCoroutine());
                RpgManager.OnEndUnloadingFacebook();
            }
        }
        public static void UnloadFacebook()
        {
            Instance.StartCoroutine(Instance.LoadUnloadFacebookSceneCoroutine(false));
        }
        public static void OnEndUnloadingFacebook()
        {
            if (CurrentStory is NeuillyPlaisanceStory)
            {
                NeuillyPlaisanceStory story = (NeuillyPlaisanceStory)CurrentStory;

                story.OnEndFacebook();
            }
        }

        #endregion Facebook

        public static void UnloadPokemon()
        {
            AsyncOperation ao = Instance.unloadingFacebook = SceneManager.UnloadSceneAsync("PokemonBattle");
            ao.completed += OnEndUnloadingPokemon;
        }
        public static void OnEndUnloadingPokemon(AsyncOperation obj)
        {
            if (CurrentStory is MontgeronStory)
            {
                MontgeronStory story = (MontgeronStory)CurrentStory;

                story.OnEndPokemonBattle();
            }
        }

        public static void ZoneDisplayName(string text)
        {
            Instance.zoneBubbleText.text = text;
            Instance.zoneBubbleAnimator.SetTrigger("Open");
        }

        public static pokemonBattle.Textshadow DialogueTalk(bool? left, string text = "")
        {
            if (left == null)
            {
                Instance.discussionInterface.EndTalk();
                return null;
            }
            else if (left == true)
            {
                Instance.player.movementEnabled = false;
                return Instance.discussionInterface.TalkLeft(text);
            }
            else 
            {
                return Instance.discussionInterface.TalkRight(text);
            }
        }

        public static void SetKey(SaveKey key, int value)
        {
            Data.SetKey(key, value);
            RpgManager.Instance.dataDebug.SetData(Data);
        }
        public static int GetKey(SaveKey key)
        {
            return Data.GetKey(key);
        }

        public static void PlaySFX(AudioClip clip, float volume = 1.0f)
        {
            Instance.audioSource.PlayOneShot(clip, volume);
        }
        
        public static void PlayEndMusic()
        {
            Instance.audioSource.PlayOneShot(Instance.endMusic);
            Instance.isEndMusicPlaying = true;
        }

        public static void StopEndMusic()
        {
            if (Instance.isEndMusicPlaying)
            {
                Instance.audioSource.Stop();
                Instance.isEndMusicPlaying = false;
            }            
        }

        public int GetGameOverCommentId(bool first, int commentsBaseCount)
        {
            List<int> list = first ? availableGameOverComments1 : availableGameOverComments2;
            if (list.Count == 0)
            {
                for (int i = 0; i < commentsBaseCount; i++)
                {
                    list.Add(i);
                }
            }

            int randomIndex = UnityEngine.Random.Range(0, list.Count);
            int commentID = list[randomIndex];
            list.RemoveAt(randomIndex);
            return commentID;
        }

        public static void RefillHP(bool mute = false)
        {
            Player.pv = 3;
            RpgManager.HUD.UpdateHearts(Player.pv, 3);
            if (!mute) RpgManager.PlaySFX(RpgManager.Instance.sfx_refillHP);
        }
        
        public static void GameOver()
        {

            CurrentStory.StopMusic();

            RpgManager.Spawn = null;
            SceneManager.LoadSceneAsync("GameOver", LoadSceneMode.Additive);
        }

        public static void SaveGame(string place)
        {
            HUD.TriggerSaveIcon();

            RpgManager.Data.scene = SceneManager.GetActiveScene().name;
            RpgManager.Data.place = place;
            RpgManager.Instance.dataDebug.SetData(RpgManager.Data);
            GameData.SaveToFile(RpgManager.Data);
        }
    }
}