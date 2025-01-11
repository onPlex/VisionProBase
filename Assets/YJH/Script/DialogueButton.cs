using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace YJH
{
    [System.Serializable]
    public struct DialoguePair
    {
        [TextArea]
        public string DialogueText; // Text content for dialogue
        public AudioClip DialogueAudio; // Corresponding audio clip
        public bool TriggerEvent; // Flag to trigger an event before moving to the next dialogue
    }

    public class DialogueButton : MonoBehaviour
    {
        // UI Elements
        [SerializeField] private TMP_Text dialogueText;

        // List of dialogue pairs
        [SerializeField] private List<DialoguePair> dialoguePairs;

        // AudioSource for playing audio
        private AudioSource audioSource;

        // Current dialogue index
        private int currentDialogueIndex = 0;

        [SerializeField]
        private ContentPhaseManager contentPhaseManager;

        // Event to trigger during specific dialogues
        public delegate void DialogueEvent();
        public event DialogueEvent OnTriggerDialogueEvent;

        void Awake()
        {
            // Initialize AudioSource
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        void Start()
        {
            // Start with the first dialogue
            if (dialoguePairs != null && dialoguePairs.Count > 0)
            {
                ShowDialogue(currentDialogueIndex);
            }
            else
            {
                Debug.LogWarning("Dialogue pairs are missing!");
            }
        }

        /// <summary>
        /// Shows the current dialogue and plays its audio.
        /// </summary>
        /// <param name="index">Index of the dialogue to display.</param>
        private void ShowDialogue(int index)
        {
            if (index >= 0 && index < dialoguePairs.Count)
            {
                // Display dialogue text
                dialogueText.text = dialoguePairs[index].DialogueText;

                // Play audio
                audioSource.Stop();
                audioSource.clip = dialoguePairs[index].DialogueAudio;
                audioSource.Play();

                // Check if an event should be triggered
                if (dialoguePairs[index].TriggerEvent && OnTriggerDialogueEvent != null)
                {
                    OnTriggerDialogueEvent.Invoke();
                }
            }
            else
            {
                Debug.LogWarning("Index out of range for dialogue pairs.");
            }
        }

        /// <summary>
        /// Advances to the next dialogue when the button is clicked.
        /// </summary>
        public void OnButtonClick()
        {
            if (currentDialogueIndex >= dialoguePairs.Count)
            {
                Debug.LogWarning("No more dialogues to proceed.");
                return; // Prevent further execution if out of range
            }

            if (dialoguePairs[currentDialogueIndex].TriggerEvent)
            {
                StartCoroutine(WaitForEventAndProceed());
            }
            else
            {
                ProceedToNextDialogue();
            }
        }

        /// <summary>
        /// Waits for the event to complete before proceeding.
        /// </summary>
        private IEnumerator WaitForEventAndProceed()
        {
            bool eventCompleted = false;

            // Subscribe to the event to mark completion
            OnTriggerDialogueEvent += () => { eventCompleted = true; };

            // Wait until the event is completed
            yield return new WaitUntil(() => eventCompleted);

            ProceedToNextDialogue();
        }

        /// <summary>
        /// Proceeds to the next dialogue.
        /// </summary>
        private void ProceedToNextDialogue()
        {
            currentDialogueIndex++;

            if (currentDialogueIndex < dialoguePairs.Count)
            {
                ShowDialogue(currentDialogueIndex);
            }
            else
            {
                Debug.Log("End of dialogues.");
                if (contentPhaseManager != null)
                {
                    contentPhaseManager.OnGamePrologueEnd();
                }
                else
                {
                    Debug.LogError("ContentPhaseManager is not assigned.");
                }
            }
        }
    }
}