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

            RpgManager.CurrentStory = this;
            RpgManager.Instance.gameState = RpgManager.GameState.Rpg;
            player = RpgManager.Player;
        }
    }
}