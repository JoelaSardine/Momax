using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using rpg;

public class GameManager : MonoBehaviour
{
    public RpgManager rpgManager;
    public CameraManager cameraManager;

    public static GameManager Instance;
    public static CameraManager CameraManager;
    public static RpgManager RpgManager;

    private void Awake()
    {
        if (Instance == null)
        {
            GameManager.Instance = this;
            GameManager.CameraManager = this.cameraManager;
            
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
