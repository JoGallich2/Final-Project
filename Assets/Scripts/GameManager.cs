using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Game Settings")]
    public int totalLives = 3;  // Set initial lives here
    public int currentLives;
    public Scene currentLevel;
    public int totalLevels = 4;

    [Header("Score Settings")]
    public int score = 0;  // Current score
    public float startTime;

    public LevelUIController UIController;

    private int balloonCount;


    void Start()
    {
        currentLevel = SceneManager.GetActiveScene();

        currentLives = totalLives;
        UIController.SetLives(currentLives);
        UIController.SetLevel(currentLevel.name);

        // Store the starting time of the game
        startTime = Time.time;

        // Generate a unique Player ID if not already set
        if (!PlayerPrefs.HasKey("PlayerID"))
        {
            PlayerPrefs.SetString("PlayerID", System.Guid.NewGuid().ToString());
        }
        if (currentLevel.buildIndex == 1 || currentLevel.buildIndex == 0)
        {
            score = 0;
            PlayerPrefs.SetInt("Score", 0);
        }
        else
        {
            // Retrieve score from previous levels
            if (PlayerPrefs.HasKey("Score"))
            {
                score = PlayerPrefs.GetInt("Score");
            }
            else
            {
                score = 0;
            }
        }
        UIController.UpdateScore(score);

        // Store the time of day the game started
        PlayerPrefs.SetString("StartTime", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
    }

    private void Update()
    {
        balloonCount = GameObject.FindGameObjectsWithTag("Balloon").Length;

        if (balloonCount == 0)
        {
            LoadNextLevel();
        }
    }

    // Method to add points
    public void AddPoints(int points)
    {
        score += points;
        UIController.UpdateScore(score);
    }

    public void LoadNextLevel()
    {
        currentLevel = SceneManager.GetActiveScene();
        if (currentLevel.buildIndex < totalLevels)
        {
            // Save the current score to PlayerPrefs before loading the next level
            PlayerPrefs.SetInt("Score", score);

            UIController.SetLevel(currentLevel.name);
            SceneManager.LoadScene(currentLevel.buildIndex + 1);
        }
        else
        {
            EndGame();
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(currentLevel.name);
    }

    public void GameOver()
    {
        Debug.Log("Game Over! You lost all your lives.");
        EndGame();
    }

    public void DecreaseLives()
    {
        currentLives--;
        UIController.SetLives(currentLives);

        if (currentLives <= 0)
        {
            GameOver();
        }
    }

    public void RespawnBee(GameObject bee)
    {
        StartCoroutine(RespawnDelay(bee));  // Start the coroutine for respawning with a delay
    }

    private IEnumerator RespawnDelay(GameObject bee)
    {
        // Add delay before respawn (e.g., 1 second)
        yield return new WaitForSeconds(1f);

        // Respawn the bee by setting its position to the starting position (or a safe spot away from the red zone)
        bee.transform.position = new Vector3(0, 0, 2); // Example: Reset position to origin or a safe point
    }

    public void IncreaseBalloons()
    {
        balloonCount++;
        Debug.Log(balloonCount);
    }

    public void DecreaseBalloons()
    {
        balloonCount--;
        Debug.Log(balloonCount);
    }
    private void EndGame()
    {
        // Calculate and save duration
        float duration = Time.time - startTime;
        PlayerPrefs.SetFloat("Duration", duration);

        // Save the final score and set score back to 0
        PlayerPrefs.SetInt("FinalScore", score);
        PlayerPrefs.SetInt("Score", 0);

        // Load the feedback scene or end screen
        SceneManager.LoadScene("FeedbackScene");
    }
}
