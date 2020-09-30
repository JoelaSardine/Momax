using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessCatsEasterEgg : MonoBehaviour
{
    public bool switchOn = false;

    public GameObject target;

    private void Awake()
    {
        target.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        rpg.PlayerManager pm = collision.GetComponent<rpg.PlayerManager>();
        if (pm)
        {
            target.SetActive(switchOn);
        }
    }
}
