using System.Collections;
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
            MainMenu = 1,
            Rpg = 2,
            Facebook = 4,
            Battle = 8,
            Menu = 16
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
        public static bool SceneJustLoaded = false;


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

        [Header("Sound FX")]
        public AudioClip sfx_openMenu;
        public AudioClip sfx_closeMenu;

        [Header("Debug")]
        public GameState gameState = GameState.Undefined;

        [Header("Obsolete Save keys")]
        public bool key_blockedRoad = false;
        public int key_seenSnake = 0; // -1 before, 0 unseen, 1 after
        public bool key_altea = false;
        public bool key_orion = false;
        public bool key_montgeron = false;
        public bool key_seenHouse = false;
        public bool key_defeatedCerberus = false;

        private Text zoneBubbleText;
        private AsyncOperation unloadingFacebook = null;

        private AudioSource audioSource;

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

            if (Input.GetKeyDown(KeyCode.Escape) && gameState != GameState.MainMenu)
            {
                ToggleMenu();
            }
            else if (Input.GetKeyDown(KeyCode.F1))
            {
                RpgManager.Data = new GameData();
                dataDebug.SetData(RpgManager.Data);
                RpgManager.ZoneDisplayName("Cheat \n Unset all save keys");
            }
            else if (Input.GetKeyDown(KeyCode.F2))
            {
                ToggleKey(SaveKey.facebookDone);
            }
            else if (Input.GetKeyDown(KeyCode.F3))
            {
                int value = (GetKey(SaveKey.seenBull) == 1 && GetKey(SaveKey.seenSnake) == 1) ? 0 : 1;
                SetKey(SaveKey.seenBull, value);
                SetKey(SaveKey.seenSnake, value);
                RpgManager.ZoneDisplayName("Cheat \n " + "seenBull and seenSnake" + " set to " + value);
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
                ToggleKey(SaveKey.defeatedCerberus);
            }
            else if (Input.GetKeyDown(KeyCode.F11))
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    RpgManager.Data = GameData.LoadFromFile();
                    RpgManager.ZoneDisplayName("Cheat \n Force load");
                    dataDebug.SetData(RpgManager.Data);
                }
                else
                {
                    GameData.SaveToFile(RpgManager.Data);
                    RpgManager.ZoneDisplayName("Cheat \n Force save");
                }
            }
            else if (Input.GetKeyDown(KeyCode.F12))
            {
                Collider2D c = player.GetComponent<Collider2D>();
                c.enabled = !c.enabled;
                RpgManager.ZoneDisplayName("Cheat \n Collisions " + (c.enabled ? "Activées" : "Désactivées"));
            }
        }

        public void ToggleMenu()
        {
            if (gameState == (gameState | GameState.Menu))
            {
                player.enabled = true;
                menu.gameObject.SetActive(false);
                gameState ^= GameState.Menu;
                PlaySFX(sfx_closeMenu);
            }
            else
            {
                player.enabled = false;
                player.Stop();
                menu.gameObject.SetActive(true);
                gameState ^= GameState.Menu;
                PlaySFX(sfx_openMenu);
            }
        }

        private void ToggleKey(SaveKey key)
        {
            SetKey(key, GetKey(key) == 0 ? 1 : 0);
            RpgManager.ZoneDisplayName("Cheat \n " + key.ToString() + " set to " + GetKey(key));
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

            if (CurrentStory is NeuillyPlaisanceStory)
            {
                NeuillyPlaisanceStory story = (NeuillyPlaisanceStory)CurrentStory;

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

        public static void SetKey(SaveKey key, int value)
        {
            Data.SetKey(key, value);
        }
        public static int GetKey(SaveKey key)
        {
            return Data.GetKey(key);
        }

        public static void PlaySFX(AudioClip clip)
        {
            Instance.audioSource.PlayOneShot(clip);
        }
    }
}