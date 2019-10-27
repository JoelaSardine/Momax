using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionBattle : MonoBehaviour
{
    public static TransitionBattle Instance;

    private bool init = false;

    private RectTransform moving;
    private RectTransform top;
    private RectTransform left;
    private RectTransform right;
    private RectTransform down;

    public Action onClosureFinished;
    public Action onOpeningFinished;

    [Header("Spiral effect")]
    public float stepSize = 0.1f;
    public float delay = 1.0f;

    [Header("Shutter effect")]
    public float shutterDuration = 2.0f;

    private void Awake()
    {
        Init();
        enabled = false;

        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void StartSpiralCoroutine()
    {
        Init();
        StartCoroutine(SpiralCoroutine());
    }
    public void StartShutterCoroutine() { StartCoroutine(ShutterCoroutine()); }

    private IEnumerator SpiralCoroutine()
    {
        float step = 0.0f;

        while (step < 0.5f + stepSize)
        {
            step += stepSize;
            yield return new WaitForSeconds(delay);
            top.anchorMin = new Vector2(0, 1 - step);
            yield return new WaitForSeconds(delay);
            right.anchorMin = new Vector2(1 - step, 0);
            yield return new WaitForSeconds(delay);
            down.anchorMax = new Vector2(1, step);
            yield return new WaitForSeconds(delay);
            left.anchorMax = new Vector2(step, 1);
        }

        if (onClosureFinished != null)
        {
            onClosureFinished();
        }
    }

    private IEnumerator ShutterCoroutine()
    {
        right.anchorMin = new Vector2(1, 0);
        left.anchorMax = new Vector2(0, 1);
        top.anchorMin = new Vector2(0, 0.5f);
        down.anchorMax = new Vector2(0.5f, 1);

        float step = 0.5f;

        while (step > 0)
        {
            step -= Time.deltaTime / shutterDuration / 2;
            top.anchorMin = new Vector2(0, 1 - step);
            down.anchorMax = new Vector2(1, step);
            yield return null;
        }

        if (onOpeningFinished != null)
        {
            onOpeningFinished();
        }
    }

    private void Init()
    {
        if (!init)
        {
            moving = GetRekt("Moving");
            top = GetRekt("Top");
            left = GetRekt("Left");
            right = GetRekt("Right");
            down = GetRekt("Down");
            init = true;
        }

        top.anchorMin = new Vector2(0, 1);
        right.anchorMin = new Vector2(1, 0);
        down.anchorMax = new Vector2(1, 0);
        left.anchorMax = new Vector2(0, 1);
    }

    private RectTransform GetRekt(string name)
    {
        return transform.Find(name).GetComponent<RectTransform>();
    }
}
