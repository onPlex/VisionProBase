using UnityEngine;
using TMPro;
using UnityEngine.Events;

namespace YJH.ChangeTheMood
{
    [System.Serializable]
    public struct DialogueLine
    {
        public string speaker;  // 말하는 사람(캐릭터 이름 등)
        [TextArea]
        public string content;  // 실제 대사
    }

    public class CharacterDialogue : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField]
        private PhaseManager phaseManager;

        // 기존 string[] 대신, 대사를 말하는 캐릭터와 내용을 짝지어 보관
        [Header("Dialogue Lines")]
        [SerializeField]
        private DialogueLine[] dialogues;

        [Header("UI")]
        [SerializeField]
        private TMP_Text NickNameTextTMP;
        [SerializeField]
        private TMP_Text dialogueTextTMP;

        private int currentDialogueIndex = 0;

        [Header("Events")]
        [SerializeField]
        private UnityEvent onDialogueComplete;

        private void OnEnable()
        {
            if (dialogues != null && dialogues.Length > 0)
            {
                ShowDialogue();
            }
            else
            {
                Debug.LogWarning("No dialogues assigned.");
            }
        }

        private void ShowDialogue()
        {
            if (currentDialogueIndex >= 0 && currentDialogueIndex < dialogues.Length)
            {
                // 현재 대사의 화자 및 내용 정보
                string speaker = dialogues[currentDialogueIndex].speaker;
                string content = dialogues[currentDialogueIndex].content;

                // {Nickname} 치환 (필요에 따라 사용)
                content = content.Replace("{Nickname}", phaseManager.SelectedNickname);
              speaker = speaker.Replace("{Nickname}", phaseManager.SelectedNickname);
                // UI 반영
                NickNameTextTMP.text = speaker;
                dialogueTextTMP.text = content;
            }
            else
            {
                Debug.LogWarning("Index out of range or no dialogues available.");
            }
        }

        public void ShowNextDialogue()
        {
            if (currentDialogueIndex < dialogues.Length - 1)
            {
                currentDialogueIndex++;
                ShowDialogue();
            }
            else
            {
                Debug.Log("Reached the end of dialogues.");
                onDialogueComplete?.Invoke();
            }
        }
    }
}
