using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    private static bool firstBootDone;


    #region Saving and Loading
    public Slider[] sliders;
    public Toggle[] toggles;
    public Dropdown[] dropdowns;

    public void SaveSettings()
    {
        HandleSettingsFile.WriteSaveFile(this);
    }

    public void ReadSettings(bool apply)
    {
        List<string> settings = HandleSettingsFile.ReadSaveFile(false);
        SetSettings(settings, apply);
    }

    public void DefaultSettings()
    {
        List<string> settings = HandleSettingsFile.ReadSaveFile(true);
        SetSettings(settings,true);
    }

    public void SetSettings(List<string> settings, bool apply)
    {
        for (int i = 0; i < sliders.Length; i++)
        {
            sliders[i].value = float.Parse(settings[i]);
            if (apply)
            {
                string thisSlider = sliders[i].name;
                CurrentSlider(thisSlider);
                ChangeVolume(sliders[i].value);
            }
        }

        for (int i = 0; i < toggles.Length; i++)
        {
            bool muted = bool.Parse(settings[i + sliders.Length]);
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
                    FullscreenToggle(muted);
            }

        }

        for (int i = 0; i < dropdowns.Length; i++)
        {
            dropdowns[i].value = (settings[i + sliders.Length + toggles.Length] == "-1") ? resolutions.Count : int.Parse(settings[i + sliders.Length + toggles.Length]);
            if (apply)
            {
                if (i == 0)
                {
                    Quality(dropdowns[i].value);
                }
                else
                {
                    SetResolution(dropdowns[i].value);
                }
            }
            dropdowns[i].RefreshShownValue();
        }
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
    }
    #endregion

    #region Quality
    public void Quality(int qualIndex)
    {
        QualitySettings.SetQualityLevel(qualIndex);
    }
    #endregion

    #region Resolution
    public List<Resolution> resolutions = new List<Resolution>();
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
    private void Start()
    {
        #region Resolution
        resolutions = new List<Resolution>(Screen.resolutions);
        resDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Count; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;

            Vector2 _currentRes = new Vector2(resolutions[i].width, resolutions[i].height);
            Vector2 _targetRes = new Vector2(16, 9);

            if (!options.Contains(option) && (_currentRes.normalized == _targetRes.normalized))
            {
                options.Add(option);
                if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
                {
                    currentResolutionIndex = i;
                }
            }
            else
            {
                resolutions.RemoveAt(i);
                i--;
            }
        }
        resDropdown.AddOptions(options);
        resDropdown.value = currentResolutionIndex;
        resDropdown.RefreshShownValue();
        #endregion
        ReadSettings(!firstBootDone);

        firstBootDone = true;
    }
}
