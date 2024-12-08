using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float hopHeight = 0.5f;
    [SerializeField] float hopDuration = 0.2f;
    [SerializeField] float gridSize = 1f;
    private Vector3 targetPosition;
    private bool isHopping = false;
    private bool isGameOver = false;

    [SerializeField] GameObject gameOverPanel;
    [SerializeField] AudioSource musicPlayer;
    [SerializeField] AudioSource gameOverSound;
    [SerializeField] AudioSource hopSound;
    [SerializeField] TMP_Text stepsText;
    [SerializeField] PauseManager pauseManager;
    private int stepsTaken = 0;

    private Transform currentLog = null; 
    void Start()
    {
        targetPosition = transform.position;
        gameOverPanel.SetActive(false);
        UpdateStepsText();
    }

    void Update()
    {
        if (!isGameOver)
        {
            HandleInput();
            MovePlayerWithLog();
            ClampPlayerPosition();
        }
    }

    void HandleInput()
    {
        if (!isHopping && (pauseManager == null || !pauseManager.IsPaused()))
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                targetPosition += new Vector3(gridSize, 0, 0);
                StartCoroutine(HopAnimation(targetPosition));
                IncrementSteps();
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                targetPosition += new Vector3(-gridSize, gridSize, 0);
                StartCoroutine(HopAnimation(targetPosition));
                IncrementSteps();
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                targetPosition += new Vector3(gridSize, -gridSize, 0);
                StartCoroutine(HopAnimation(targetPosition));
                IncrementSteps();
            }
        }
    }

    private void ClampPlayerPosition()
    {
        Camera cam = Camera.main;
        if (cam == null) return;

        Vector3 bottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
        Vector3 topRight = cam.ViewportToWorldPoint(new Vector3(1, 1, cam.nearClipPlane));

        float clampedX = Mathf.Clamp(transform.position.x, bottomLeft.x, topRight.x);
        float clampedY = Mathf.Clamp(transform.position.y, bottomLeft.y, topRight.y);

        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }

    void MovePlayerWithLog()
    {
        if (currentLog != null)
        {
            Vector3 logMovement = currentLog.position - transform.position;
            transform.position += logMovement;

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S))
            {
                currentLog = null; 
            }
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * moveSpeed);
        }
    }

    public void ResetPlayer(Vector3 newStartPosition)
    {
        transform.position = newStartPosition;
        targetPosition = newStartPosition;
    }

    private IEnumerator HopAnimation(Vector3 finalTargetPosition)
    {
        isHopping = true;

        if (hopSound != null)
        {
            hopSound.Play(); 
        }

        Vector3 startPosition = transform.position;
        Vector3 peakPosition = new Vector3(
            (startPosition.x + finalTargetPosition.x) / 2,
            (startPosition.y + finalTargetPosition.y) / 2 + hopHeight,
            startPosition.z
        );

        float elapsedTime = 0f;
        while (elapsedTime < hopDuration / 2f)
        {
            transform.position = Vector3.Lerp(startPosition, peakPosition, elapsedTime / (hopDuration / 2f));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0f;
        while (elapsedTime < hopDuration / 2f)
        {
            transform.position = Vector3.Lerp(peakPosition, finalTargetPosition, elapsedTime / (hopDuration / 2f));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = finalTargetPosition;
        isHopping = false;
    }

    private void IncrementSteps()
    {
        stepsTaken++;
        UpdateStepsText();
    }

    private void UpdateStepsText()
    {
        if (stepsText != null)
        {
            stepsText.text = "Steps: " + stepsTaken;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Log"))
        {
            Debug.Log("Player is on a log.");
            currentLog = other.transform; 
        }
        else if (other.CompareTag("River") && currentLog == null)
        {
            Debug.Log("Game Over: Player fell into the river!");
            StartCoroutine(FallIntoRiver());
        }
        else if (other.CompareTag("Animal"))
        {
            Debug.Log("Game Over: Player collided with an animal!");
            GameOver();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Log") && currentLog == other.transform)
        {
            Debug.Log("Player left the log.");
            currentLog = null; 
        }
    }

    private IEnumerator FallIntoRiver()
    {
        isGameOver = true;
        float sinkDuration = 1f;
        Vector3 startPosition = transform.position;
        Vector3 endPosition = startPosition - new Vector3(0, 1f, 0); // Sink downwards

        float elapsedTime = 0f;
        while (elapsedTime < sinkDuration)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / sinkDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        GameOver();
    }

    private void GameOver()
    {
        isGameOver = true;
        gameOverPanel.SetActive(true);
        Time.timeScale = 0;
        musicPlayer.Stop();
        gameOverSound.Play();
    }

    public void ContinueGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
}
