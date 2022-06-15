using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class HandleSettingsFile
{
    static string path = Path.Combine(Application.streamingAssetsPath, "Options/Settings.txt");
    static string pathDefault = Path.Combine(Application.streamingAssetsPath, "Options/SettingsDefault.txt");

    public static void WriteSaveFile(Settings settings)
    {
        StreamWriter saveWrite = new StreamWriter(path, false);

        for (int i = 0; i < settings.sliders.Length; i++)
        {
            saveWrite.WriteLine(settings.sliders[i].value);
        }

        for (int i = 0; i < settings.toggles.Length; i++)
        {
            saveWrite.WriteLine(settings.toggles[i].isOn);
        }

        for (int i = 0; i < settings.dropdowns.Length; i++)
        {
            saveWrite.WriteLine(settings.dropdowns[i].value);
        }

        saveWrite.Close();
    }

    public static List<string> ReadSaveFile(bool defaulting)
    {
        StreamReader saveRead = new StreamReader(defaulting ? pathDefault : path);

        List<string> settings = new List<string>();
        string newLine = "";
        while ((newLine = saveRead.ReadLine()) != null && newLine != "")
        {
            Debug.Log(newLine);
            settings.Add(newLine);
        }
        saveRead.Close();

        return settings;

    }
}
