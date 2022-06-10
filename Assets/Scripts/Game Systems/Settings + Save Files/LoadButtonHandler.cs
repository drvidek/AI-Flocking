using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadButtonHandler : MonoBehaviour
{
    [SerializeField] private Button[] loadButtons;

    public void SetButton()
    {
        ResetButtons();
        List<string> files = HandleGameSaveFile.ListSaveFiles();
        for (int i = 0; i < loadButtons.Length; i++)
        {
            if (files[i] != "")
            {
                loadButtons[i].interactable = true;
            }
        }
    }

    void ResetButtons()
    {
        for (int i = 0; i < loadButtons.Length; i++)
        {
            loadButtons[i].interactable = false;
        }
    }
}
