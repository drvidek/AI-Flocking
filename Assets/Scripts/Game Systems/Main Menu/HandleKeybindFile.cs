using System;
using System.IO;
using UnityEngine;
using UnityEditor;

public static class HandleKeybindFile
{
    static string path = Path.Combine(Application.streamingAssetsPath, "Options/Keybinds.txt");

        [MenuItem("My Tools/Save/Write File/Keybinds")]
    public static void WriteSaveFile()
    {
        StreamWriter saveWrite = new StreamWriter(path, false);

        foreach (var keyEntry in KeyBinds.keys)
        {
            saveWrite.WriteLine(keyEntry.Key + ":" + keyEntry.Value.ToString());
        }

        saveWrite.Close();
    }

        [MenuItem("My Tools/Save/Read File/Keybinds")]
    public static void ReadSaveFile()
    {
        StreamReader saveRead = new StreamReader(path);
        string line;

        while ((line = saveRead.ReadLine()) != null)
        {
            string[] parts = line.Split(':');
            //if we have keys and we are just updating them
            if (KeyBinds.keys.Count > 0)
            {
                KeyBinds.keys[parts[0]] = (KeyCode)System.Enum.Parse(typeof(KeyCode), parts[1]);
            }
            else
            {
                KeyBinds.keys.Add(parts[0], (KeyCode)System.Enum.Parse(typeof(KeyCode), parts[1]));
            }

        }
        saveRead.Close();

    }
}
