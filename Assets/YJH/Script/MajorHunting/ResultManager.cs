using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace YJH.MajorHunting
{
    public class ResultManager : MonoBehaviour
    {
        // 정답 저장 (라운드별)
        private Dictionary<int, List<(string jobName, string jobDesc, string major, string majorDesc)>> roundAnswers
            = new Dictionary<int, List<(string, string, string, string)>>();

        [Header("Answer UI Elements")]
        [SerializeField] private TMP_Text[] AnswerTexts; // 미래직업 버튼 (JobName을 표시)
        [SerializeField] private TMP_Text JobNameText;  // 미래직업 표시
        [SerializeField] private TMP_Text JobDescText;  // 직업정보 표시
        [SerializeField] private TMP_Text MajorNameText; // 관련학과 표시
        [SerializeField] private TMP_Text MajorDescText; // 학과정보 표시

        private Dictionary<string, (string jobName, string jobDesc, string major, string majorDesc)> finalAnswers
            = new Dictionary<string, (string, string, string, string)>();

        /// <summary>
        /// 정답 저장 (라운드별 관리)
        /// </summary>
        public void AddCorrectAnswer(int round, string jobName, string jobDesc, string major, string majorDesc)
        {
            if (!roundAnswers.ContainsKey(round))
            {
                roundAnswers[round] = new List<(string, string, string, string)>();
            }

            roundAnswers[round].Add((jobName, jobDesc, major, majorDesc));
        }

        /// <summary>
        /// 1라운드 & 2라운드 모든 정답 삭제
        /// </summary>
        public void ClearAllAnswers()
        {
            roundAnswers.Clear();
            finalAnswers.Clear();
        }

        /// <summary>
        /// 특정 라운드(1 또는 2) 정답만 삭제
        /// </summary>
        public void ClearRoundAnswers(int round)
        {
            if (roundAnswers.ContainsKey(round))
            {
                roundAnswers[round].Clear();
            }
        }

        /// <summary>
        /// 최종 결과 출력 (정답 6개를 AnswerTexts에 표시)
        /// </summary>
        public void ShowFinalResults()
        {
            finalAnswers.Clear();

            // 모든 라운드의 정답을 합쳐서 저장
            foreach (var roundEntry in roundAnswers)
            {
                foreach (var answer in roundEntry.Value)
                {
                    finalAnswers[answer.jobName] = answer; // jobName을 key로 저장
                }
            }

            if (finalAnswers.Count == 0)
            {
                Debug.LogError("저장된 정답이 없습니다.");
                return;
            }

            OnClickAnswerButton(AnswerTexts[0]);
            UpdateAnswerButtons();
        }

        /// <summary>
        /// 버튼 UI에 JobName 표시
        /// </summary>
        private void UpdateAnswerButtons()
        {
            int index = 0;
            foreach (var job in finalAnswers.Keys)
            {
                if (index < AnswerTexts.Length)
                {
                    AnswerTexts[index].text = job; // 버튼에 미래직업명 표시
                    index++;
                }
                else
                {
                    break; // 최대 개수(6개) 초과 시 종료
                }
            }
        }

        /// <summary>
        /// 미래직업 버튼 클릭 시 해당하는 직업 정보를 UI에 표시
        /// </summary>
        public void OnClickAnswerButton(TMP_Text clickedButton)
        {
            string jobName = clickedButton.text;

            if (finalAnswers.ContainsKey(jobName))
            {
                var selectedAnswer = finalAnswers[jobName];

                JobNameText.text = $"<b>미래직업:</b> {selectedAnswer.jobName}";
                JobDescText.text = $"<b>직업정보:</b>\n{selectedAnswer.jobDesc}";
                MajorNameText.text = $"<b>관련학과:</b> {selectedAnswer.major}";
                MajorDescText.text = $"<b>학과정보:</b>\n{selectedAnswer.majorDesc}";
            }
            else
            {
                Debug.LogWarning($"해당하는 미래직업을 찾을 수 없습니다: {jobName}");
            }
        }
    }
}
