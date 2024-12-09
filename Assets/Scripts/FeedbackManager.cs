using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class FeedbackManager : MonoBehaviour
{
    public TMP_InputField feedbackInput;
    public TextMeshProUGUI displayText;

    void Start()
    {
        // Retrieve and display stored PlayerPrefs
        string playerId = PlayerPrefs.GetString("PlayerID", "Unknown");
        string startTime = PlayerPrefs.GetString("StartTime", "Unknown");
        float duration = PlayerPrefs.GetFloat("Duration", 0f);
        int score = PlayerPrefs.GetInt("FinalScore", 0);

        displayText.text = $"Player ID: {playerId}\n" +
                           $"Start Time: {startTime}\n" +
                           $"Duration: {duration:F2} seconds\n" +
                           $"Score: {score}";
    }

    public void SubmitFeedback()
    {
        // Retrieve data from PlayerPrefs
        string playerId = PlayerPrefs.GetString("PlayerID", "Unknown");
        string startTime = PlayerPrefs.GetString("StartTime", "Unknown");
        float duration = PlayerPrefs.GetFloat("Duration", 0f);
        int score = PlayerPrefs.GetInt("Score", 0);
        string feedback = feedbackInput.text;

        // Create a GameData object to store all the data
        GameData gameData = new GameData
        {
            PlayerID = playerId,
            StartTime = startTime,
            Duration = duration,
            Score = score,
            Feedback = feedback
        };

        // Convert data to JSON
        string jsonData = JsonUtility.ToJson(gameData, true);

        if (string.IsNullOrEmpty(jsonData))
        {
            Debug.LogError("Failed to serialize data to JSON.");
            return;
        }

        // Save the data to a file
        string filePath = Path.Combine(Application.persistentDataPath, "GameData.json");
        try
        {
            File.WriteAllText(filePath, jsonData);
            Debug.Log($"Game data saved to {filePath}");
        }
        catch (IOException e)
        {
            Debug.LogError($"Failed to save game data to file: {e.Message}");
        }

        Debug.Log("Feedback Saved: " + feedback);
    }

    [System.Serializable]
    private class GameData
    {
        public string PlayerID;
        public string StartTime;
        public float Duration;
        public int Score;
        public string Feedback;
    }
}
