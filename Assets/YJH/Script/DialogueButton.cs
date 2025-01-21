using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events; // UnityEvent 사용

namespace YJH
{
    [System.Serializable]
    public struct DialoguePair
    {
        [TextArea]
        public string DialogueText; // Text content for dialogue
        public AudioClip DialogueAudio; // Corresponding audio clip
        public bool TriggerEvent; // Flag to trigger an event before moving to the next dialogue

        public UnityEvent EventOnTrigger;    // Inspector에서 등록 가능한 이벤트
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
        // public delegate void DialogueEvent();
        // public event DialogueEvent OnTriggerDialogueEvent;

        // "트리거 이벤트 완료" 신호 대기를 위한 상태값
        private bool eventCompleted = false;

        // 이벤트가 실행 중인지 여부
        private bool isEventRunning = false;


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
                // 텍스트 표시
                dialogueText.text = dialoguePairs[index].DialogueText;

                // 오디오 재생
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
            // 이벤트가 이미 진행 중이면 무시
            if (isEventRunning)
            {
                return;
            }

            // 대사 범위 초과 확인
            if (currentDialogueIndex >= dialoguePairs.Count)
            {
                Debug.LogWarning("No more dialogues to proceed.");
                return;
            }

            // 트리거 이벤트가 있는 대사라면, 코루틴으로 이벤트 완료 대기
            if (dialoguePairs[currentDialogueIndex].TriggerEvent)
            {
                StartCoroutine(WaitForEventAndProceed());
            }
            else
            {
                // 바로 다음 대사 진행
                ProceedToNextDialogue();
            }
        }

        /// <summary>
        /// Waits for the event to complete before proceeding.
        /// </summary>
        private IEnumerator WaitForEventAndProceed()
        {
            eventCompleted = false;
            isEventRunning = true; // 이벤트 진행 중

            // Inspector에서 등록된 UnityEvent 실행
            dialoguePairs[currentDialogueIndex].EventOnTrigger?.Invoke();

            // MarkEventComplete()가 호출될 때까지 대기
            yield return new WaitUntil(() => eventCompleted);

            // 이벤트 끝 → 이벤트 진행 상태 해제
            isEventRunning = false;

            // 이벤트가 끝난 뒤 다음 대사 진행
            ProceedToNextDialogue();
        }

        /// <summary>
        /// Inspector에서 등록된 이벤트 쪽에서 "이벤트 작업이 끝났다"를 알리기 위해 호출
        /// </summary>
        public void MarkEventComplete()
        {
            eventCompleted = true;
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
                    this.gameObject.SetActive(false);
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