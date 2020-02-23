using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace facebook
{
    [System.Serializable]
    public class FacebookPhrase
    {
        public enum Talker { None, Mo, Max }

        public Talker talker;
        [TextArea]
        public string phrase;
    }
}