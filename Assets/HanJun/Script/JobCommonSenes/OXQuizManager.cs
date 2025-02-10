using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using NUnit.Framework.Constraints;

namespace Jun
{
    public class OXQuizManager : MonoBehaviour
    {
        [System.Serializable]
        public class QuizData
        {
            [TextArea] public string quizContent; // 퀴즈 내용
            public bool answerIsO;                // true = O, false = X
            [TextArea] public string explanation; // 해설
        }

        [Header("Round 1 (5문제)")]
        public QuizData[] round1 = new QuizData[5];

        [Header("Round 2 (5문제)")]
        public QuizData[] round2 = new QuizData[5];

        [Header("Target Animators")]
        public Animator leftTargetAnimator;  // 왼쪽 타겟 Animator
        public Animator rightTargetAnimator; // 오른쪽 타겟 Animator

        [Header("UI References")]
        public TMP_Text questionText;     // 퀴즈 내용 표시
        public TMP_Text explanationText;  // 정답/해설 표시
        public GameObject explanationPanel; // 해설을 보여줄 패널(UI)

        public JobCommonSenseController jobCommonSenseController;

        private int currentRound = 1;     // 현재 라운드(1 or 2)
        private int questionIndex = 0;    // 현재 문제 번호(0~4)
        private bool isWaitingNext = false; // 해설 확인 후 넘어가는 중인지
        private bool isRound2 = false;
        private bool isEndQuiz = false;

        private int scoreNumber = 0;
        private int roundScore = 0;

        public int ScoreNumber { get => scoreNumber; set => scoreNumber = value; }

        void Start()
        {
            // TSV 내용에 맞춰 하드코딩 (Round1, Round2)
            // =================== Round1 ===================
            round1[0].quizContent = "부검 전문의는 외과의사만 할 수 있다.";
            round1[0].answerIsO = false; // X
            round1[0].explanation = "외과의사뿐만 아니라 다양한 의학적 배경을 가진 의사는 부검 전공을 할 수 있습니다.\n법의학 교육과 훈련을 받은 법의학 전문의가 부검을 수행합니다.";

            round1[1].quizContent = "간호사와 간호조무사는 비슷한 업무를 수행한다.";
            round1[1].answerIsO = false; // X
            round1[1].explanation = "간호사와 간호조무사는 교육 수준, 역할, 책임에서 차이가 있습니다.\n간호사는 3~4년제 간호학과 졸업 후 국가면허시험을 통과해야 하며, 전문적이고 독립적인 역할을 수행합니다.\n간호조무사는 1~2년의 전문교육과정 이수 후 자격증을 취득하며, 간호사를 지원하는 역할입니다.";

            round1[2].quizContent = "건축가는 인테리어 디자이너가 하는 업무를 할 수 있다.";
            round1[2].answerIsO = true; // O
            round1[2].explanation = "건축가는 건물의 외관과 구조 설계뿐만 아니라 내부 공간 디자인에도 참여할 수 있습니다.\n다만, 전문적인 인테리어 디자인에는 추가 교육이나 자격증이 필요할 수 있습니다.";

            round1[3].quizContent = "회계사 자격증을 따면 세무사로도 개업할 수 있다.";
            round1[3].answerIsO = false; // X
            round1[3].explanation = "세무사 자격증은 공인회계사(CPA) 자격증과 별도로 존재합니다.\nCPA 소지자가 세무사 시험에 응시하면 일부 면제 혜택을 받을 수는 있지만, 그대로 개업 가능한 것은 아닙니다.";

            round1[4].quizContent = "항공교통관제사는 여러 대의 항공기를 동시에 관제할 수 있다.";
            round1[4].answerIsO = true; // O
            round1[4].explanation = "항공기들의 안전한 이착륙과 비행을 위해 실시간으로 여러 항공기를 동시에 관제해야 합니다.";

            // =================== Round2 ===================
            round2[0].quizContent = "치과기공사는 환자의 구강 상태를 진단할 수 있는 권한이 있다.";
            round2[0].answerIsO = false; // X
            round2[0].explanation = "치과기공사는 보철물, 교정기, 의치 등을 제작하는 역할로, 진단 및 치료는 치과의사의 업무입니다.";

            round2[1].quizContent = "기상캐스터는 기상 데이터를 직접 수집하고 분석해 날씨 정보를 전달한다.";
            round2[1].answerIsO = false; // X
            round2[1].explanation = "기상 데이터 수집·분석은 기상청 전문가들이 담당합니다.\n기상캐스터는 이 데이터를 해석·정리하여 시청자에게 전달하는 역할입니다.";

            round2[2].quizContent = "큐레이터와 도슨트 직업 모두 전시 안내가 주요 업무다.";
            round2[2].answerIsO = false; // X
            round2[2].explanation = "큐레이터는 전시 기획, 작품 수집·관리 등 전반을 책임지고,\n도슨트는 전시 해설 및 관람객과 소통을 담당합니다. 업무 범위가 다릅니다.";

            round2[3].quizContent = "비서는 경영에 대한 기본 지식과 이해가 있어야 한다.";
            round2[3].answerIsO = true; // O
            round2[3].explanation = "비서는 회의 자료, 의전, 스케줄 관리 등 다양하고 폭넓은 업무를 수행해야 하므로\n경영진을 보좌하기 위한 경영 지식도 필요합니다.";

            round2[4].quizContent = "의료코디네이터는 환자와 의료진 간의 커뮤니케이션을 원활하게 하는 역할을 한다.";
            round2[4].answerIsO = true; // O
            round2[4].explanation = "환자의 진료 일정, 치료 과정, 결과 등을 설명하거나, 의료진에 환자 정보를 전달하며\n양측 간 소통을 돕는 중요한 역할을 합니다.";

            // 첫 문제 표시
            explanationPanel.SetActive(false);
            LoadQuestion();
        }

