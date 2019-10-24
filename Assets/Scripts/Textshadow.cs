using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class Textshadow : MonoBehaviour
{
    public string text = "text";
    public int size = 20;

    public Text realText;
    private Text myText;

    public void SetText(string text)
    {
        myText.text = text;
        if (realText)
        {
            realText.text = text;
        }
    }
    
    private void OnValidate()
    {
        myText = GetComponent<Text>();
        myText.text = text;
        myText.fontSize = size;
        if (realText)
        {
            realText.text = text;
            realText.fontSize = size;
        }
    }
}
