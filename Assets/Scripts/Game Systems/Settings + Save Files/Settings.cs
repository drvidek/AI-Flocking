using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.IO;

public class Settings : MonoBehaviour
{
    private static bool firstBootDone;


    #region Saving and Loading
    static string path = Path.Combine(Application.streamingAssetsPath, "Options/Settings.txt");

    public Slider[] sliders;
    public Toggle[] toggles;
    public Dropdown[] dropdowns;

    public void SaveSettings()
    {
        HandleSettingsFile.WriteSaveFile(this);
    }

    public void ReadSettings(bool apply)
    {
        HandleSettingsFile.ReadSaveFile(this, apply);
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

        //read the settings file, apply the settings to the screen, and apply the settings to the game on first boot
        ReadSettings(!firstBootDone);

        firstBootDone = true;
    }
}
