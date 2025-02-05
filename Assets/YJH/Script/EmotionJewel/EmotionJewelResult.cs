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
            Debug.Log(sb.ToString());
            MatchHighestIntelligenceUI(highestIntelligence);



            // 더미 데이터 전송
            SendDummyResultByHighestIntelligence(highestIntelligence);


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

        /// <summary>
        /// highestIntelligence에 따라 임시/더미 데이터 셋팅 후 SendGameResult 호출
        /// </summary>
        private void SendDummyResultByHighestIntelligence(string highestIntelligence)
        {
            // 각각의 지능 타입별로 "ContDATA, ImgTypeDATA, StatusDATA"를 다르게 넣는 예시
            string contData = "기본 더미 내용";
            int imgTypeData = -1;
            int statusData = 0;

            switch (highestIntelligence)
            {
                case "언어지능":
                    contData = "노벨라 다이아 : 보인다, 미래의 언어천재! 말과 글을 통해 생각을 효과적으로 표현하고, 사람들과 소통하는 언어능력을 가졌군요. 언어의 의미, 리듬과 소리를 잘 파악하고, 창의적으로 표현하는 데 강점이 있어요. 꾸준히 독서하는 습관을 기르고, 타인과 함께 생각과 감정을 나누는 토론 및 연극 활동을 통해 강점을 강화해 보세요!";
                    imgTypeData = 8;    // 예: 1번 이미지
                    statusData = 1;   // 임시 상태 코드
                    break;
                case "대인관계지능":
                    contData = "크라우드 사파이어 : 어디를 가나 인기쟁이! 다른 사람의 말과 행동을 통해 기분과 감정을 잘 헤아리는 인간친화 능력을 가졌군요! 주변 사람들과 공감대를 형성하여 조화롭게 관계를 맺고 협력하는 데 강점이 있어요. 꾸준한 봉사활동, 전국 청소년 동아리나 학생기자단 등 학교 외 활동에 적극 참여하여 강점을 강화해 보세요!";
                    imgTypeData = 9;
                    statusData = 1;
                    break;
                case "자연친화지능":
                    contData = "네이처 펄 : 세상을 꿰뚫어 보는 인간 돋보기! 뛰어난 관찰력과 탐구 능력을 가졌군요! 특히 자연현상, 동식물, 사람을 잘 관찰하고, 환경 변화를 민감하게 감지하여 탐구하는 데 강점이 있어요. 산책/등산/캠핑 등 야외 활동을 하며 자연 속 시간을 즐기거나, 일상 속 작은 변화를 기록하는 관찰 일기를 쓰며 강점을 강화해 보세요!";
                    imgTypeData = 10;
                    statusData = 1;
                    break;
                case "논리수학지능":
                    contData = "로지 가넷 : 이 구역 논리왕! 논리적으로 생각하고 추론하는 능력을 가졌군요! 특히 수학적 공식이나 규칙을 이해하고, 문제를 체계적으로 분석하여 해결 방법을 찾는 데 강점이 있어요. 논리적 사고와 추론 능력을 향상시키는 공상과학소설을 읽거나, 퍼즐/스도쿠/미로/체스 등 두뇌 회전에 좋은 논리 게임을 즐기며 강점을 강화해 보세요!";
                    imgTypeData = 11;
                    statusData = 1;
                    break;
                case "음악지능":
                    contData = "뮤즈 페리도트 : 타고난 흥부자! 자신의 감정을 음악으로 잘 표현하는 능력을 가졌군요! 소리의 높낮이, 리듬, 멜로디, 음색 등의 미묘한 차이를 잘 알아차리고, 창의적으로 활용하는 데 강점이 있어요. 새로운 장르의 음악을 감상하거나 평소 관심 있었던 악기를 배워보세요. 또는 태블릿, 휴대폰 속 무료 작곡 앱들을 활용하여 나만의 음악을 만들며 강점을 강화해 보세요!";
                    imgTypeData = 12;
                    statusData = 1;
                    break;
                case "신체운동지능":
                    contData = "애슬릿 토파즈 : 타고난 운동신경! 나의 몸을 능숙하게 사용하고 조절하는 능력을 가졌군요. 운동, 무용처럼 신체를 통해 생각을 표현하거나, 공예처럼 물건을 솜씨 있게 다루는 데 강점이 있어요. 새로운 스포츠 활동에 도전하거나, 목공/도자기 만들기/그림 등 손을 활용한 공예 활동을 즐기며 강점을 강화해 보세요!";
                    imgTypeData = 13;
                    statusData = 1;
                    break;
                case "공간지능":
                    contData = "스페이스 루비 : 치밀한 설계자! 눈으로 본 것들을 잘 기억하고, 머릿속에 그려내는 능력을 가졌군요. 지도, 그림, 설계도를 쉽게 이해하고, 길이나 물건 위치를 기억하는 데 강점이 있어요. 우리 지역에서 유명한 대표 건축물들을 찾아가 스케치하거나, 직접 동네를 누비며 우리 동네 지도를 만들어 보면서 강점을 강화해 보세요!";
                    imgTypeData = 14;
                    statusData = 1;
                    break;
                case "자기이해지능":
                    contData = "퍼스널 에메랄드 : 진정한 내 마음의 주인! 자신의 감정을 잘 알고 다스리는 능력을 가졌군요. 장단점, 특기, 희망, 관심 등 자신의 모습을 잘 알고 관리하며, 계획과 목표 달성을 위해 행동하는 데 강점이 있어요. 친구들에게 나를 소개하는 포트폴리오를 만들어 보거나, 롤 모델을 찾아 배우고 싶은 점을 정리한 후 일상 속에서 실천하며 강점을 강화해 보세요!";
                    imgTypeData = 15;
                    statusData = 1;
                    break;
                default:
                    // 혹시라도 매칭되는 케이스가 없을 시 기본값
                    contData = "조금 더 시간이 있었다면 좋았을 텐데 아쉽다… 하지만 오늘의 아쉬움은 내일의 성공을 위한 준비! 다음 번에는 꼭 너만의 강점 보석을 찾을 수 있을 거야.";
                    imgTypeData = 16;
                    statusData = 1;
                    break;
            }

            // 전송
            //sendResultData.SendGameResult(contData, imgTypeData, statusData);
            ResultDataStorage.Instance.Game2ContData = contData;
            ResultDataStorage.Instance.Game2ImgTypeData =  imgTypeData;
            ResultDataStorage.Instance.Game2StatusData = statusData;
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
