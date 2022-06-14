using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContinueButtonHandler : MonoBehaviour
{
    [SerializeField] private Button contButton;

    public void SetButton()
    {
        ResetButtons();
        string[] files = HandleGameSaveFile.ReadContinueFile();
        if (files.Length > 0 && files[0] != "")
        {
            contButton.interactable = true;
        }
    }

    void ResetButtons()
    {
        contButton.interactable = false;
    }
}
