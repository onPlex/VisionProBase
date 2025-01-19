using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

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
        [SerializeField] private List<JewelResultType> JewelResultTypes;

        #region UI
        [Header("UI")]
        [SerializeField] private RawImage JewelRawImage;
        [SerializeField] private TMP_Text JewelCategoryText;
        [SerializeField] private TMP_Text JewelNameText;

        [SerializeField] private TMP_Text CoreTitleText;
        [SerializeField] private TMP_Text CoreText;

        [SerializeField] private TMP_Text LevelUpTextTitle;
        [SerializeField] private TMP_Text LevelUpText;
        [SerializeField] private TMP_Text RecommendJobText;
        #endregion

        [Header("3DOBJ")]
        [SerializeField] private MeshRenderer DescBoardObj;

        [Header("Category")]
        [SerializeField] private GameObject CoreContent;
        [SerializeField] private GameObject LevelUpContent;
        [SerializeField] private GameObject JobContent;

        [Header("Data")]
        [SerializeField]
        private List<QuestionBoard> questionBoards; // 복수의 QuestionBoard를 할당

        // (1) 모든 QuestionBoard의 응답을 통합 저장하는 딕셔너리 (key: "전역 질문 인덱스", value: 선택지)
        private Dictionary<int, int> combinedResponses = new Dictionary<int, int>();

        // (2) 지능 유형별 점수
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

        // (3) 문항 인덱스 → 어떤 지능 유형인가를 알려주는 매핑
        private readonly string[] intelligenceOrder = new string[]
        {
            "언어지능", "대인관계지능", "자연친화지능", "논리수학지능",
            "음악지능", "신체운동지능", "공간지능", "자기이해지능"
        };

        /// <summary>
        /// 특정 QuestionBoard 한 개의 결과를 누적하여 저장하는 메서드
        /// </summary>
        /// <param name="boardIndex">questionBoards에서 처리할 보드의 인덱스</param>
        public void StoreBoardResult(int boardIndex)
        {
            // 인덱스 범위 체크
            if (boardIndex < 0 || boardIndex >= questionBoards.Count)
            {
                Debug.LogWarning($"StoreBoardResult: 잘못된 boardIndex({boardIndex})입니다.");
                return;
            }

            var board = questionBoards[boardIndex];
            if (board == null)
            {
                Debug.LogWarning($"StoreBoardResult: questionBoards[{boardIndex}] 가 null입니다.");
                return;
            }

            // (A) board의 플레이어 응답들을 사본으로 받아온다(순회 중 수정 방지)
            var responsesCopy = new Dictionary<int, int>(board.PlayerResponses);

            // (B) 이 QuestionBoard의 응답만 처리
            foreach (var response in responsesCopy)
            {
                int questionIndex = response.Key;    // 0 ~ 7 (문항 인덱스)
                int selectedOption = response.Value; // 0 ~ 4 (실제 선택지 인덱스, 점수는 +1 해서 1~5)

                // (B-1) 전역 인덱스 산출: "questionIndex + boardIndex * intelligenceOrder.Length"
                int globalIndex = questionIndex + boardIndex * intelligenceOrder.Length;

                // (B-2) combinedResponses에 저장 (기존에 있으면 덮어쓰기)
                if (combinedResponses.ContainsKey(globalIndex))
                {
                    Debug.LogWarning($"중복 응답 발견: QuestionBoard[{boardIndex}]의 questionIndex={questionIndex}가 이미 기록되어 있습니다.");
                }
                combinedResponses[globalIndex] = selectedOption;

                // (B-3) 지능 유형 점수 누적
                if (questionIndex >= 0 && questionIndex < intelligenceOrder.Length)
                {
                    string intelligenceType = intelligenceOrder[questionIndex];
                    // 선택 인덱스(0~4)에 +1 해서 실제 점수(1~5)
                    intelligenceScores[intelligenceType] += (selectedOption + 1);
                }
            }

            Debug.Log($"[StoreBoardResult] QuestionBoard[{boardIndex}] 저장 완료.");
        }

        /// <summary>
        /// 기존 점수들을 모두 초기화합니다.
        /// (새롭게 부분 저장을 시작할 때 호출)
        /// </summary>
        public void ResetAllScores()
        {
            combinedResponses.Clear();

            foreach (var key in intelligenceScores.Keys)
            {
                intelligenceScores[key] = 0;
            }

            Debug.Log("[ResetAllScores] 모든 점수 리셋 완료");
        }

        /// <summary>
        /// *이미 StoreBoardResult() 등으로 누적된* combinedResponses, intelligenceScores를 최종 표시
        /// </summary>
        public void CalculateFinalResult()
        {
            DisplayResults();
        }

        /// <summary>
        /// 통합된 결과를 UI/디버그로 출력합니다.
        /// </summary>
        private void DisplayResults()
        {
            // 결과를 문자열로 구성
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine("----- Survey Results -----");

            // (C) 지능 유형별 점수 출력
            foreach (var pair in intelligenceScores)
            {
                sb.AppendLine($"{pair.Key}: {pair.Value}점");
            }

            // (D) 최고 점수 계산
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
            sb.AppendLine($"[Highest Intelligence] {highestIntelligence} ({highestScore}점)");

            // 필요하다면 UI에 출력 가능
            // resultText.text = sb.ToString();
            MatchHighestIntelligenceUI(highestIntelligence);

            Debug.Log(sb.ToString());
        }

        private void MatchHighestIntelligenceUI(string highestIntelligence)
        {
            //  "언어지능", "대인관계지능", "자연친화지능", "논리수학지능", "음악지능", "신체운동지능", "공간지능", "자기이해지능"
            switch (highestIntelligence)
            {
                case "언어지능":
                    UISetUP(0);
                    break;
                case "대인관계지능":
                    UISetUP(1);
                    break;
                case "자연친화지능":
                    UISetUP(2);
                    break;
                case "논리수학지능":
                    UISetUP(3);
                    break;
                case "음악지능":
                    UISetUP(4);
                    break;
                case "신체운동지능":
                    UISetUP(5);
                    break;
                case "공간지능":
                    UISetUP(6);
                    break;
                case "자기이해지능":
                    UISetUP(7);
                    break;
            }
        }

        private void UISetUP(int index)
        {
            JewelRawImage.texture = JewelResultTypes[index].JewelRawImage;
            JewelCategoryText.text = JewelResultTypes[index].JewelCategoryText;
            JewelNameText.text = JewelResultTypes[index].JewelNameText;
            JewelNameText.color = JewelResultTypes[index].JwewlColor;


            CoreTitleText.text = JewelResultTypes[index].CorePowerTextTitle;
            CoreTitleText.color = JewelResultTypes[index].JwewlColor;
            CoreText.text = JewelResultTypes[index].CorePowerText;

            LevelUpTextTitle.text = JewelResultTypes[index].LevelUpText1Title;
            LevelUpText.text = JewelResultTypes[index].LevelUpText1;
            RecommendJobText.text = JewelResultTypes[index].RecommendJob;


            Material[] mats = DescBoardObj.materials;
            // URP의 기본 쉐이더라면 "_BaseColor" 키를, Built-in Pipeline이라면 "_Color"를 사용
            mats[0].SetColor("_BaseColor", JewelResultTypes[index].JwewlColor);
            DescBoardObj.materials = mats;
        }


        #region [UI Button Methods for Category Switching]
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
        #endregion
    }
}
