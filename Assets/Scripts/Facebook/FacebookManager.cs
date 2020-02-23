using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace facebook
{
    public class FacebookManager : MonoBehaviour
    {
        public enum MessengerState {
            None,
            PickAnswer,
            MaxWriting,
            MoWriting
        }

        private bool busy = false;
        private bool waitForInput = false;
        public bool debugMode = false;

        public FacebookConvPanel conversationPanel;
        public FacebookChoicePanel choicePanel;

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
            conv = GetComponent<FacebookConv>();
            CloseAnswerPanel();
        }

        private void Start()
        {
            currentDialog = conv.dialogs[0];
            currentPhraseId = 0;

            Next();
        }

        private void Update()
        {
            if (!busy)
            {
                if (Input.GetButtonDown("Right"))
                {
                    Move("right");
                }
                else if (Input.GetButtonDown("Left"))
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
                }

                if (Input.GetButtonDown("Interact") && waitForInput)
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
            if (state == MessengerState.PickAnswer)
            {
                choicePanel.Move(direction);
            }
        }

        private void Select()
        {
            if (state == MessengerState.PickAnswer && choicePanel.selected >= 0)
            {
                if (debugMode && currentDialog.choices[choicePanel.selected].dialogId == 0)
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
            yield return new WaitForSeconds(delayBetweenReplies / 2);
            conversationPanel.WriteMax(text);

            Next();
        }

        private IEnumerator MoWritingCoroutine(string text)
        {
            state = MessengerState.MoWriting;

            yield return new WaitForSeconds(delayBetweenReplies + text.Length * delayPerLetter);
            conversationPanel.WriteMo(text);
            //yield return new WaitForSeconds(delayBetweenReplies + text.Length * delayPerLetter);

            Next();
        }
    }
}