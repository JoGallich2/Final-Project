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
    public int CurrentLevel = 0; // Tutorial level is counted as level 0
    public int totalLevels = 4;

    [Header("Score Settings")]
    public int score = 0;  // Current score
    public Text scoreText;  // UI Text component for displaying the score

    public LevelUIController UIController;


    void Start()
    {
        currentLives = totalLives;
        UIController.SetLives(currentLives);
        UIController.SetLevel(CurrentLevel);
    }

    // Method to add points
    public void AddPoints(int points)
    {
        score += points;
        UIController.UpdateScore(score);
    }

    public void LoadNextLevel()
    {
        if (CurrentLevel < totalLevels)
        {
            CurrentLevel++;
            SceneManager.LoadScene("Level" + CurrentLevel);
        }
        else
        {
            SceneManager.LoadScene("End"); // Load the End screen after completing all levels
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene("Level" + CurrentLevel);
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
}
