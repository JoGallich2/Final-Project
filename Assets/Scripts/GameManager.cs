using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [Header("Game Settings")]
    public int totalLives = 3;  // Set initial lives here
    public int currentLives;
    public int CurrentLevel = 0; //Tuturial level is being counted as level 0
    public int totalLevels = 4;

    void Start()
    {
        currentLives = totalLives;
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
            SceneManager.LoadScene("EndGame"); // Load the End screen after completing all levels
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene("level" + CurrentLevel);
    }

    public void GameOver()
    {
        Debug.Log("Game Over! You lost all your lives.");
        SceneManager.LoadScene("EndGame"); // Load the End screen after completing all levels
    }

    public void DecreaseLives()
    {
        currentLives--;
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
