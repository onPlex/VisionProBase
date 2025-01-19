using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Text;

namespace YJH
{
    public class MagicalGardenResult : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text tMP_Text;

        [SerializeField]
        private List<QuestionBoard> questionBoards;

        // (1) 응답을 전부 모아두는 딕셔너리 (key: "전역 질문 인덱스", value: 선택 옵션)
        private Dictionary<int, int> combinedResponses = new Dictionary<int, int>();

        [Header("Mesh & Material")]
        [SerializeField] private MeshRenderer targetMeshRenderer;
        // 두 번째 Material을 가리키므로 보통 인덱스는 1
        [SerializeField] private int materialIndex = 1;

        // (2) R, I, A, S, E, C 점수를 저장
        private readonly Dictionary<string, int> resultScores = new Dictionary<string, int>
        {
            { "R", 0 },
            { "I", 0 },
            { "A", 0 },
            { "S", 0 },
            { "E", 0 },
            { "C", 0 }
        };

        // (3) 각 문항이 어떤 항목에 해당하는지 매핑
        private readonly string[] questionMappings = new string[]
        {
            "R", "I", "A", "S", "E", "C", // 1-6
            "I", "S", "C", "E", "A", "R", // 7-12
            "C", "E", "S", "A", "I", "R", // 13-18
            "E", "A", "R", "I", "S", "C"  // 19-24
        };

        /// <summary>
        /// 모든 점수 및 응답을 초기화(재설문 또는 새로 시작하기 전 호출)
        /// </summary>
        public void ResetAllScores()
        {
            combinedResponses.Clear();
            foreach (var key in resultScores.Keys)
            {
                resultScores[key] = 0;
            }

            Debug.Log("[ResetAllScores] 점수 및 응답 초기화 완료");
        }

        /// <summary>
        /// 특정 QuestionBoard(인덱스로 식별)의 점수를 부분적으로 누적
        /// </summary>
        public void StoreBoardResult(int boardIndex)
        {
            // 범위 체크
            if (boardIndex < 0 || boardIndex >= questionBoards.Count)
            {
                Debug.LogWarning($"StoreBoardResult: 잘못된 boardIndex {boardIndex}");
                return;
            }

            var board = questionBoards[boardIndex];
            if (board == null)
            {
                Debug.LogWarning($"StoreBoardResult: questionBoards[{boardIndex}]가 null입니다.");
                return;
            }

            // 원본 Dictionary -> 사본으로 복사 (순회 중 수정 예외 방지)
            var responsesCopy = new Dictionary<int, int>(board.PlayerResponses);

            // 해당 보드 응답을 combinedResponses와 resultScores에 누적
            foreach (var pair in responsesCopy)
            {
                int questionIndex = pair.Key;      // 문항 인덱스
                int selectedOptionIndex = pair.Value; // (0~4)
                int globalIndex = questionIndex + boardIndex * questionMappings.Length;

                // 1) combinedResponses에 저장
                combinedResponses[globalIndex] = selectedOptionIndex;

                // 2) resultScores에 반영 (questionMappings에 따라)
                if (questionIndex >= 0 && questionIndex < questionMappings.Length)
                {
                    string category = questionMappings[questionIndex];
                    // 실제 점수 = 선택 인덱스(0~4) + 1 → (1~5)
                    resultScores[category] += (selectedOptionIndex + 1);
                }
            }

            Debug.Log($"[StoreBoardResult] QuestionBoard[{boardIndex}] 응답 누적 완료");
        }

        /// <summary>
        /// 최종 결과를 계산하고, tMP_Text에 출력 (가장 높은 항목에 대한 "나무 설명")
        /// </summary>
        public void CalculateFinalResult()
        {
            if (tMP_Text == null)
            {
                Debug.LogWarning("MagicalGardenResult: tMP_Text가 할당되지 않았습니다.");
                return;
            }

            DisplayHighestCategory();  // 최고 점수 카테고리를 찾아 UI 표시
        }

        /// <summary>
        /// 가장 높은 점수의 카테고리를 찾아, 해당 설명을 tMP_Text에 표시
        /// </summary>
        private void DisplayHighestCategory()
        {
            // 최고점 찾기
            string highestCategory = "";
            int highestScore = int.MinValue;

            foreach (var kvp in resultScores)
            {
                if (kvp.Value > highestScore)
                {
                    highestCategory = kvp.Key;
                    highestScore = kvp.Value;
                }
            }

            // 매칭되는 설명문을 가져옴
            string resultDescription = GetCategoryDescription(highestCategory);
            tMP_Text.text = resultDescription;

               // (추가) 머티리얼 색상 변경
            ApplyMaterialColorForCategory(highestCategory);

            Debug.Log($"[DisplayHighestCategory] 최고점: {highestCategory} ({highestScore}점)");
        }

        /// <summary>
        /// R, I, A, S, E, C 각각에 대한 설명 문자열을 반환
        /// </summary>
        private string GetCategoryDescription(string category)
        {
            return category switch
            {
                "R" => "결과로 대지의 나무, 가이아가 나왔군요! ...",
                "I" => "결과로 천체의 나무, 아스트룸이 나왔군요! ...",
                "A" => "결과로 새벽의 나무, 오로라가 나왔군요! ...",
                "S" => "결과로 온기의 나무, 아미카가 나왔군요! ...",
                "E" => "결과로 용기의 나무, 비르투스가 나왔군요! ...",
                "C" => "결과로 질서의 나무, 오르도가 나왔군요! ...",
                _   => "결과 없음"
            };
        }

               /// <summary>
        /// 최고점 카테고리에 따라 Material의 BaseColor 변경
        /// - MeshRenderer의 2번째(materialIndex=1) 머티리얼에 적용
        /// </summary>
        private void ApplyMaterialColorForCategory(string category)
        {
            // targetMeshRenderer가 할당되어 있지 않으면 무시
            if (targetMeshRenderer == null)
            {
                Debug.LogWarning("ApplyMaterialColorForCategory: targetMeshRenderer가 할당되지 않았습니다.");
                return;
            }

            // 머티리얼 배열 획득 (인스턴스화)
            Material[] mats = targetMeshRenderer.materials; 
            if (materialIndex < 0 || materialIndex >= mats.Length)
            {
                Debug.LogWarning($"ApplyMaterialColorForCategory: materialIndex({materialIndex})가 범위를 벗어났습니다.");
                return;
            }

            // 카테고리별 색상
            Color color = category switch
            {
                "R" => Color.red,                               // 빨강
                "I" => Color.green,                             // 초록
                "A" => Color.blue,                              // 파랑
                "S" => Color.yellow,                            // 노랑
                "E" => new Color(0.5f, 0.0f, 0.5f, 1.0f),       // 보라
                "C" => new Color(1.0f, 0.5f, 0.75f, 1.0f),      // 핑크
                _   => Color.white
            };

            // 2번째 머티리얼의 BaseColor 세팅
            // (URP/HDRP인 경우 "_BaseColor", Built-in Legacy인 경우 "_Color"일 수도 있으니 Shader에 맞춰 조정)
            mats[materialIndex].SetColor("_BaseColor", color);

            // 변경된 머티리얼 배열 다시 할당
            targetMeshRenderer.materials = mats;

            Debug.Log($"[ApplyMaterialColorForCategory] 카테고리={category}, 색상={color}");
        }
    }
}
