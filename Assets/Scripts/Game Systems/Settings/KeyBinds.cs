using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyBinds : MonoBehaviour
{
    [SerializeField]
    public static Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>();
    [System.Serializable]
    public struct KeyUISetup
    {
        public string keyName;
        public Text keyDisplayText;
        public string defaultKey;
        public GameObject buttonObject;
    }

    public KeyUISetup[] baseSetup;
    public GameObject currentKeyButton;
    public Color32 currentKeyCol;
    public Color32 changedKeyCol = new Color32(39, 171, 249, 255);
    public Color32 selectedKeyCol = new Color32(239, 116, 36, 255);
    public Color32 cWhite = new Color32(255, 255, 255, 255);

    // Start is called before the first frame update
    void Start()
    {
        HandleKeybindFile.ReadSaveFile();
        if (!PlayerPrefs.HasKey("FirstLoadKeys") || keys.Count < baseSetup.Length)
        {
            DefaultKeyBinds();
        }
        else
        {
            HandleKeybindFile.WriteSaveFile();
        }
        for (int i = 0; i < baseSetup.Length; i++)
        {
            baseSetup[i].keyDisplayText.text = keys[baseSetup[i].keyName].ToString();

        }
    }

    public void DefaultKeyBinds()
    {
        keys.Clear();
        for (int i = 0; i < baseSetup.Length; i++)
        {
            //add key according to the saved string
            keys.Add(baseSetup[i].keyName, (KeyCode)System.Enum.Parse(typeof(KeyCode), baseSetup[i].defaultKey));
            baseSetup[i].keyDisplayText.text = baseSetup[i].defaultKey;
            //change the colour of the button to blue
            baseSetup[i].buttonObject.GetComponent<Image>().color = cWhite;
        }
        HandleKeybindFile.WriteSaveFile();
        PlayerPrefs.SetString("FirstLoadKeys", "");
    }

    public void SaveKeys()
    {
        HandleKeybindFile.WriteSaveFile();
    }

    public void ChangeKey(GameObject clickedKey)
    {
        if (currentKeyButton != null)
        {
            //restore the colour of the button
            currentKeyButton.GetComponent<Image>().color = currentKeyCol;
            //forget the object we were editing
            currentKeyButton = null;
        }

        currentKeyButton = clickedKey;
        //if we have a key selected
        if (clickedKey != null)
        {
            //store the old color of the new button
            currentKeyCol = clickedKey.GetComponent<Image>().color;
            //change the colour of the button to the "select a key" colour
            clickedKey.GetComponent<Image>().color = selectedKeyCol;
        }
    }

    private void OnGUI()    ///run events such as key press
    {
        //temp reference to the string value of our keycode
        string newKey = "";

        //temp reference to a current event
        Event e = Event.current;
        if (currentKeyButton != null)
        {
            //if the event is a keypress
            if (e.isKey)
            {
                //our temp key reference is the event key that was pressed
                newKey = e.keyCode.ToString();

            }
            //issue in Unity getting left and right shift keys??? WHAT
            //the following part fixes this issue
            if (Input.GetKey(KeyCode.LeftShift))
            {
                newKey = "LeftShift";
            }
            if (Input.GetKey(KeyCode.RightShift))
            {
                newKey = "RightShift";
            }
            //if (e.isMouse)
            //{
            //    newKey = e.button.ToString();
            //}

            //if we have pressed a new key
            if (newKey != "")
            {
                int index = -1;
                for (int i = 0; i < baseSetup.Length; i++)
                {
                    if (baseSetup[i].buttonObject == currentKeyButton)
                        index = i;
                    string keyString = keys[baseSetup[i].buttonObject.name].ToString();

                    if (keyString == newKey && baseSetup[i].buttonObject != currentKeyButton)
                    {
                        //change the key value in the dictionary
                        keys[baseSetup[i].buttonObject.name] = keys[currentKeyButton.name];
                        //change display text to match the new key
                        baseSetup[i].buttonObject.GetComponentInChildren<Text>().text = currentKeyButton.GetComponentInChildren<Text>().text;
                        //change the colour of the button to blue
                        baseSetup[i].buttonObject.GetComponent<Image>().color = (keys[currentKeyButton.name] == (KeyCode)System.Enum.Parse(typeof(KeyCode), baseSetup[i].defaultKey)) ? cWhite : changedKeyCol;
                    }

                }

                //change the key value in the dictionary
                keys[currentKeyButton.name] = (KeyCode)System.Enum.Parse(typeof(KeyCode), newKey);
                //change display text to match the new key
                currentKeyButton.GetComponentInChildren<Text>().text = newKey;
                //change the colour of the button to blue
                currentKeyButton.GetComponent<Image>().color = (keys[currentKeyButton.name] == (KeyCode)System.Enum.Parse(typeof(KeyCode), baseSetup[index].defaultKey)) ? cWhite : changedKeyCol;
                //forget the object we were editing
                currentKeyButton = null;
            }
        }
    }

}
