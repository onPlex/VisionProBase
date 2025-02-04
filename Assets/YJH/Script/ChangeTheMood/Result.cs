using UnityEngine;
using TMPro;


namespace YJH.ChangeTheMood
{
    public class Result : MonoBehaviour
    {
        [Header("Quizzes to Check")]
        [SerializeField]
        private Quiz[] quizzes;  // 여러 Quiz 스크립트를 참조

        [SerializeField]
        private GameObject CorrectObj;
        [SerializeField]
        private GameObject InCorrectObj;

        [Header("InfoUpdate")]
        [SerializeField]
        PhaseManager phaseManager;
        [SerializeField]
        TMP_Text CorrectResultPlayerNickName;
        [SerializeField]
        TMP_Text InCorrectResultPlayerNickName;

        public bool isHappyEnd;
        /// <summary>
        /// 전체 Quiz들의 정답 여부를 평가
        /// </summary>
        public void EvaluateQuizzes()
        {
            // 하나라도 오답이 있으면 false로 변경할 플래그
            bool allCorrect = true;

            // 모든 Quiz를 확인
            foreach (Quiz quiz in quizzes)
            {
                if (quiz == null)
                {
                    Debug.LogWarning("Quiz 참조가 null입니다. 건너뜁니다.");
                    continue;
                }

                // 하나라도 오답이면 플래그를 false로 만들고 루프 탈출
                if (!quiz.IsUserAnsweredCorrectly())
                {
                    allCorrect = false;
                    break;
                }
            }

            SetUpNickName();
            // 최종 판단 후 함수 호출
            if (allCorrect)
            {
                OnAllCorrect();
            }
            else
            {
                OnSomeIncorrect();
            }
        }

        /// <summary>
        /// 모든 Quiz를 정답 맞췄을 때 호출
        /// </summary>
        private void OnAllCorrect()
        {
            Debug.Log("[Result] 모든 문제 정답!");
            // 원하는 로직(예: 보상, 다음 스토리 진행, UI 표시 등)
            CorrectObj.SetActive(true);
            isHappyEnd = true;
        }

        /// <summary>
        /// 하나라도 오답이 있을 때 호출
        /// </summary>
        private void OnSomeIncorrect()
        {
            Debug.Log("[Result] 오답이 있습니다.");
            // 원하는 로직(예: 재시도 버튼 표시, 안내 메시지 등)
            InCorrectObj.SetActive(true);
            isHappyEnd = false;
        }

        public void SetUpNickName()
        {
            // 정답 결과 화면에 표시
            if (CorrectResultPlayerNickName != null)
            {
                string textToDisplay =
              $"김시우 - 7표\n반장 - 2표\n{phaseManager.SelectedNickname} - 11표";
                CorrectResultPlayerNickName.text = textToDisplay;
            }

            // 오답 결과 화면에 표시
            if (InCorrectResultPlayerNickName != null)
            {
                string textToDisplay =
              $"김시우 - 12표\n반장 - 7표\n{phaseManager.SelectedNickname} - 1표";
                InCorrectResultPlayerNickName.text = textToDisplay;
            }
        }
    }
}