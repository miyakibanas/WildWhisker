using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;


public class PauseManager : MonoBehaviour
{
    [SerializeField] TMP_Text pauseResumeButtonText;
    [SerializeField] AudioSource musicPlayer;
    private bool isPaused = false;

    void Start()
    {
        if (pauseResumeButtonText != null) 
            pauseResumeButtonText.text = "Pause"; 
        DeselectUI();
    }

    public void TogglePauseResume()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    private void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0; 
        if (musicPlayer != null && musicPlayer.isPlaying)
        {
            musicPlayer.Pause();
        }
        if (pauseResumeButtonText != null) 
            pauseResumeButtonText.text = "Resume";
    }

    private void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1; 
        if (musicPlayer != null && !musicPlayer.isPlaying)
        {
            musicPlayer.Play();
        }
        DeselectUI();
        if (pauseResumeButtonText != null) 
            pauseResumeButtonText.text = "Pause";
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1; 
        SceneManager.LoadScene("MainMenu");
    }

    public bool IsPaused()
    {
        return isPaused;
    }

    private void DeselectUI()
    {
        EventSystem.current?.SetSelectedGameObject(null);
    }
}
