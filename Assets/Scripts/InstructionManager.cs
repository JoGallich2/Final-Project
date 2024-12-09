using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InstructionsManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_Text instructionText; // Reference to the UI Text component for displaying instructions
    public Button nextButton;    // Reference to the "Next" button
    public GameObject textbox;

    private int currentStep = 0; // Counter for the current instruction step
    private string[] instructions; // Array to hold the instruction steps

    void Start()
    {
        // Initialize the instructions array with the necessary steps
        instructions = new string[]
        {
            "Welcome to the Bee Balloon: Brain Game!",
            "Move the bee horizontally and vertically using your mouse.",
            "Your goal is to pop all the balloons while avoiding obstacles.",
            "Red zones and bombs are dangerous! If you hit them, you lose a life and restart from the initial position.",
            "To complete a level, you must pop all the balloons.",
            "Good luck! Click 'Start' when you're ready to play."
        };

        // Display the first instruction
        UpdateInstruction();

        // Add a listener to the Next button
        nextButton.onClick.AddListener(OnNextClicked);
    }

    void OnNextClicked()
    {
        // Increment the step counter
        currentStep++;

        // If we've reached the end of the instructions, disable the button
        if (currentStep >= instructions.Length)
        {
            nextButton.gameObject.SetActive(false);
            textbox.SetActive(false);
            return;
        }

        // Update the displayed instruction
        UpdateInstruction();
    }

    void UpdateInstruction()
    {
        // Display the current step in the instruction text
        instructionText.text = instructions[currentStep];
    }
}
