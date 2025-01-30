using UnityEngine;
using TMPro;
using UnityEngine.Events;
using System.Collections;


namespace YJH.ChangeTheMood
{
    [System.Serializable]
    public class DialogueLine
    {
        // 어떤 speakerObjects 인덱스를 사용할지 (-1 -> 주인공 캐릭터 -> 자동 치환 됨 speakerObjects[0] 남자 , speakerObjects[1] 여자)
        public int speakerIndex;
        public string speaker;  // 말하는 사람(캐릭터 이름 등)       
        public string content;  // 실제 대사
    }

    public class CharacterDialogue : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField]
        private PhaseManager phaseManager;

        // 기존 string[] 대신, 대사를 말하는 캐릭터와 내용을 짝지어 보관
        [Header("Dialogue Lines \n 주인공 캐릭터 -> -1")]
        [SerializeField]
        private DialogueLine[] dialogues;

        [Header("Speakers (Characters) \n - speakerObjects[0] 남자 , speakerObjects[1] 여자 필수")]
        [SerializeField]
        private GameObject[] speakerObjects; //speakerObjects[0] 남자 , speakerObjects[1] 여자 필수

        [Header("UI")]
        [SerializeField]
        private TMP_Text NickNameTextTMP;
        [SerializeField]
        private TMP_Text dialogueTextTMP;

        [SerializeField]
        private GameObject ProcessButtonObj;

        private int currentDialogueIndex = 0;

        [Header("Events")]
        [SerializeField]
        private UnityEvent onDialogueComplete;


        [Header("Move Events")]
        [SerializeField]
        private UnityEvent onMoveComplete;
        [SerializeField]
        float distance = 0.6f;
        [SerializeField]
        float duration = 1.5f;




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

                // 현재 대사 정보 가져오기
                DialogueLine currentLine = dialogues[currentDialogueIndex];

                // speakerIndex 기반으로 캐릭터 활성화/비활성화
                ActivateSpeakerByIndex(currentLine.speakerIndex);

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

        private void ActivateSpeakerByIndex(int speakerIndex)
        {
            // (1) 모든 스피커 오브젝트 비활성화
            foreach (var speakerObj in speakerObjects)
            {
                speakerObj.SetActive(false);
            }

            // (2) speakerIndex == -1이면, Sex에 따라 예외 처리
            if (speakerIndex == -1)
            {
                if (phaseManager.SelectedSex == PhaseManager.Sex.Boy)
                {
                    // 예: speakerObjects[0] 사용
                    if (speakerObjects.Length > 0)
                        speakerObjects[0].SetActive(true);
                    else
                        Debug.LogWarning("speakerObjects 배열이 비어 있음");
                }
                else if (phaseManager.SelectedSex == PhaseManager.Sex.Girl)
                {
                    // 예: speakerObjects[1] 사용
                    if (speakerObjects.Length > 1)
                        speakerObjects[1].SetActive(true);
                    else
                        Debug.LogWarning("speakerObjects 배열 크기가 2개 미만임");
                }
                else
                {
                    Debug.LogWarning("Unknown Sex type, cannot set speaker object properly.");
                }
            }
            else
            {
                // (3) 기존 로직: 0 이상일 때 일반 인덱스 처리
                if (speakerIndex >= 0 && speakerIndex < speakerObjects.Length)
                {
                    speakerObjects[speakerIndex].SetActive(true);
                }
                else
                {
                    Debug.LogWarning($"Speaker index {speakerIndex} is out of range for speakerObjects[] array.");
                }
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

        public void MoveUpAndDeactivateSpeakers()
        {

            ProcessButtonObj.SetActive(false);
            // 1) 모든 speakerObjects 비활성화
            foreach (var speakerObj in speakerObjects)
            {
                speakerObj.SetActive(false);
            }

            // 2) 코루틴으로 Y축 이동
            StartCoroutine(MoveUpRoutine(distance, duration));
        }

        private IEnumerator MoveUpRoutine(float distance, float duration)
        {
            Vector3 startPos = transform.position;
            Vector3 endPos = startPos + Vector3.up * distance;

            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
                transform.position = Vector3.Lerp(startPos, endPos, t);
                yield return null;
            }

            // 이동이 끝난 후 이벤트 호출
            onMoveComplete?.Invoke();
        }
    }
}
