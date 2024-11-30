using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int CurrentLevel = 0; //Tuturial level is being counted as level 0
    public int totalLevels = 4;

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

}
