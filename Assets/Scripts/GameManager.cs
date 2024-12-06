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
    public int totalLevels = 3;

    [Header("Score Settings")]
    public int score = 0;  // Current score

    public LevelUIController UIController;

    private int balloonCount;


    void Start()
    {
        currentLevel = SceneManager.GetActiveScene();

        currentLives = totalLives;
        UIController.SetLives(currentLives);
        UIController.SetLevel(currentLevel.name);
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
            UIController.SetLevel(currentLevel.name);
            SceneManager.LoadScene(currentLevel.buildIndex + 1);
        }
        else
        {
            SceneManager.LoadScene("End"); // Load the End screen after completing all levels
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(currentLevel.name);
    }

    public void GameOver()
    {
        Debug.Log("Game Over! You lost all your lives.");
        SceneManager.LoadScene("End"); // Load the End screen after losing all lives
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
}
