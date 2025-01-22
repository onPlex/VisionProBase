using UnityEngine;
using TMPro;

namespace YJH.ChangeTheMood
{

    [System.Serializable]
    public class QuizData
    {
       
        public string QuizConetn;
        public string CorrectAnswer;  
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
        /// 퀴즈의 정답 여부를 외부에서 확인
        /// </summary>
        public bool IsUserAnsweredCorrectly()
        {
            return userAnsweredCorrectly;
        }
    }
}
