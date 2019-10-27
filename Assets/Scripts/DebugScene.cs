using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugScene : MonoBehaviour
{
    public void StartBattleScene()
    {
        TransitionBattle trbattle = TransitionBattle.Instance;
        trbattle.onClosureFinished = delegate () { SceneManager.LoadScene("PokemonBattle"); };
        trbattle.StartSpiralCoroutine();
    }
}
