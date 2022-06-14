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

        //for (int i = 0; i < settingsClass.sliders.Length; i++)
        //{
        //    settingsClass.sliders[i].value = float.Parse(saveRead.ReadLine());
        //    if (apply)
        //    {
        //        string thisSlider = settingsClass.sliders[i].name;
        //        settingsClass.CurrentSlider(thisSlider);
        //        settingsClass.ChangeVolume(settingsClass.sliders[i].value);
        //    }
        //}

        //for (int i = 0; i < settingsClass.toggles.Length; i++)
        //{
        //    bool muted = bool.Parse(saveRead.ReadLine());
        //    settingsClass.toggles[i].isOn = muted;
        //    if (i < settingsClass.sliders.Length)
        //    {
        //        settingsClass.sliders[i].interactable = !muted;
        //        if (apply && muted)
        //        {
        //            string thisSlider = settingsClass.sliders[i].name;
        //            settingsClass.CurrentSlider(thisSlider);
        //            settingsClass.ChangeVolume(-80f);
        //        }
        //    }
        //    else
        //    {
        //        if (apply)
        //            settingsClass.FullscreenToggle(muted);
        //    }

        //}

        //for (int i = 0; i < settingsClass.dropdowns.Length; i++)
        //{
        //    settingsClass.dropdowns[i].value = int.Parse(saveRead.ReadLine());
        //    if (apply)
        //    {
        //        if (i == 0)
        //        {
        //            settingsClass.Quality(settingsClass.dropdowns[i].value);
        //        }
        //        else
        //        {
        //            settingsClass.SetResolution(settingsClass.dropdowns[i].value);
        //        }
        //    }
        //    settingsClass.dropdowns[i].RefreshShownValue();
        //}


    }
}
