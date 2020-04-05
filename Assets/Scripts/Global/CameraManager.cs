using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class CameraManager : MonoBehaviour
{
    public CanvasGroup blackScreen;
    public Camera mainCamera;

    public float fadeInTime = 1.0f;
    public float fadeOutTime = 1.0f;

    public void FadeIn(Action callback = null) { }
    public void FadeOut(Action callback = null) { }
    public void FadeInAndOut(Action callbackIn = null, Action callbackOut = null) { }

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            ChangeCameraOutputSize(mainCamera.rect.width == 1 ? 0.5f : 1);
        }
    }

    public void ChangeCameraOutputSize(float width)
    {
        PixelPerfectCamera ppcam = mainCamera.GetComponent<PixelPerfectCamera>();
        ppcam.enabled = width == 1.0f;

        Rect rect = mainCamera.rect;
        rect.width = width;
        mainCamera.rect = rect; 
    }

    public IEnumerator FadeInCoroutine()
    {
        float step = 1.0f / fadeInTime;

        while (blackScreen.alpha < 1)
        {
            blackScreen.alpha += step * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    public IEnumerator FadeOutCoroutine()
    {
        float step = 1.0f / fadeOutTime;

        while (blackScreen.alpha > 0)
        {
            blackScreen.alpha -= step * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
}
