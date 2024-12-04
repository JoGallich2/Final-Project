using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Runtime.CompilerServices;

public class LevelUIController : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text levelText;
    public GameObject beeImagePreFab;
    public Transform beeContainer;

    private List<GameObject> beeImages = new List<GameObject>();
    private int score;
    private int currentLevel;
    private bool isPaused = false;

    public void SetLives(int lives)
    {
        foreach (var bee in beeImages)
        {
            Destroy(bee);
        }
        beeImages.Clear();

        for (int i=0; i < lives; i++)
        {
            GameObject bee = Instantiate(beeImagePreFab, beeContainer);
            beeImages.Add(bee);
        }
    }

    public void UpdateScore(int newScore)
    {
        score = newScore;
        scoreText.text = $"Score: {score}";
    }

    public void SetLevel(int level)
    {
        currentLevel = level;
        levelText.text = $"Level {currentLevel}";
    }

    public void PauseGame()
    {
        if (!isPaused)
        {
            Time.timeScale = 0;
            isPaused = true;
        }
        else
        {
            Time.timeScale = 1;
            isPaused = false;
        }
    }

}
