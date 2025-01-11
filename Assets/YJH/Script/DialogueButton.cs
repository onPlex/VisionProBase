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
            currentDialogueIndex++;

            if (currentDialogueIndex < dialoguePairs.Count)
            {
                ShowDialogue(currentDialogueIndex);
            }
            else
            {
                Debug.Log("End of dialogues.");
                currentDialogueIndex = 0; // Optionally loop to the start
            }
        }
    }
}