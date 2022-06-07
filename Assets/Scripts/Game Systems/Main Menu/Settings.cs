using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.IO;

public class Settings : MonoBehaviour
{
    private static bool firstBootDone;

    private void Start()
    {
        #region Resolution
        resolutions = Screen.resolutions;
        resDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        resDropdown.AddOptions(options);
        resDropdown.value = currentResolutionIndex;
        resDropdown.RefreshShownValue();
        #endregion

        //read the settings file, apply the settings to the screen, and apply the settings to the game on first boot
        ReadSettings(!firstBootDone);

        firstBootDone = true;
    }

    #region Saving and Loading
    static string path = Path.Combine(Application.streamingAssetsPath, "Options/Settings.txt");

    public Slider[] sliders;
    public Toggle[] toggles;
    public Dropdown[] dropdowns;

    public void SaveSettings()
    {
        StreamWriter saveWrite = new StreamWriter(path, false);

        for (int i = 0; i < sliders.Length; i++)
        {
            saveWrite.WriteLine(sliders[i].value);
        }

        for (int i = 0; i < toggles.Length; i++)
        {
            saveWrite.WriteLine(toggles[i].isOn);
        }

        for (int i = 0; i < dropdowns.Length; i++)
        {
            saveWrite.WriteLine(dropdowns[i].value);
        }

        saveWrite.Close();
    }

    public void ReadSettings(bool apply)
    {
        StreamReader saveRead = new StreamReader(path);

        for (int i = 0; i < sliders.Length; i++)
        {
            sliders[i].value = float.Parse(saveRead.ReadLine());
            if (apply)
            {
                string thisSlider = sliders[i].name;
                CurrentSlider(thisSlider);
                ChangeVolume(sliders[i].value);
            }
        }

        for (int i = 0; i < toggles.Length; i++)
        {
            bool muted = bool.Parse(saveRead.ReadLine());
            toggles[i].isOn = muted;
            if (i < sliders.Length)
            {
                sliders[i].interactable = !muted;
                if (apply && muted)
                {
                    string thisSlider = sliders[i].name;
                    CurrentSlider(thisSlider);
                    ChangeVolume(-80f);
                }
            }
            else
            {
                if (apply)
                Screen.fullScreen = muted;
            }

        }

        for (int i = 0; i < dropdowns.Length; i++)
        {
            dropdowns[i].value = int.Parse(saveRead.ReadLine());
            if (apply)
            {
                if (i == 0)
                    Quality(dropdowns[i].value);
                else
                    SetResolution(dropdowns[i].value);
            }
        }

        saveRead.Close();
    }
    #endregion


    #region Audio
    public AudioMixer masterAudio;
    public string currentSlider;
    public Slider tempSlider;

    public void GetSlider(Slider slider)
    {
        tempSlider = slider;
    }

    public void MuteToggle(bool isMuted)
    {
        if (isMuted)
        {
            masterAudio.SetFloat(currentSlider, -80);
            tempSlider.interactable = false;
        }
        else
        {
            masterAudio.SetFloat(currentSlider, tempSlider.value);
            tempSlider.interactable = true;
        }

        float newVol;
        masterAudio.GetFloat(currentSlider, out newVol);
        Debug.Log(currentSlider + " mute toggle to" + newVol);
    }

    public void CurrentSlider(string sliderName)
    {
        currentSlider = sliderName;
    }

    public void ChangeVolume(float volume)
    {
        masterAudio.SetFloat(currentSlider, volume);
        float newVol;
        masterAudio.GetFloat(currentSlider, out newVol);
        Debug.Log(currentSlider + " set to " + newVol);
    }
    #endregion

    #region Quality
    public void Quality(int qualIndex)
    {
        QualitySettings.SetQualityLevel(qualIndex);
    }
    #endregion

    #region Resolution
    public Resolution[] resolutions;
    public Dropdown resDropdown;

    public void FullscreenToggle(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }



    public void SetResolution(int index)
    {
        Resolution res = resolutions[index];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }

    #endregion
}
