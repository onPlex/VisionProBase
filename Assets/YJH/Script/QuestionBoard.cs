using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace YJH
{
    [System.Serializable]
    public struct SurveyQuestion
    {
        public string QuestionText; // The text of the survey question
        public List<string> Options; // List of options (e.g., 5 options)
        public bool TriggerSpecialEvent; // Flag to trigger a special event after this question
        public List<SpecialEvent> SpecialEvents; // List of special events for this question
    }

    [System.Serializable]
    public class SpecialEvent
    {
        public string EventName; // Name of the event (for identification/debugging)
        public MonoBehaviour EventScript; // Script containing the special event logic
        public string MethodName; // Name of the method to invoke
    }

    public class QuestionBoard : MonoBehaviour
    {
        [SerializeField] private List<SurveyQuestion> surveyQuestions; // List of survey questions
        [SerializeField] private GameObject[] optionButtons; // Array of 3D button objects
        [SerializeField] private TMP_Text questionText; // UI element for question text

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

            // Display question text
            questionText.text = question.QuestionText;

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
            // 우선 currentQuestionIndex도 유효한지 확인
            if (currentQuestionIndex < 0 || currentQuestionIndex >= surveyQuestions.Count)
            {
                Debug.LogError($"Invalid currentQuestionIndex: {currentQuestionIndex}");
                return;
            }

            // 여기서 'Options' 접근 전, selectedOptionIndex 범위 체크
            if (selectedOptionIndex < 0 || selectedOptionIndex >= surveyQuestions[currentQuestionIndex].Options.Count)
            {
                Debug.LogError($"Invalid option index {selectedOptionIndex} for question {currentQuestionIndex}. " +
                               $"Options.Count={surveyQuestions[currentQuestionIndex].Options.Count}");
                return;
            }

            Debug.Log($"Player selected option {selectedOptionIndex} for question {currentQuestionIndex}.");

            // Save the player's response
            playerResponses[currentQuestionIndex] = selectedOptionIndex;

            // Check if a special event should be triggered
            if (surveyQuestions[currentQuestionIndex].TriggerSpecialEvent)
            {
                StartCoroutine(HandleSpecialEvents(surveyQuestions[currentQuestionIndex].SpecialEvents));
            }
            else
            {
                ProceedToNextQuestion();
            }
        }

        /// <summary>
        /// Handles special events and proceeds to the next question after completion.
        /// </summary>
        private IEnumerator HandleSpecialEvents(List<SpecialEvent> specialEvents)
        {
            foreach (var specialEvent in specialEvents)
            {
                if (specialEvent.EventScript != null && !string.IsNullOrEmpty(specialEvent.MethodName))
                {
                    var method = specialEvent.EventScript.GetType().GetMethod(specialEvent.MethodName);
                    if (method != null)
                    {
                        Debug.Log($"Triggering special event: {specialEvent.EventName}");
                        var result = method.Invoke(specialEvent.EventScript, null) as IEnumerator;
                        if (result != null)
                        {
                            yield return StartCoroutine(result);
                        }
                    }
                    else
                    {
                        Debug.LogError($"Method {specialEvent.MethodName} not found on {specialEvent.EventScript.GetType().Name}.");
                    }
                }
            }

            ProceedToNextQuestion();
        }

        /// <summary>
        /// Proceeds to the next question or finishes the survey.
        /// </summary>
        private void ProceedToNextQuestion()
        {
            currentQuestionIndex++;

            if (currentQuestionIndex < surveyQuestions.Count)
            {
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
}