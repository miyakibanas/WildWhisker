using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] AudioSource musicPlayer;

    void Start()
    {
        ApplyVolumeSettings();
        ApplyResolutionSettings();
    }

    private void ApplyVolumeSettings()
    {
        float masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        AudioListener.volume = masterVolume;

        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        if (musicPlayer != null) musicPlayer.volume = musicVolume;

        //float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        //if (sfxPlayer != null) sfxPlayer.volume = sfxVolume;
    }

    private void ApplyResolutionSettings()
    {
        if (PlayerPrefs.HasKey("Resolution"))
        {
            int resolutionIndex = PlayerPrefs.GetInt("Resolution");
            Resolution[] resolutions = Screen.resolutions;

            if (resolutionIndex >= 0 && resolutionIndex < resolutions.Length)
            {
                Resolution selectedResolution = resolutions[resolutionIndex];
                Screen.SetResolution(selectedResolution.width, selectedResolution.height, Screen.fullScreen);
            }
        }
    }
}
