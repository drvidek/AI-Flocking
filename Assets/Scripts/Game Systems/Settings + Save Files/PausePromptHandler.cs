using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PausePromptHandler : MonoBehaviour
{
    Text myText;
    string myString = " - PAUSE";

    private void Start()
    {
        myText = GetComponent<Text>();
        UpdateText();
    }

    public void UpdateText()
    {
        myText.text = KeyBinds.keys["Pause"].ToString().ToUpper() + myString;
    }
}
