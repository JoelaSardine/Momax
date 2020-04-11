using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rpg
{
    public class CaveStory : GameStory
    {
        protected override IEnumerator Start()
        {
            yield return StartCoroutine(base.Start());
        }
    }
}