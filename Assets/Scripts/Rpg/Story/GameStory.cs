using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rpg
{
    public class GameStory : MonoBehaviour
    {
        protected PlayerManager player;

        protected virtual IEnumerator Start()
        {
            if (!RpgManager.Instance)
            {
                yield return new WaitForEndOfFrame();
            }

            RpgManager.currentStory = this;
            player = RpgManager.Player;
        }
    }
}