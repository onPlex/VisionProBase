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

        // 평가 결과 저장
        private readonly Dictionary<string, int> resultScores = new Dictionary<string, int>
        {
            { "R", 0 },
            { "I", 0 },
            { "A", 0 },
            { "S", 0 },
            { "E", 0 },
            { "C", 0 }
        };

        // 각 문항이 어떤 평가에 해당하는지 매핑
        private readonly string[] questionMappings = new string[]
        {
            "R", "I", "A", "S", "E", "C", // 1-6
            "I", "S", "C", "E", "A", "R", // 7-12
            "C", "E", "S", "A", "I", "R", // 13-18
            "E", "A", "R", "I", "S", "C"  // 19-24
        };

        private void OnEnable()
        {
            DisplayAllResponses();
        }

        /// <summary>
        /// 모든 QuestionBoard의 playerResponses를 순회하여 결과를 텍스트로 표시하고,
        /// R, I, A, S, E, C 점수를 계산하여 가장 높은 결과를 표시합니다.
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

            // 점수 초기화
            var updatedScores = new Dictionary<string, int>(resultScores);

            // QuestionBoard들을 차례대로 순회
            foreach (var qb in questionBoards)
            {
                if (qb == null) continue;

                // 각 QuestionBoard의 Dictionary (응답들)
                Dictionary<int, int> responses = qb.PlayerResponses;

                // 응답 딕셔너리 순회
                foreach (var pair in responses)
                {
                    int questionIndex = pair.Key;
                    int selectedOptionIndex = pair.Value;

                    // 안전 범위 체크
                    if (questionIndex >= 0 && questionIndex < questionMappings.Length)
                    {
                        string category = questionMappings[questionIndex];
                        // 선택된 옵션의 값을 점수에 합산 (1~5)
                        updatedScores[category] += selectedOptionIndex + 1;
                    }
                }
            }

            // 결과 출력
            string highestCategory = "";
            int highestScore = int.MinValue;

            foreach (var pair in updatedScores)
            {
                if (pair.Value > highestScore)
                {
                    highestCategory = pair.Key;
                    highestScore = pair.Value;
                }
            }

            string cont = highestCategory switch
            {
                "R" => "결과로 대지의 나무, 가이아가 나왔군요! 광활한 땅의 힘을 상징하는 이 나무는 어떤 환경에서도 흔들리지 않을 만큼 깊게 내린 뿌리와 튼튼한 줄기, 넓게 뻗은 잎사귀로 신뢰감을 줍니다. 현실적이고 실용적이며 성실하게 목표를 이루어 가는 성향을 가진 사람들에게 어울리는 나무죠. 가이아는 모든 이들에게 든든한 존재가 되어주는 아주 매력적인 나무랍니다.",
                "I" => "결과로 천체의 나무, 아스트룸이 나왔군요! 밤하늘의 신비로운 별빛을 담은 이 나무는 호기심 가득한 가지들이 하늘에 닿을 듯이 끝없이 뻗어 나갑니다. 논리적이고 창의적이며 지식을 탐구하기 좋아하는 성향을 가진 사람들에게 어울리는 나무죠. 아스트룸은 매일 조금씩 달라지는 별빛을 뿜어내며 사람들에게 지적 영감을 주는 아주 매력적인 나무랍니다.",
                "A" => "결과로 새벽의 나무, 오로라가 나왔군요! 새벽의 별처럼 형형색색의 빛을 띄고 있는 가지와, 창의적인 아이디어가 샘솟듯 잎사귀가 풍성하게 피어오른 이 나무는 예술적 영감을 줍니다. 상상력과 감수성이 풍부하며 새로운 시도를 즐기는 성향을 가진 사람들에게 어울리는 나무죠. 오로라는 사람들에게 예술적 영감을 주는 아주 매력적인 나무랍니다.",
                "S" => "결과로 온기의 나무, 아미카가 나왔군요! 따스한 봄날의 햇살처럼 은은한 빛이 흘러넘치며 풍성한 잎사귀로 주변을 감싸는 이 나무는 포근함을 줍니다. 타인과의 유대감을 소중히 여기고, 사람들에게 위로와 편안함을 제공하는 성향을 가진 사람들에게 어울리는 나무죠. 아미카는 언제나 온화하고 친근한 에너지를 주며, 주변 환경을 밝게 물들이는 아주 매력적인 나무입니다.",
                "E" => "결과로 용기의 나무, 비르투스가 나왔군요! 힘찬 기운으로 높이 솟아오르는 줄기와 강인한 잎사귀, 단단한 뿌리는 굳센 용기와 의지를 내비칩니다. 도전적이고, 리더십이 있으며, 목표를 향해 멈추지 않고 나아가는 사람들에게 어울리는 나무죠. 비르투스는 빛나는 성취의 길로 사람들을 이끄는 아주 매력적인 나무입니다.",
                "C" => "결과로 질서의 나무, 오르도가 나왔군요! 질서를 바탕으로 대칭적으로 뻗어나가는 가지와 강인한 뿌리와 잎사귀가 완벽한 조화를 이루는 이 나무는 평온함과 안정감을 줍니다. 늘 미리 준비하고 대비하며, 맡은 일을 꼼꼼하고 성실하게 수행하는 사람들에게 어울리는 나무죠. 오르도는 강한 책임감과 신뢰를 바탕으로 사람들에게 든든한 지원군이 되어주는 아주 매력적인 나무입니다.",
                _ => "결과 없음"
            };

            tMP_Text.text = cont;
        }
    }
}
