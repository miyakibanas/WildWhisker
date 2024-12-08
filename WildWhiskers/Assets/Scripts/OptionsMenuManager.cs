using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class OptionsMenuManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] TMP_Dropdown resolutionDropdown;
    [SerializeField] Slider masterVolumeSlider;
    [SerializeField] Slider sfxVolumeSlider;
    [SerializeField] Slider musicVolumeSlider;

    [Header("Audio Mixers")]
    [SerializeField] private AudioMixer audioMixer;

    private Resolution[] resolutions;

    void Start()
    {
        PopulateResolutionDropdown();
        LoadSettings();
    }

    private void PopulateResolutionDropdown()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        int currentResolutionIndex = 0;
        var options = new System.Collections.Generic.List<string>();
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = $"{resolutions[i].width} x {resolutions[i].height}";
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        if (resolutions != null && resolutionIndex < resolutions.Length)
        {
            Resolution selectedResolution = resolutions[resolutionIndex];
            Screen.SetResolution(selectedResolution.width, selectedResolution.height, Screen.fullScreen);
        }
    }

    public void SetMasterVolume(float volume)
    {
        PlayerPrefs.SetFloat("MasterVolume", volume);
        float volumeDb = volume > 0 ? Mathf.Log10(volume) * 20 : -80;
        if (audioMixer != null)
        {
            audioMixer.SetFloat("MasterVolume", volumeDb);
        }
    }

    public void SetSFXVolume(float volume)
    {
        PlayerPrefs.SetFloat("SFXVolume", volume);
        float volumeDb = volume > 0 ? Mathf.Log10(volume) * 20 : -80;
        if (audioMixer != null)
        {
            audioMixer.SetFloat("SFXVolume", volumeDb);
        }
    }

    public void SetMusicVolume(float volume)
    {
        PlayerPrefs.SetFloat("MusicVolume", volume);
        float volumeDb = volume > 0 ? Mathf.Log10(volume) * 20 : -80;
        if (audioMixer != null)
        {
            audioMixer.SetFloat("MusicVolume", volumeDb);
        }
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetInt("Resolution", resolutionDropdown.value);
        PlayerPrefs.SetFloat("MasterVolume", masterVolumeSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolumeSlider.value);
        PlayerPrefs.SetFloat("MusicVolume", musicVolumeSlider.value);
        PlayerPrefs.Save();
    }

    private void LoadSettings()
    {
        if (PlayerPrefs.HasKey("Resolution"))
        {
            resolutionDropdown.value = PlayerPrefs.GetInt("Resolution");
            resolutionDropdown.RefreshShownValue();
            SetResolution(resolutionDropdown.value);
        }

        masterVolumeSlider.value = Mathf.Clamp(PlayerPrefs.GetFloat("MasterVolume", 1f), 0f, 1f);
        SetMasterVolume(masterVolumeSlider.value);

        sfxVolumeSlider.value = Mathf.Clamp(PlayerPrefs.GetFloat("SFXVolume", 1f), 0f, 1f);
        SetSFXVolume(sfxVolumeSlider.value);

        musicVolumeSlider.value = Mathf.Clamp(PlayerPrefs.GetFloat("MusicVolume", 1f), 0f, 1f);
        SetMusicVolume(musicVolumeSlider.value);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}

