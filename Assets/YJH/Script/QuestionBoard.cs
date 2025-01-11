using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct SurveyQuestion
{
    public string QuestionText; // The text of the survey question
    public List<string> Options; // List of options (e.g., 5 options)
}
public class QuestionBoard : MonoBehaviour
{
    [SerializeField] private List<SurveyQuestion> surveyQuestions; // List of survey questions
    [SerializeField] private GameObject[] optionButtons; // Array of 3D button objects

    private int currentQuestionIndex = 0;
    private Dictionary<int, int> playerResponses = new Dictionary<int, int>(); // Stores question index and player's selected option

    void Start()
    {
        if (surveyQuestions.Count == 0)
        {
            Debug.LogError("No survey questions available.");
            return;
        }

        DisplayQuestion(currentQuestionIndex);
    }

    /// <summary>
    /// Displays the current survey question and options on the 3D buttons.
    /// </summary>
    /// <param name="questionIndex">Index of the question to display.</param>
    private void DisplayQuestion(int questionIndex)
    {
        if (questionIndex < 0 || questionIndex >= surveyQuestions.Count)
        {
            Debug.LogError("Question index out of range.");
            return;
        }

        SurveyQuestion question = surveyQuestions[questionIndex];

        // Display question text (Assume you have a 3D TextMeshPro or other UI setup)
        Debug.Log("Question: " + question.QuestionText);

        // Update option buttons
        for (int i = 0; i < optionButtons.Length; i++)
        {
            if (i < question.Options.Count)
            {
                optionButtons[i].SetActive(true);
                var button = optionButtons[i].GetComponent<QuestionOptionButton>();
                if (button != null)
                {
                    button.SetOptionText(question.Options[i]);
                    button.SetOptionIndex(i);
                    button.OnOptionSelected = OnOptionSelected;
                }
            }
            else
            {
                optionButtons[i].SetActive(false);
            }
        }
    }

    /// <summary>
    /// Called when a player selects an option.
    /// </summary>
    /// <param name="selectedOptionIndex">Index of the selected option.</param>
    private void OnOptionSelected(int selectedOptionIndex)
    {
        Debug.Log($"Player selected option {selectedOptionIndex} for question {currentQuestionIndex}.");

        // Save the player's response
        playerResponses[currentQuestionIndex] = selectedOptionIndex;

        // Proceed to the next question or finish
        if (currentQuestionIndex + 1 < surveyQuestions.Count)
        {
            currentQuestionIndex++;
            DisplayQuestion(currentQuestionIndex);
        }
        else
        {
            FinishSurvey();
        }
    }

    /// <summary>
    /// Finalizes the survey and processes responses.
    /// </summary>
    private void FinishSurvey()
    {
        Debug.Log("Survey finished. Processing responses...");

        foreach (var response in playerResponses)
        {
            int questionIndex = response.Key;
            int selectedOption = response.Value;

            Debug.Log($"Question {questionIndex}: Player selected option {selectedOption}");
        }

        // Additional logic for processing survey results, e.g., calculating scores.
    }
}

