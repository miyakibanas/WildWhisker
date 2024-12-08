using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] GameObject tutorialPanel;

    public void ShowTutorial()
    {
        if (tutorialPanel != null)
        {
            tutorialPanel.SetActive(true); 
            Time.timeScale = 0; 
        }
    }

    public void CloseTutorial()
    {
        if (tutorialPanel != null)
        {
            tutorialPanel.SetActive(false); 
            Time.timeScale = 1; 
        }
    }
}