        /// <summary>
        /// 현재 라운드 & 문제번호에 해당하는 문제를 UI에 세팅
        /// </summary>
        void LoadQuestion()
        {
            if (currentRound > 2)
            {
                // 모든 라운드 종료
                questionText.text = "모든 문제를 마쳤습니다.";
                return;
            }

            // roundText.text = "Round " + currentRound + " / Question " + (questionIndex + 1);
            QuizData qd = (currentRound == 1) ? round1[questionIndex] : round2[questionIndex];
            questionText.text = qd.quizContent;
            explanationPanel.SetActive(false);
        }

        /// <summary>
        /// 왼쪽 타겟(O)에 맞췄을 때
        /// </summary>
        public void OnSelectO()
        {
            if (isWaitingNext) return; // 해설 표시 중 클릭 방지
            CheckAnswer(true);
        }

        /// <summary>
        /// 오른쪽 타겟(X)에 맞췄을 때
        /// </summary>
        public void OnSelectX()
        {
            if (isWaitingNext) return;
            CheckAnswer(false);
        }

        /// <summary>
        /// 정답 확인
        /// </summary>
        void CheckAnswer(bool userChoiceO)
        {
            QuizData qd = (currentRound == 1) ? round1[questionIndex] : round2[questionIndex];
            bool isCorrect = (qd.answerIsO == userChoiceO);

            if (isCorrect)
            {
                // 정답
                // explanationText.text = "<color=green>정답입니다!</color>\n\n" + qd.explanation;

                // ============ [정답 애니메이션 재생] ============
                if (userChoiceO)
                {
                    // O(왼쪽) 선택이 정답
                    // leftTargetAnimator.SetBool("Correct", true);
                    leftTargetAnimator.SetTrigger("Correct");
                }
                else
                {
                    // X(오른쪽) 선택이 정답
                    // rightTargetAnimator.SetBool("Correct", true);
                    rightTargetAnimator.SetTrigger("Correct");
                }

                ScoreNumber++;
                roundScore++;

                // 다음 문제로 넘어갈 준비
                StartCoroutine(WaitAndNext());
            }
            else
            {
                // 오답
                explanationText.text = "<color=red>오답입니다!</color>\n\n" + qd.explanation;
                // 해설 패널 활성화
                explanationPanel.SetActive(true);
            }


        }

        /// <summary>
        /// 일정 시간 후 다음 문제로 넘어감
        /// </summary>
        IEnumerator WaitAndNext()
        {
            isWaitingNext = true;
            yield return new WaitForSeconds(2.0f); // 해설 2초 정도 보여주고

            NextQuiz();
        }

        public void NextQuiz()
        {
            // 문제 인덱스 증가
            questionIndex++;

            // 만약 5문제 다 풀었으면 다음 라운드로
            if (questionIndex >= 5)
            {
                if (!isRound2)
                {
                    isRound2 = true;
                    currentRound++;
                    questionIndex = 0;
                    roundScore = 0;
                 

                    jobCommonSenseController.SetPhaseIndex(4);
                }
                else
                {
                    jobCommonSenseController.SetPhaseIndex(5);
                    isEndQuiz = true;
                }
            }


            if (isEndQuiz) return;

            // 다음 문제 UI 로드
            LoadQuestion();
            isWaitingNext = false;
        }

    }
}