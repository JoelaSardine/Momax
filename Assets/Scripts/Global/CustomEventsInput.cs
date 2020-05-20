using UnityEngine;
using UnityEngine.EventSystems;

public class CustomEventsInput : MonoBehaviour
{
    [Range(0, 1)]
    public float deadZone = 0.5f;

    public bool buttonUp = false;
    public bool buttonLeft = false;
    public bool buttonRight = false;
    public bool buttonDown = false;

    private bool isUp = false;
    private bool isLeft = false;
    private bool isRight = false;
    private bool isDown = false;

    void Update()
    {
        buttonUp = false;
        buttonLeft = false;
        buttonRight = false;
        buttonDown = false;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (isUp && v < deadZone)
        {
            isUp = false;
        }
        else if (!isUp && v > deadZone)
        {
            isUp = true;
            buttonUp = true;
        }

        if (isDown && v > -deadZone)
        {
            isDown = false;
        }
        else if (!isDown && v <-deadZone)
        {
            isDown = true;
            buttonDown = true;
        }

        if (isLeft && h > -deadZone)
        {
            isLeft = false;
        }
        else if (!isLeft && h < -deadZone)
        {
            isLeft = true;
            buttonLeft = true;
        }

        if (isRight && h < deadZone)
        {
            isRight = false;
        }
        else if (!isRight && h > deadZone)
        {
            isRight = true;
            buttonRight = true;
        }
    }
}