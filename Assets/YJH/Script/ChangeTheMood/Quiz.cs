using UnityEngine;
using TMPro;

namespace YJH.ChangeTheMood
{
    /// <summary>
    /// 문제 유형(정답/오답 있음, 없음)을 구분하기 위한 enum
    /// </summary>
    public enum QuizType
    {
        WithAnswer,  // 정답/오답이 존재하는 문제
        NoAnswer     // 정답/오답 개념이 없는 문항
    }

    [System.Serializable]
    public class QuizData
    {
        [Header("문제 유형")]
        public QuizType quizType = QuizType.WithAnswer;

        [Header("문제(질문)")]
        [TextArea]
        public string QuizConetn;

        [Header("정답 텍스트(또는 보기1)")]
        public string CorrectAnswer;

        [Header("오답 텍스트(또는 보기2)")]
        public string InCorrectAnswer;
    }
    public class Quiz : MonoBehaviour
    {
        [Header("Quiz Data")]
        [SerializeField]
        private QuizData quizData;

        [Header("UI")]
        [SerializeField] private TMP_Text QuizContText;  // 문제(질문) 표시
        [SerializeField] private TMP_Text AnswerAText;   // 선택지 A
        [SerializeField] private TMP_Text AnswerBText;   // 선택지 B

        [Header("Feedback Objects")]
        [SerializeField]
        private GameObject storyDialogueObject;
        [SerializeField]
        private GameObject correctFeedbackObject;   // 정답 연출 오브젝트
        [SerializeField]
        private GameObject incorrectFeedbackObject; // 오답 연출 오브젝트
        [SerializeField] private GameObject noAnswerFeedbackObject;   // 답이 없는 문제용 피드백

        // 선택지 중 어느 쪽이 정답인지 (0 = A, 1 = B)
        private int correctAnswerIndex = -1;

        // 사용자가 마지막으로 선택한 선택지 (0 = A, 1 = B, -1 = 아직 선택 안 함)
        private int selectedAnswerIndex = -1;

        // 사용자가 정답 맞췄는지 여부
        private bool userAnsweredCorrectly = false;

        private void OnEnable()
        {
            if (quizData != null)
            {
                // 문제 표시
                QuizContText.text = quizData.QuizConetn;

                // 정답/오답을 무작위로 A/B에 배치
                RandomizeAnswers();
            }
            else
            {
                Debug.LogWarning("QuizData is not assigned.");
            }
        }

        /// <summary>
        /// 정답/오답을 무작위로 A, B 중 어디에 놓을지 결정
        /// </summary>
        private void RandomizeAnswers()
        {
            // 50% 확률로 correctAnswer를 A에 배치, 나머지는 B에 배치
            bool placeCorrectInA = (Random.value > 0.5f);

            if (placeCorrectInA)
            {
                AnswerAText.text = quizData.CorrectAnswer;
                AnswerBText.text = quizData.InCorrectAnswer;
                correctAnswerIndex = 0; // A가 정답
            }
            else
            {
                AnswerAText.text = quizData.InCorrectAnswer;
                AnswerBText.text = quizData.CorrectAnswer;
                correctAnswerIndex = 1; // B가 정답
            }
        }

        /// <summary>
        /// 선택지 A 클릭 시 (Button OnClick 등으로 연결)
        /// </summary>
        public void SelectAnswerA()
        {
            selectedAnswerIndex = 0;
            CheckAnswer();
        }

        /// <summary>
        /// 선택지 B 클릭 시 (Button OnClick 등으로 연결)
        /// </summary>
        public void SelectAnswerB()
        {
            selectedAnswerIndex = 1;
            CheckAnswer();
        }

        /// <summary>
        /// 사용자 답안과 정답 비교
        /// </summary>
        private void CheckAnswer()
        {
            if (quizData == null)
            {
                Debug.LogWarning("QuizData not set, cannot check answer.");
                return;
            }

            if (quizData.quizType == QuizType.NoAnswer)
            {
                ShowNoAnswerFeedback();
            }
            else
            {
                userAnsweredCorrectly = (selectedAnswerIndex == correctAnswerIndex);


                if (userAnsweredCorrectly)
                {
                    Debug.Log("정답을 맞혔습니다!");
                    ShowCorrectFeedback();
                }
                else
                {
                    Debug.Log("오답입니다!");
                    ShowIncorrectFeedback();
                }


            }


            // 정답/오답에 따른 추가 로직(효과, 점수 등)을 여기서 처리

            storyDialogueObject.SetActive(false);
            gameObject.SetActive(false);
        }

        // ▼▼▼ 정답 연출용 함수 ▼▼▼
        private void ShowCorrectFeedback()
        {
            Debug.Log("정답을 맞혔습니다!");
            if (correctFeedbackObject != null) correctFeedbackObject.SetActive(true);

            // 필요시 추가 로직(애니메이션, 사운드, 점수 등)...
        }

        // ▼▼▼ 오답 연출용 함수 ▼▼▼
        private void ShowIncorrectFeedback()
        {
            Debug.Log("오답입니다!");
            if (incorrectFeedbackObject != null) incorrectFeedbackObject.SetActive(true);

            // 필요시 추가 로직(애니메이션, 사운드, 재시도 등)...
        }

        /// <summary>
        /// 정답/오답 개념이 없는 문제를 선택했을 때 보여줄 피드백
        /// </summary>
        private void ShowNoAnswerFeedback()
        {
            Debug.Log("정답/오답이 없는 문제를 선택했습니다. 동일 피드백을 표시합니다.");
            if (noAnswerFeedbackObject != null)
                noAnswerFeedbackObject.SetActive(true);
        }

        /// <summary>
        /// 퀴즈 종료 시 공통 처리
        /// </summary>
        private void EndQuiz()
        {
            if (storyDialogueObject != null)
                storyDialogueObject.SetActive(false);

            gameObject.SetActive(false);
        }

        /// <summary>
        /// 퀴즈의 정답 여부를 외부에서 확인
        /// </summary>
        public bool IsUserAnsweredCorrectly()
        {
            return userAnsweredCorrectly;
        }
    }
}
