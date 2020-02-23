using UnityEngine;
using UnityEngine.UI;

namespace facebook
{
    public class FacebookChoiceObject : MonoBehaviour
    {
        public GameObject background;
        public GameObject arrow;
        public Text text;

        private void Awake()
        {
            Reset();
        }

        public void Reset(string t = "")
        {
            SetSelected(false);
            text.text = t;
        }

        public void SetSelected(bool state)
        {
            background.SetActive(state);
            arrow.SetActive(state);
        }
    }
}