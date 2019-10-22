using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public GameObject interactableState;

    private void Awake()
    {
        ExitInteraction();
    }

    public void EnterInteraction()
    {
        interactableState.SetActive(true);
    }

    public void ExitInteraction()
    {
        interactableState.SetActive(false);
    }
}
