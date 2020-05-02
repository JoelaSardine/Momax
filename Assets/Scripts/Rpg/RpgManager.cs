using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace rpg
{
    public class RpgManager : MonoBehaviour
    {
        [System.Flags]
        public enum GameState {
            Undefined,
            MainMenu, 
            Rpg, 
            Facebook,
            Battle,
            Menu
        }

        public static CameraManager CameraManager;
        public static RpgManager Instance;
        public static EnnemiesManager Ennemies;
        public static PlayerManager Player;
        public static ProjectilesManager Projectiles;
        public static InteractionManager Interaction;
        public static PlayerHUD HUD;

        public static GameStory currentStory;

        public static string Spawn;
        public static bool SceneJustLoaded = false;


        [Header("Setup")]
        public CameraManager cameraManager;
        public EnnemiesManager ennemies;
        public PlayerManager player;
        public ProjectilesManager projectiles;
        public InteractionManager interaction;
        public PlayerHUD hud;

        public MenuController menu;
        public Animator zoneBubbleAnimator;
        public DiscussionInterface discussionInterface;

        [Header("Debug")]
        public GameState gameState = GameState.Undefined;
        
        [Header("Save keys")]
        public bool key_fb = false;
        public bool key_blockedRoad = false;
        public int key_seenSnake = 0; // -1 before, 0 unseen, 1 after
        public bool key_altea = false;
        public bool key_orion = false;
        public bool key_montgeron = false;
        public bool key_seenHouse = false;
        public bool key_defeatedCerberus = false;

        private Text zoneBubbleText;
        private AsyncOperation unloadingFacebook = null;


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
            zoneBubbleText = zoneBubbleAnimator.GetComponentInChildren<Text>();

            menu.gameObject.SetActive(false);

            player.Init(projectiles);
            player.movementEnabled = false;
            player.GetComponent<Collider2D>().enabled = false;

            cameraManager.blackScreen.alpha = 1;
            StartCoroutine(FinishLoadSceneCoroutine(null));
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

            yield return null; StartCoroutine(cameraManager.FadeOutCoroutine());
            if (player)
            {
                player.GetComponent<Collider2D>().enabled = true;
                player.movementEnabled = true;
            }
        }

        public static void UnloadFacebook()
        {
            AsyncOperation ao = Instance.unloadingFacebook = SceneManager.UnloadSceneAsync("FacebookConversation");
            ao.completed += OnEndUnloadingFacebook;
        }
        public static void OnEndUnloadingFacebook(AsyncOperation obj)
        {
            CameraManager.ChangeCameraOutputSize(1.0f);

            if (currentStory is NeuillyPlaisanceStory)
            {
                NeuillyPlaisanceStory story = (NeuillyPlaisanceStory)currentStory;

                story.OnEndFacebook();
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
    }
}