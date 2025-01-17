using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEditor.Rendering;

namespace YJH
{
    [System.Serializable]
    public struct JewelResultType
    {
        public Texture JewelRawImage;
        public string JewelCategoryText;
        public string JewelNameText;
        public Color JwewlColor;

        public string CorePowerTextTitle;

        public string CorePowerText;

        public string LevelUpText1Title;
        public string LevelUpText1;
        public string LevelUpText2Title;
        public string LevelUpText2;
        public string LevelUpText3Title;
        public string LevelUpText3;

        public string RecommendJob;
    }

    public class EmotionJewelResult : MonoBehaviour
    {


        [SerializeField]
        List<JewelResultType> JewelResultTypes;

        [SerializeField]
        RawImage JewelRawImage;
        [SerializeField]
        TMP_Text JewelCategoryText;
        [SerializeField]
        TMP_Text JewelNameText;

        [SerializeField]
        TMP_Text CoreTitleText;
        [SerializeField]
        TMP_Text CoreText;

        [SerializeField]
        TMP_Text LevelUpTextTitle;
        [SerializeField]
        TMP_Text LevelUpText;
        [SerializeField]
        TMP_Text RecommendJobText;


        [Header("Category")]
        [SerializeField]
        GameObject CoreContent;
        [SerializeField]
        GameObject LevelUpContent;
        [SerializeField]
        GameObject JobContent;

        [Header("Data")]
        [SerializeField]
        private List<QuestionBoard> questionBoards; // 복수의 QuestionBoard를 할당

        private Dictionary<int, int> combinedResponses = new Dictionary<int, int>(); // 모든 응답을 통합

        // 지능 유형별 점수 저장
        private readonly Dictionary<string, int> intelligenceScores = new Dictionary<string, int>
        {
            { "언어지능", 0 },
            { "대인관계지능", 0 },
            { "자연친화지능", 0 },
            { "논리수학지능", 0 },
            { "음악지능", 0 },
            { "신체운동지능", 0 },
            { "공간지능", 0 },
            { "자기이해지능", 0 }
        };

        // 문항별 지능 유형 매핑 (각 QuestionBoard에서 순차적으로 반복)
        private readonly string[] intelligenceOrder = new string[]
        {
            "언어지능", "대인관계지능", "자연친화지능", "논리수학지능", "음악지능", "신체운동지능",
            "공간지능", "자기이해지능"
        };

private void OnEnable() {
    ProcessSurveyResults();
}

        /// <summary>
        /// 모든 QuestionBoard의 응답을 통합하고 결과를 처리합니다.
        /// </summary>
        private void ProcessSurveyResults()
        {
            combinedResponses.Clear();
            foreach (var key in intelligenceScores.Keys)
            {
                intelligenceScores[key] = 0;
            }

            // 모든 QuestionBoard의 응답을 통합
            for (int boardIndex = 0; boardIndex < questionBoards.Count; boardIndex++)
            {
                var board = questionBoards[boardIndex];
                if (board == null) continue;

                var responses = board.PlayerResponses;

                foreach (var response in responses)
                {
                    int questionIndex = response.Key;
                    int selectedOption = response.Value;

                    // 통합 응답에 추가
                    if (combinedResponses.ContainsKey(questionIndex + boardIndex * intelligenceOrder.Length))
                    {
                        Debug.LogWarning($"Duplicate response detected for question {questionIndex + boardIndex * intelligenceOrder.Length}. Overwriting with the latest response.");
                    }

                    combinedResponses[questionIndex + boardIndex * intelligenceOrder.Length] = selectedOption;

                    // 지능 유형 점수 누적
                    if (questionIndex >= 0 && questionIndex < intelligenceOrder.Length)
                    {
                        string intelligenceType = intelligenceOrder[questionIndex];
                        intelligenceScores[intelligenceType] += (selectedOption + 1); // 점수는 1~5로 계산
                    }
                }
            }

            DisplayResults();
        }


        /// <summary>
        /// 통합된 결과를 표시합니다.
        /// </summary>
        private void DisplayResults()
        {


            // 결과를 문자열로 구성
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine("Survey Results:");

            // 지능 유형별 점수 출력
            foreach (var pair in intelligenceScores)
            {
                sb.AppendLine($"{pair.Key}: {pair.Value}점");
            }

            // 최고 점수 계산
            string highestIntelligence = "";
            int highestScore = int.MinValue;

            foreach (var pair in intelligenceScores)
            {
                if (pair.Value > highestScore)
                {
                    highestIntelligence = pair.Key;
                    highestScore = pair.Value;
                }
            }

            sb.AppendLine();
            sb.AppendLine($"Highest Intelligence: {highestIntelligence} ({highestScore}점)");

            // 결과를 TMP_Text에 출력
            // resultText.text = sb.ToString();

            Debug.Log("Survey Results Displayed:");
            Debug.Log(sb.ToString());
        }

        public void OnClickContentCore()
        {
            CoreContent.SetActive(true);
            LevelUpContent.SetActive(false);
            JobContent.SetActive(false);
        }
        public void OnClickContentLevelUp()
        {
            CoreContent.SetActive(false);
            LevelUpContent.SetActive(true);
            JobContent.SetActive(false);
        }

        public void OnClickContentJob()
        {
            CoreContent.SetActive(false);
            LevelUpContent.SetActive(false);
            JobContent.SetActive(true);
        }

    }
}
