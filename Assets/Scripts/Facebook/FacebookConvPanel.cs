using System;
using UnityEngine;
using UnityEngine.UI;

namespace facebook
{
    public class FacebookConvPanel : MonoBehaviour
    {
        public GameObject p_maxReply;
        public GameObject p_moReply;
        public GameObject p_interReply;

        public GameObject replyInProgress;
        public ScrollRect scrollView;
        public Transform container;
        //public Text chatbox;

        public FacebookPhraseObject lastMaxPhrase;

        private int updateScrollPos = -1;

        private void Awake()
        {
            for (int i = container.childCount - 1; i >= 0; --i)
            {
                GameObject child = container.GetChild(i).gameObject;
                if (child != replyInProgress)
                {
                    Destroy(container.GetChild(i).gameObject);
                }
            }
        }

        public void Blank()
        {
            lastMaxPhrase = null;
            Instantiate(p_interReply, container);

            replyInProgress.transform.SetSiblingIndex(container.childCount);
        }

        public void DisplayIsWriting(bool v)
        {
            replyInProgress.SetActive(v);

            UpdateViewPosition();
        }

        public void WriteMo(string text)
        {
            FacebookPhraseObject phrase = Instantiate(p_moReply, container).GetComponent<FacebookPhraseObject>();
            phrase.SetText(text);

            replyInProgress.transform.SetSiblingIndex(container.childCount);

            UpdateViewPosition();
        }

        public void WriteMax(string text)
        {
            if (container.childCount == 1)
                Blank();

            if (lastMaxPhrase != null)
                lastMaxPhrase.ShowPicture(false);

            FacebookPhraseObject phrase = Instantiate(p_maxReply, container).GetComponent<FacebookPhraseObject>();
            phrase.SetText(text);

            lastMaxPhrase = phrase;
            lastMaxPhrase.ShowPicture(true);

            replyInProgress.transform.SetSiblingIndex(container.childCount);

            UpdateViewPosition();
        }

        public void UpdateViewPosition()
        {
            updateScrollPos = 1;
        }

        private void Update()
        {
            if (updateScrollPos == 0)
            {
                updateScrollPos = -1;
                scrollView.verticalScrollbar.value = 0.0f;
            }
            else if (updateScrollPos > 0)
            {
                updateScrollPos--;
            }
        }
    }
}