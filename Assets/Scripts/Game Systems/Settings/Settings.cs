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

    private int fullscreenToggle = 3;
    private int scorePopupToggle = 4;

    public void SaveSettings()
    {
        HandleSettingsFile.WriteSaveFile(this);
    }

    public void LoadSettingsFromFile(bool apply)
    {
        List<string> settings = HandleSettingsFile.ReadSaveFile(false);
        SetAllSettingsFromLoad(settings, apply);
    }

    #region Defaults
    public void DefaultSettings()
    {
        List<string> settings = HandleSettingsFile.ReadSaveFile(true);
        SetAllSettingsFromLoad(settings, true);
    }

    public void DefaultAudioSettings()
    {
        List<string> settings = HandleSettingsFile.ReadSaveFile(true);
        SetAudioFromLoad(settings, true);
    }

    public void DefaultVideoSettings()
    {
        List<string> settings = HandleSettingsFile.ReadSaveFile(true);
        SetVideoFromLoad(settings, true);
    }

    public void DefaultGameplaySettings()
    {
        List<string> settings = HandleSettingsFile.ReadSaveFile(true);
        SetGameplayFromLoad(settings, true);
    }
    #endregion

    public void SetAllSettingsFromLoad(List<string> settings, bool apply)
    {
        SetAudioFromLoad(settings, apply);
        SetVideoFromLoad(settings, apply);
        SetGameplayFromLoad(settings, apply);
    }

    public void SetAudioFromLoad(List<string> settings, bool apply)
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

        for (int i = 0; i < fullscreenToggle; i++)
        {
            bool muted = bool.Parse(settings[i + sliders.Length]);
            toggles[i].isOn = muted;
            sliders[i].interactable = !muted;
            if (apply && muted)
            {
                string thisSlider = sliders[i].name;
                CurrentSlider(thisSlider);
                ChangeVolume(-80f);
            }
        }
    }

    public void SetVideoFromLoad(List<string> settings, bool apply)
    {
        bool toggle = bool.Parse(settings[sliders.Length + fullscreenToggle]);
        toggles[fullscreenToggle].isOn = toggle;

        if (apply)
            FullscreenToggle(toggle);


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

    public void SetGameplayFromLoad(List<string> settings, bool apply)
    {
        bool toggle = bool.Parse(settings[sliders.Length + scorePopupToggle]);
        toggles[scorePopupToggle].isOn = toggle;

        if (apply)
            ScorePopupToggle(toggle);
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

    #region Game options
    public static bool showScorePopup = true;
    public void ScorePopupToggle(bool toggle)
    {
        showScorePopup = toggle;
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

        if (!PlayerPrefs.HasKey("FirstLoadSettings"))
        {
            DefaultSettings();
            PlayerPrefs.SetString("FirstLoadSettings", "");
        }
        else
            if (!firstBootDone)
            LoadSettingsFromFile(true);

        firstBootDone = true;
    }
}
