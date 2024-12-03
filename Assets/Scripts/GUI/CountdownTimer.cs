using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountdownTimer : MonoBehaviour
{
    public TMP_Text timerText;
    public float startTime = 240f; // 4min

    private float timeRemaining;
    private bool isRunning = false;


    void Start()
    {
        timeRemaining = startTime;
        isRunning = true;
    }

    void Update()
    {
        if (isRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                UpdateTimerDisplay(timeRemaining);
            }
            else
            {
                timeRemaining = 0;
                isRunning=false;
                TimerEnded();
            }
        }
    }

    void UpdateTimerDisplay( float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);

        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    void TimerEnded()
    {
        Debug.Log("Timer has ended!");
        // add logic here for timer ending
    }
}
