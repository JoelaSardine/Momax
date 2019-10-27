using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hpbar : MonoBehaviour
{
    public Image image;

    void Start()
    {
        StartCoroutine(HpCoroutine(0));
    }

    private IEnumerator HpCoroutine(int target)
    {
        yield return new WaitForSeconds(1.0f);

        float floatHp = 100;
        int hp = 100;

        while (hp != target)
        {
            floatHp -= Time.deltaTime * 100 / 2.0f;
            hp = Mathf.RoundToInt(floatHp);
            image.fillAmount = hp / 100f;
            yield return null;
        }
    }
}
