using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace pokemonBattle
{
    public class BattleManager : MonoBehaviour
    {
        // Begin with transition apparition
        // First, dialog introduction
        // Then first choice. 
        // -> Song
        //      -> Choice 4 songs
        // -> Fight
        //      -> Choice 4 punches
        // -> Bag
        //      -> ???????????
        // -> Flee
        //      -> Dialog box

        public enum BATTLESTATE
        {
            DEFAULT = 0,
            CHOICES,
            SONG, // choice
            PUNCH, // choice
            ATTACKSPLAY,
            ENNEMYTURN
        }

        private bool busy = false;
        private bool waitForInput = false;
        private Textshadow bigTxt;
        private Textshadow smallTxt;
        private Textshadow descPpTxt;
        private Textshadow descTypeTxt;
        private ActionPanel choicesPanel;
        private ActionPanel songPanel; // Serve for songs & attacks
        private ActionPanel punchPanel; // Serve for songs & attacks
        private GameObject attackDescriptionPanel;

        private EnnemyAttacks ennemyAttacks;

        private AudioSource audioSource;
        public AudioClip ennemyCry;

        public FighterBattle player;
        public FighterBattle ennemy;
        public Transform actionsPanel;

        public BATTLESTATE battleState;


        private void Awake()
        {
            player.Init();
            ennemy.Init();

            ennemyAttacks = GetComponent<EnnemyAttacks>();

            Transform dialPanel = actionsPanel.Find("DialogPanel");
            bigTxt = dialPanel.Find("DialogBig").GetComponentInChildren<Textshadow>();
            smallTxt = dialPanel.Find("DialogSmall").GetComponentInChildren<Textshadow>();
            choicesPanel = actionsPanel.Find("ChoicesPanel").GetComponent<ActionPanel>();
            songPanel = actionsPanel.Find("SongPanel").GetComponent<ActionPanel>();
            punchPanel = actionsPanel.Find("PunchPanel").GetComponent<ActionPanel>();
            Transform descPanel = actionsPanel.Find("AttacksDescriptionPanel");
            attackDescriptionPanel = descPanel.gameObject;
            descPpTxt = descPanel.Find("PP").GetComponentInChildren<Textshadow>();
            descTypeTxt = descPanel.Find("Type").GetComponentInChildren<Textshadow>();

            audioSource = GetComponent<AudioSource>();

            choicesPanel.Init(descPpTxt, descTypeTxt);
            songPanel.Init(descPpTxt, descTypeTxt);
            punchPanel.Init(descPpTxt, descTypeTxt);
        }

        private void Start()
        {
            bigTxt.Shutup();
            smallTxt.Shutup();

            GameObject trGo = GameObject.FindGameObjectWithTag("Transition");
            if (trGo)
            {
                InitTransition();
                TransitionBattle trbattle = trGo.GetComponent<TransitionBattle>();
                trbattle.onOpeningFinished = delegate () { StartCoroutine(IntroTransitionCoroutine()); };
                trbattle.StartShutterCoroutine();
            }
            else
            {
                StartCoroutine(introCoroutine());
            }            
        }

        private void DefaultCallback()
        {
            busy = false;
        }

        private void Update()
        {
            if (!busy)
            {
                float h = Input.GetAxisRaw("Horizontal");
                float v = Input.GetAxisRaw("Vertical");
                if (h > 0.5f)
                {
                    Move("right");
                }
                else if (h < -0.5f)
                {
                    Move("left");
                }
                if (v > 0.5f)
                {
                    Move("up");
                }
                else if (v < -0.5f)
                {
                    Move("down");
                }

                if (Input.GetButtonDown("Fire"))
                {
                    waitForInput = false;
                    Select();
                }
                else if (Input.GetButtonDown("Back"))
                {
                    waitForInput = false;
                    Back();
                }
            }
        }

        private void Move(string direction)
        {
            switch (battleState)
            {
                case BATTLESTATE.DEFAULT:
                    break;
                case BATTLESTATE.CHOICES:
                    choicesPanel.Move(direction);
                    break;
                case BATTLESTATE.SONG:
                    songPanel.Move(direction);
                    break;
                case BATTLESTATE.PUNCH:
                    punchPanel.Move(direction);
                    break;
                default: break;
            }
        }

        private void Select()
        {
            switch (battleState)
            {
                case BATTLESTATE.DEFAULT:
                    break;
                case BATTLESTATE.CHOICES:
                    string selected = choicesPanel.GetSelectedItem().name;
                    switch (selected)
                    {
                        case "song":
                            battleState = BATTLESTATE.SONG;
                            choicesPanel.gameObject.SetActive(false);
                            songPanel.gameObject.SetActive(true);
                            attackDescriptionPanel.SetActive(true);
                            break;
                        case "punch": 
                            battleState = BATTLESTATE.PUNCH;
                            choicesPanel.gameObject.SetActive(false);
                            punchPanel.gameObject.SetActive(true);
                            attackDescriptionPanel.SetActive(true);
                            break;
                        case "bag":
                            StartCoroutine(bagCoroutine());
                            break;
                        case "flee":
                            StartCoroutine(fleeCoroutine());
                            break;
                        default:
                            break;
                    }
                    break;
                case BATTLESTATE.SONG:
                    StartCoroutine(fightCoroutine(songPanel.GetSelectedItem()));
                    break;
                case BATTLESTATE.PUNCH:
                    StartCoroutine(fightCoroutine(punchPanel.GetSelectedItem()));
                    break;
                default:
                    break;
            }
        }

        private void Back()
        {
            switch (battleState)
            {
                case BATTLESTATE.DEFAULT:
                    break;
                case BATTLESTATE.CHOICES: break; // Do nothing
                case BATTLESTATE.SONG:
                    battleState = BATTLESTATE.CHOICES;
                    choicesPanel.gameObject.SetActive(true);
                    songPanel.gameObject.SetActive(false);
                    attackDescriptionPanel.SetActive(false);
                    break;
                case BATTLESTATE.PUNCH:
                    battleState = BATTLESTATE.CHOICES;
                    choicesPanel.gameObject.SetActive(true);
                    punchPanel.gameObject.SetActive(false);
                    attackDescriptionPanel.SetActive(false);
                    break;
                case BATTLESTATE.ATTACKSPLAY:
                    break;
                default:
                    break;
            }
        }

        private IEnumerator introCoroutine()
        {
            busy = waitForInput = true;
            smallTxt.Shutup();
            bigTxt.Display("MORGANE est attaquée par ENSIL-ENSCI !", true, DefaultCallback);
            while (busy || waitForInput)
                yield return null;
            
            busy = true;
            bigTxt.Shutup();
            smallTxt.Display(BattleConsts.I.choiceText, false, DefaultCallback);
            while (busy)
                yield return null;
            choicesPanel.gameObject.SetActive(true);
            smallTxt.ShowImg(false);
            battleState = BATTLESTATE.CHOICES;
        }

        private IEnumerator fightCoroutine(BattleChoice choice)
        {
            battleState = BATTLESTATE.ATTACKSPLAY;
            choicesPanel.gameObject.SetActive(false);
            songPanel.gameObject.SetActive(false);
            punchPanel.gameObject.SetActive(false);
            attackDescriptionPanel.gameObject.SetActive(false);

            // Display text
            busy = waitForInput = true;
            smallTxt.Shutup();
            bigTxt.Display(choice.message, true, DefaultCallback);
            while (busy)
                yield return null;
            yield return new WaitForSeconds(1f);

            // Play animations
            busy = true;
            ennemy.PlayAnim("Hit");
            StartCoroutine(ennemy.ModifyHpCoroutine(-choice.damage, DefaultCallback));
            while (busy || waitForInput)
                yield return null;

            // Display result if any
            if (string.IsNullOrEmpty(choice.resultMessage))
            {
                busy = true;
                bigTxt.Display(choice.resultMessage, false, DefaultCallback);
            }

            while (busy)
                yield return null;

            if (ennemy.Hp <= 0)
                StartCoroutine(endCoroutine(true));
            else 
                StartCoroutine(ennemyCoroutine());
        }

        private IEnumerator ennemyCoroutine()
        {
            battleState = BATTLESTATE.ENNEMYTURN;
            EnnemyAttack atk = ennemyAttacks.GetRandomAttack();
            
            // Display text
            busy = waitForInput = true;
            smallTxt.Shutup();
            bigTxt.Display(atk.label, true, DefaultCallback);
            while (busy)
                yield return null;
            yield return new WaitForSeconds(1f);

            // Play animations
            busy = true;
            player.PlayAnim("Hit");
            StartCoroutine(player.ModifyHpCoroutine(-atk.damage, DefaultCallback));
            while (busy || waitForInput)
                yield return null;

            if (player.Hp <= 0)
            {
                StartCoroutine(endCoroutine(false));
            }
            else
            {
                busy = true;
                bigTxt.Shutup();
                smallTxt.Display(BattleConsts.I.choiceText, false, DefaultCallback);
                while (busy)
                    yield return null;
                choicesPanel.gameObject.SetActive(true);
                battleState = BATTLESTATE.CHOICES;
            }
        }

        private IEnumerator bagCoroutine()
        {
            battleState = BATTLESTATE.DEFAULT;
            choicesPanel.gameObject.SetActive(false);

            busy = waitForInput = true;
            smallTxt.Shutup();
            bigTxt.Display(BattleConsts.I.bagText, true, DefaultCallback);

            while (busy || waitForInput) yield return null;

            busy = true;
            bigTxt.Shutup();
            smallTxt.Display(BattleConsts.I.choiceText, false, DefaultCallback);
            while (busy)
                yield return null;
            choicesPanel.gameObject.SetActive(true);
            battleState = BATTLESTATE.CHOICES;
        }

        private IEnumerator fleeCoroutine()
        {
            battleState = BATTLESTATE.DEFAULT;
            choicesPanel.gameObject.SetActive(false);

            busy = waitForInput = true;
            smallTxt.Shutup();
            bigTxt.Display(BattleConsts.I.fleeText, true, DefaultCallback);
            while (busy || waitForInput) yield return null;

            busy = true;
            bigTxt.Shutup();
            smallTxt.Display(BattleConsts.I.choiceText, false, DefaultCallback);
            while (busy)
                yield return null;
            choicesPanel.gameObject.SetActive(true);
            battleState = BATTLESTATE.CHOICES;
        }

        private IEnumerator endCoroutine(bool win)
        {
            if (rpg.RpgManager.Instance)
            {
                rpg.RpgManager.CurrentStory.StopMusic();
            }
            audioSource.Play();

            (win ? ennemy : player).animator.SetTrigger("Dead");

            battleState = BATTLESTATE.DEFAULT;
            busy = waitForInput = true;
            smallTxt.Shutup();
            bigTxt.Display(win ? BattleConsts.I.winText : BattleConsts.I.loseText, true, DefaultCallback);
            while (busy || waitForInput) yield return null;
        }

        private void InitTransition()
        {
            ennemy.avatarPanel.anchorMin += Vector2.left;
            ennemy.avatarPanel.anchorMax += Vector2.left;
            ennemy.statsPanel.anchorMin += Vector2.left;
            ennemy.statsPanel.anchorMax += Vector2.left;
            player.avatarPanel.anchorMin += Vector2.right;
            player.avatarPanel.anchorMax += Vector2.right;
            player.statsPanel.anchorMin += Vector2.right;
            player.statsPanel.anchorMax += Vector2.right;
        }

        private IEnumerator IntroTransitionCoroutine()
        {
            float time = BattleConsts.I.introAvatarDuration;
            float timer = 0;
            float value = 1; // from 1 to 0

            Vector2 ennemyMin = ennemy.avatarPanel.anchorMin;
            Vector2 ennemyMax = ennemy.avatarPanel.anchorMax;
            Vector2 playerMin = player.avatarPanel.anchorMin;
            Vector2 playerMax = player.avatarPanel.anchorMax;

            while (timer <= time)
            {
                timer += Time.deltaTime;
                value = Mathf.Clamp01((time - timer) / time);

                ennemyMin.x = 0.5f - value;
                ennemyMax.x = 1.0f - value;
                playerMin.x = 0.0f + value;
                playerMax.x = 0.5f + value;

                ennemy.avatarPanel.anchorMin = ennemyMin;
                ennemy.avatarPanel.anchorMax = ennemyMax;
                player.avatarPanel.anchorMin = playerMin;
                player.avatarPanel.anchorMax = playerMax;

                yield return null;
            }

            audioSource.PlayOneShot(ennemyCry);

            time = BattleConsts.I.introStatsDuration;
            timer = 0;
            value = 1;

            ennemyMin = ennemy.statsPanel.anchorMin;
            ennemyMax = ennemy.statsPanel.anchorMax;
            playerMin = player.statsPanel.anchorMin;
            playerMax = player.statsPanel.anchorMax;

            while (timer < time)
            {
                timer += Time.deltaTime;
                value = Mathf.Clamp01((time - timer) / time);

                ennemyMin.x = 0.3f - value;
                ennemyMax.x = 0.3f - value;
                playerMin.x = 0.7f + value;
                playerMax.x = 0.7f + value;

                ennemy.statsPanel.anchorMin = ennemyMin;
                ennemy.statsPanel.anchorMax = ennemyMax;
                player.statsPanel.anchorMin = playerMin;
                player.statsPanel.anchorMax = playerMax;

                yield return null;
            }

            StartCoroutine(introCoroutine());
        }
    }
}