using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace facebook
{
    [System.Serializable]
    public class FacebookDialog
    {
        public string chapter;

        public List<FacebookPhrase> phrases = new List<FacebookPhrase>();
        public List<FacebookChoice> choices = new List<FacebookChoice>();
    }
}