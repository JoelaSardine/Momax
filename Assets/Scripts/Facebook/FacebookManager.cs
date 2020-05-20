using pokemonBattle;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace facebook
{
    public class FacebookManager : MonoBehaviour
    {
        private AudioSource audioSource;
        public AudioClip sfx_notification;
        public AudioClip sfx_send;

        public enum MessengerState {
            None,
            PickAnswer,
            MaxWriting,
            MoWriting
        }

        private bool busy = false;
        private bool waitForInput = false;
        public bool quickRestart = false;

        public FacebookConvPanel conversationPanel;
        public FacebookChoicePanel choicePanel;
        public Camera sceneCamera;

        public Text textInputPlaceholder;
        public Textshadow textInput;

        private CustomEventsInput inputs;
        private FacebookConv conv;
        private FacebookDialog currentDialog;
        private int currentPhraseId;
        private int currentDialogueId = 0;
        private int previousDialogueId = 0;

        public float delayPerLetter = 0.1f;
        public float delayBetweenReplies = 1.0f;
        
        public MessengerState state = MessengerState.None;

        private void Awake()
        {
            if (sceneCamera != Camera.main)
            {
                sceneCamera.gameObject.SetActive(false);
            }

            audioSource = GetComponent<AudioSource>();
            inputs = GetComponent<CustomEventsInput>();
            conv = GetComponent<FacebookConv>();
            CloseAnswerPanel();
        }

        private void Start()
        {
            textInput.SetTxt("");

            currentDialog = conv.dialogs[0];
            currentPhraseId = 0;

            Next();
        }

        private void Update()
        {
            if (!busy)
            {
                if (waitForInput && Input.GetKeyDown(KeyCode.F2))
                {
                    waitForInput = false;
                    currentDialog = conv.dialogs[30];
                    currentPhraseId = 2;
                    Next();
                    //End();
                }
                else if (Input.GetButtonDown("Vertical") || inputs.buttonDown || inputs.buttonUp)
                {
                    Move(Input.GetAxis("Vertical") > 0 ? "up" : "down");
                }
                /*else if (Input.GetButtonDown("Left"))
                {
                    Move("left");
                }
                if (Input.GetButtonDown("Up"))
                {
                    Move("up");
                }
                else if (Input.GetButtonDown("Down"))
                {
                    Move("down");
                }*/

                if (Input.GetButtonDown("Fire") && waitForInput)
                {
                    waitForInput = false;
                    Select();
                }
                /*else if (Input.GetButtonDown("Back"))
                {
                    waitForInput = false;
                    Back();
                }*/
            }
        }

        private void Move(string direction)
        {
            if (state == MessengerState.PickAnswer)
            {
                choicePanel.Move(direction);
            }
        }

        private void Select()
        {
            if (state == MessengerState.PickAnswer && choicePanel.selected >= 0)
            {
                if (currentDialog.choices[choicePanel.selected].dialogId == -1)
                {
                    End();
                    return;
                }

                if (quickRestart && currentDialog.choices[choicePanel.selected].dialogId == 0)
                {
                    currentDialogueId = previousDialogueId;
                    currentDialog = conv.dialogs[currentDialogueId];
                    currentPhraseId = currentDialog.phrases.Count;
                }
                else
                {
                    previousDialogueId = currentDialogueId;
                    currentDialogueId = currentDialog.choices[choicePanel.selected].dialogId;
                    currentDialog = conv.dialogs[currentDialogueId];
                }
            }

            Next();
        }

        private void Back()
        {
        }

        private void End()
        {
            if (rpg.RpgManager.Instance == null)
            {
                Debug.Log("The end.");
                return;
            }

            rpg.RpgManager.UnloadFacebook();
        }

        private void Next()
        {
            CloseAnswerPanel();

            if (currentPhraseId < currentDialog.phrases.Count)
            {
                FacebookPhrase phrase = currentDialog.phrases[currentPhraseId];

                if (phrase.talker == FacebookPhrase.Talker.Max)
                {
                    StartCoroutine(MaxWritingCoroutine(phrase.phrase));
                }
                else if (phrase.talker == FacebookPhrase.Talker.Mo)
                {
                    StartCoroutine(MoWritingCoroutine(phrase.phrase));
                }
                else
                {
                    Debug.LogError("Invalid talker");
                }

                currentPhraseId++;
            }
            else
            {
                // Lose a life
                if (rpg.RpgManager.Instance && currentDialog.choices[0].dialogId == 0)
                {
                    rpg.RpgManager.Player.GetHitFunc();
                    if (rpg.RpgManager.Player.pv <= 0)
                    {
                        rpg.RpgManager.CameraManager.ChangeCameraOutputSize(1);
                        //enabled = false;
                        this.gameObject.SetActive(false);
                        return;
                    }
                }

                OpenAnswerPanel();

                currentPhraseId = 0;
            }

        }

        private void CloseAnswerPanel()
        {
            choicePanel.gameObject.SetActive(false);
        }

        private void OpenAnswerPanel()
        {
            state = MessengerState.PickAnswer;
            waitForInput = true;

            choicePanel.gameObject.SetActive(true);

            choicePanel.UpdateChoices(currentDialog.choices);
        }

        private IEnumerator MaxWritingCoroutine(string text)
        {
            state = MessengerState.MaxWriting;

            yield return new WaitForSeconds(delayBetweenReplies / 2);
            conversationPanel.DisplayIsWriting(true);
            yield return new WaitForSeconds(text.Length * delayPerLetter);
            conversationPanel.DisplayIsWriting(false);
            //yield return new WaitForSeconds(delayBetweenReplies / 2);
            conversationPanel.WriteMax(text);
            audioSource.PlayOneShot(sfx_notification);

            Next();
        }

        private IEnumerator MoWritingCoroutine(string text)
        {
            state = MessengerState.MoWriting;

            textInputPlaceholder.enabled = false;

            bool wait = true;
            textInput.Display(text, false, () => wait = false);
            yield return new WaitWhile(() => wait);

            yield return new WaitForSeconds(delayBetweenReplies);

            //yield return new WaitForSeconds(delayBetweenReplies + text.Length * delayPerLetter);
            conversationPanel.WriteMo(text);
            audioSource.PlayOneShot(sfx_send);

            textInput.SetTxt("");
            textInputPlaceholder.enabled = true;

            Next();
        }
    }
}