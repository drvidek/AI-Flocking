using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class HandleSettingsFile
{
    static string path = Path.Combine(Application.streamingAssetsPath, "Options/Settings.txt");

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

    public static void ReadSaveFile(Settings settings, bool apply)
    {
        StreamReader saveRead = new StreamReader(path);

        for (int i = 0; i < settings.sliders.Length; i++)
        {
            settings.sliders[i].value = float.Parse(saveRead.ReadLine());
            if (apply)
            {
                string thisSlider = settings.sliders[i].name;
                settings.CurrentSlider(thisSlider);
                settings.ChangeVolume(settings.sliders[i].value);
            }
        }

        for (int i = 0; i < settings.toggles.Length; i++)
        {
            bool muted = bool.Parse(saveRead.ReadLine());
            settings.toggles[i].isOn = muted;
            if (i < settings.sliders.Length)
            {
                settings.sliders[i].interactable = !muted;
                if (apply && muted)
                {
                    string thisSlider = settings.sliders[i].name;
                    settings.CurrentSlider(thisSlider);
                    settings.ChangeVolume(-80f);
                }
            }
            else
            {
                if (apply)
                    settings.FullscreenToggle(muted);
            }

        }

        for (int i = 0; i < settings.dropdowns.Length; i++)
        {
            settings.dropdowns[i].value = int.Parse(saveRead.ReadLine());
            if (apply)
            {
                if (i == 0)
                {
                    settings.Quality(settings.dropdowns[i].value);
                }
                else
                {
                    settings.SetResolution(settings.dropdowns[i].value);
                }
            }
            settings.dropdowns[i].RefreshShownValue();
        }

        saveRead.Close();

    }
}
