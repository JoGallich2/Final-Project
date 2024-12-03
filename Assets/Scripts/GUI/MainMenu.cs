using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Level 0");
    }

    public void OpenTutorial()
    {

    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
