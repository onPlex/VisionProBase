using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Text;


namespace YJH
{
    public class MagicalGardenResult : MonoBehaviour
    {

        [SerializeField]
        TMP_Text tMP_Text;
        // 복수의 QuestionBoard를 Inspector에서 할당 가능하도록
        [SerializeField]
        private List<QuestionBoard> questionBoards;



        private void OnEnable()
        {
            DisplayAllResponses();
        }
        /// <summary>
        /// questionBoards에 등록된 모든 QuestionBoard의 
        /// playerResponses를 순회하여 결과를 텍스트로 표시합니다.
        /// </summary>
        public void DisplayAllResponses()
        {
            if (tMP_Text == null)
            {
                Debug.LogWarning("tMP_Text is not assigned in MagicalGardenResult.");
                return;
            }

            if (questionBoards == null || questionBoards.Count == 0)
            {
                Debug.LogWarning("No QuestionBoards assigned to MagicalGardenResult.");
                tMP_Text.text = "No QuestionBoards assigned.";
                return;
            }

            StringBuilder sb = new StringBuilder();

            // QuestionBoard들을 차례대로 순회
            for (int qbIndex = 0; qbIndex < questionBoards.Count; qbIndex++)
            {
                QuestionBoard qb = questionBoards[qbIndex];
                if (qb == null)
                    continue;

                // 각 QuestionBoard의 Dictionary (응답들)
                Dictionary<int, int> responses = qb.PlayerResponses;
                // 각 QuestionBoard의 문항들
                List<SurveyQuestion> questions = qb.SurveyQuestions;

                // 구분선 또는 타이틀 (옵션)
                sb.AppendLine($"--- QuestionBoard {qbIndex} ---");

                // 응답 딕셔너리 순회
                foreach (var pair in responses)
                {
                    int questionIndex = pair.Key;
                    int selectedOptionIndex = pair.Value;

                    // 안전 범위 체크
                    if (questionIndex >= 0 && questionIndex < questions.Count)
                    {
                        SurveyQuestion q = questions[questionIndex];
                        string questionTxt = q.QuestionText;

                        // 선택된 옵션이 유효한지 확인
                        if (selectedOptionIndex >= 0 && selectedOptionIndex < q.Options.Count)
                        {
                            string chosenOption = q.Options[selectedOptionIndex];
                            //sb.AppendLine($"Q{questionIndex} - {questionTxt} \n> 선택: {chosenOption}");
                             sb.AppendLine($"Q{questionIndex}-{chosenOption}");
                        }
                        else
                        {
                            // out of range 대비
                            //sb.AppendLine($"Q{questionIndex} - {questionTxt} \n> 선택: (Invalid Option Index {selectedOptionIndex})");
                             sb.AppendLine($"Q{questionIndex}-(Invalid Option Index {selectedOptionIndex})");
                        }
                    }
                    else
                    {
                        sb.AppendLine($"(Invalid Question Index {questionIndex})");
                    }
                }

                sb.AppendLine(); // 개행
            }

            // 최종 조합된 텍스트를 TMP에 표시
            tMP_Text.text = sb.ToString();
        }

    }
}
