using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Text;

namespace YJH
{
    public class MagicalGardenResult : MonoBehaviour
    {
        //[SerializeField]
        //private TMP_Text tMP_Text;

        [Header("TMP")]
        // (추가) 나무 이름/특징용 TMP_Text
        [SerializeField] private TMP_Text treeNameText;
        [SerializeField] private TMP_Text treeFeatureText;

        // (추가) 흥미열매 6개의 "이름" 표기를 위한 TMP_Text 배열
        [SerializeField] private TMP_Text[] fruitNameTexts;

        // (추가) 흥미열매 6개의 "설명" 표기를 위한 TMP_Text 배열
        [SerializeField] private TMP_Text[] fruitDescTexts;

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

        // (3) [추가] 카테고리별 흥미열매 데이터
        //  - 한 카테고리에 6개씩 (여기서는 예시로 6개를 넣어둠)
        private readonly Dictionary<string, List<(string fruitName, string fruitDesc)>> fruitDataByCategory
            = new Dictionary<string, List<(string, string)>>
            {
                ["R"] = new List<(string, string)>
            {
                ("척척 열매",   "이 열매를 먹으면 \n 지금 바로 눈앞에 필요한 일을 척척 알아차릴 수 있어요."),
                ("튼튼 열매",   "이 열매를 먹으면 \n 몸이 강해지고 운동도 더 잘하게 돼요."),
                ("꼼꼼 열매",   "이 열매를 먹으면  \n작은 부분까지 잘 보고 꼼꼼하게 할 수 있어요."),
                ("자연 열매", "이 열매를 먹으면 \n 동물과 나무 같은 자연과 더 친해질 수 있어요."),
                ("손재주 열매", "이 열매를 먹으면 \n 손으로 무언가를 잘 만들고 다룰 수 있어요."),
                ("뚝딱 열매",   "이 열매를 먹으면 \n 로봇이나 기계를 더 잘 이해하고, 고칠 수 있게 돼요."),
            },
                ["I"] = new List<(string, string)>
            {
                ("생각 열매",      "이 열매를 먹으면 \n 생각을 또렷하게 정리할 수 있어요."),
                ("판단 열매",      "이 열매를 먹으면 \n 중요한 결정을 똑똑하게 내릴 수 있어요."),
                ("궁금 열매",    "이 열매를 먹으면 \n '이건 왜 그럴까?'하는 궁금한 마음이 자꾸 생겨요."),
                ("탐구 열매",  "이 열매를 먹으면 \n 궁금한 것을 깊이 파고들어 알아내는 힘이 생겨요."),
                ("분석 열매",      "이 열매를 먹으면 \n 복잡한 문제도 쉽게 이해할 수 있어요."),
                ("똑똑 열매",      "이 열매를 먹으면 \n 공부하는 게 더 재미있고 쉬워져요."),
            },
                ["A"] = new List<(string, string)>
            {
                ("예술 열매",    "이 열매를 먹으면 \n 그림이나 음악 같은 예술이 더 멋져 보여요."),
                ("번뜩 열매",    "이 열매를 먹으면 \n 새로운 아이디어가 쏙쏙 떠올라요."),
                ("느낌 열매",    "이 열매를 먹으면 \n 마음이 더 따뜻해지고 예쁜 것에 감동하게 돼요."),
                ("직감 열매",    "이 열매를 먹으면 \n 영감이 쏙 떠올라 무언가를 바로 표현할 수 있어요."),
                ("표현 열매",    "이 열매를 먹으면 \n 내가 느낀 것을 그림이나 글로 잘 표현할 수 있어요."),
                ("상상 열매",    "이 열매를 먹으면 \n 머릿속에서 멋진 상상이 펼쳐져요."),
            },
                ["S"] = new List<(string, string)>
            {
                ("친구 열매",    "이 열매를 먹으면 \n 친구들과 잘 어울리고 쉽게 친해질 수 있어요."),
                ("배려 열매",    "이 열매를 먹으면 \n 다른 사람을 더 잘 챙기고 이해할 수 있어요."),
                ("마음 열매",    "이 열매를 먹으면 \n 다른 사람의 생각과 기분을 더 잘 알 수 있어요."),
                ("봉사 열매",    "이 열매를 먹으면 \n 남을 돕는 일이 더 즐거워져요."),
                ("가르침 열매",  "이 열매를 먹으면 \n 친구나 동생에게 무언가를 가르쳐 주는 게 더 재미있어져요."),
                ("관계 열매",    "이 열매를 먹으면 \n 다른 사람들과 친해지고 신뢰를 쌓는 데 도움이 돼요."),
            },
                ["E"] = new List<(string, string)>
            {
                ("리더 열매",     "이 열매를 먹으면 \n 친구들을 이끄는 힘이 생겨요."),
                ("설득 열매",     "이 열매를 먹으면 \n 다른 사람을 내 생각에 동의하게 만드는 능력이 생겨요."),
                ("도전 열매",   "이 열매를 먹으면 \n 어려운 일에도 용감하게 도전할 수 있어요."),
                ("목표 열매", "이 열매를 먹으면 \n 내가 세운 목표를 열심히 이루고 싶어져요."),
                ("승부 열매",     "이 열매를 먹으면 \n 다른 사람과 경쟁하면서 더 열심히 하게 돼요."),
                ("성취 열매",     "이 열매를 먹으면 \n 무언가를 계획하고 이루는 능력이 좋아져요."),
            },
                ["C"] = new List<(string, string)>
            {
                ("책임 열매",    "이 열매를 먹으면 \n 맡은 일을 끝까지 해내려는 마음이 생겨요."),
                ("계획 열매",    "이 열매를 먹으면 \n 하고 싶은 일들을 잘 계획할 수 있어요."),
                ("성실 열매",    "이 열매를 먹으면 \n 꾸준히 노력하는 성실한 마음이 생겨요."),
                ("맞춤 열매",    "이 열매를 먹으면 \n 주변 상황에 맞춰 잘 적응하고 협력할 수 있어요."),
                ("안정 열매",    "이 열매를 먹으면 \n 안정적이고 편안하게 행동하고 싶어져요."),
                ("정리 열매",    "이 열매를 먹으면 \n 서류나 정리 같은 일을 잘할 수 있어요."),
            },
            };

        // 나무 이름/특징만 반환하는 예시(튜플 활용)
        private (string treeName, string treeFeature) GetTreeInfo(string category)
        {
            switch (category)
            {
                case "R":
                    return ("대지의 나무, 가이아", "광활한 땅의 힘을 상징하는 이 나무는 어떤 환경에서도 흔들리지 않을 만큼 깊게 내린 뿌리와 튼튼한 줄기, 넓게 뻗은 잎사귀로 신뢰감을 줍니다. \n 현실적이고 실용적이며 성실하게 목표를 이루어 가는 성향을 가진 사람들에게 어울리는 나무죠. \n 가이아는 모든 이들에게 든든한 존재가 되어주는 아주 매력적인 나무랍니다.");
                case "I":
                    return ("천체의 나무, 아스트룸", "밤하늘의 신비로운 별빛을 담은 이 나무는 호기심 가득한 가지들이 하늘에 닿을 듯이 끝없이 뻗어나갑니다. \n 논리적이고 창의적이며 지식을 탐구하기 좋아하는 성향을 가진 사람들에게 어울리는 나무죠. \n 아스트룸은 매일 조금씩 달라지는 별빛을 뿜어내며 사람들에게 지적 영감을 주는 아주 매력적인 나무랍니다.");
                case "A":
                    return ("새벽의 나무, 오로라", "새벽의 별처럼 형형색색의 빛을 띄고 있는 가지와, 창의적인 아이디어가 샘솟듯 잎사귀가 풍성하게 피어오른 이 나무는 예술적 영감을 줍니다. \n 상상력과 감수성이 풍부하며 새로운 시도를 즐기는 성향을 가진 사람들에게 어울리는 나무죠. \n 오로라는 사람들에게 예술적 영감을 주는 아주 매력적인 나무랍니다.");
                case "S":
                    return ("온기의 나무, 아미카", "따스한 봄날의 햇살처럼 은은한 빛이 흘러넘치며 풍성한 잎사귀로 주변을 감싸는 이 나무는 포근함을 줍니다. \n 타인과의 유대감을 소중히 여기고, 사람들에게 위로와 편안함을 제공하는 성향을 가진 사람들에게 어울리는 나무죠. \n 아미카는 언제나 온화하고 친근한 에너지를 주며, 주변 환경을 밝게 물들이는 아주 매력적인 나무입니다.");
                case "E":
                    return ("용기의 나무, 비르투스", "힘찬 기운으로 높이 솟아오르는 줄기와 강인한 잎사귀, 단단한 뿌리는 굳센 용기와 의지를 내비칩니다. \n 도전적이고, 리더십이 있으며, 목표를 향해 멈추지 않고 나아가는 사람들에게 어울리는 나무죠. \n 비르투스는 빛나는 성취의 길로 사람들을 이끄는 아주 매력적인 나무입니다.");
                case "C":
                    return ("질서의 나무, 오르도", "질서를 바탕으로 대칭적으로 뻗어나가는 가지와 강인한 뿌리와 잎사귀가 완벽한 조화를 이루는 이 나무는 평온함과 안정감을 줍니다. \n 늘 미리 준비하고 대비하며, 맡은 일을 꼼꼼하고 성실하게 수행하는 사람들에게 어울리는 나무죠. \n 오르도는 강한 책임감과 신뢰를 바탕으로 사람들에게 든든한 지원군이 되어주는 아주 매력적인 나무입니다");
                default:
                    return ("결과 없음", "해당 결과의 나무 특징 설명 없음");
            }
        }

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
            // if (tMP_Text == null)
            // {
            //     Debug.LogWarning("MagicalGardenResult: tMP_Text가 할당되지 않았습니다.");
            //     return;
            // }

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

            // (1) 먼저 "나무 이름/특징" 가져오기
            var (treeName, treeFeature) = GetTreeInfo(highestCategory);

            // (2) 흥미열매 6개 가져오기
            List<(string fruitName, string fruitDesc)> fruits;
            if (!fruitDataByCategory.TryGetValue(highestCategory, out fruits))
            {
                // 만약 매핑되지 않은 카테고리일 경우 빈 리스트 할당
                fruits = new List<(string, string)>();
            }

            // (3) 각각의 TMP_Text에 대입
            treeNameText.text = treeName;       // 나무 이름
            treeFeatureText.text = treeFeature; // 나무 특징

            // 흥미열매 (이름, 설명) 6개를 각각 대입
            for (int i = 0; i < fruits.Count && i < fruitNameTexts.Length; i++)
            {
                fruitNameTexts[i].text = fruits[i].fruitName;
                fruitDescTexts[i].text = fruits[i].fruitDesc;
            }
            // 남은 Text가 있다면 빈 칸 처리 (예: 결과가 6개 미만이거나, Text가 더 많을 경우)
            for (int i = fruits.Count; i < fruitNameTexts.Length; i++)
            {
                fruitNameTexts[i].text = "";
                fruitDescTexts[i].text = "";
            }


            // (추가) 머티리얼 색상 변경
            ApplyMaterialColorForCategory(highestCategory);

            Debug.Log($"[DisplayHighestCategory] 최고점: {highestCategory} ({highestScore}점)");


            SendDummyResultByHighestCategory(highestCategory);

        }

        /// <summary>
        /// 최고점 카테고리에 따라 임시로 설정된 데이터로 SendGameResult 전송
        /// </summary>
        private void SendDummyResultByHighestCategory(string highestCategory)
        {
            // 예시로 사용하는 더미 데이터
            string contData = "마법 정원 기본 설명";
            int imgTypeData = -1;
            int statusData = 0;

            switch (highestCategory)
            {
                case "R":
                    contData = "결과로 대지의 나무, 가이아가 나왔군요! 광활한 땅의 힘을 상징하는 이 나무는 어떤 환경에서도 흔들리지 않을 만큼 깊게 내린 뿌리와 튼튼한 줄기, 넓게 뻗은 잎사귀로 신뢰감을 줍니다. 현실적이고 실용적이며 성실하게 목표를 이루어 가는 성향을 가진 사람들에게 어울리는 나무죠. 가이아는 모든 이들에게 든든한 존재가 되어주는 아주 매력적인 나무랍니다.";
                    imgTypeData = 98;
                    statusData = 1;
                    break;
                case "I":
                    contData = "결과로 천체의 나무, 아스트룸이 나왔군요! 밤하늘의 신비로운 별빛을 담은 이 나무는 호기심 가득한 가지들이 하늘에 닿을 듯이 끝없이 뻗어 나갑니다. 논리적이고 창의적이며 지식을 탐구하기 좋아하는 성향을 가진 사람들에게 어울리는 나무죠. 아스트룸은 매일 조금씩 달라지는 별빛을 뿜어내며 사람들에게 지적 영감을 주는 아주 매력적인 나무랍니다.";
                    imgTypeData = 99;
                    statusData = 1;
                    break;
                case "A":
                    contData = "결과로 새벽의 나무, 오로라가 나왔군요! 새벽의 별처럼 형형색색의 빛을 띄고 있는 가지와, 창의적인 아이디어가 샘솟듯 잎사귀가 풍성하게 피어오른 이 나무는 예술적 영감을 줍니다. 상상력과 감수성이 풍부하며 새로운 시도를 즐기는 성향을 가진 사람들에게 어울리는 나무죠. 오로라는 사람들에게 예술적 영감을 주는 아주 매력적인 나무랍니다.";
                    imgTypeData = 100;
                    statusData = 1;
                    break;
                case "S":
                    contData = "결과로 온기의 나무, 아미카가 나왔군요! 따스한 봄날의 햇살처럼 은은한 빛이 흘러넘치며 풍성한 잎사귀로 주변을 감싸는 이 나무는 포근함을 줍니다. 타인과의 유대감을 소중히 여기고, 사람들에게 위로와 편안함을 제공하는 성향을 가진 사람들에게 어울리는 나무죠. 아미카는 언제나 온화하고 친근한 에너지를 주며, 주변 환경을 밝게 물들이는 아주 매력적인 나무입니다.";
                    imgTypeData = 101;
                    statusData = 1;
                    break;
                case "E":
                    contData = "결과로 용기의 나무, 비르투스가 나왔군요! 힘찬 기운으로 높이 솟아오르는 줄기와 강인한 잎사귀, 단단한 뿌리는 굳센 용기와 의지를 내비칩니다. 도전적이고, 리더십이 있으며, 목표를 향해 멈추지 않고 나아가는 사람들에게 어울리는 나무죠. 비르투스는 빛나는 성취의 길로 사람들을 이끄는 아주 매력적인 나무입니다.";
                    imgTypeData = 102;
                    statusData = 1;
                    break;
                case "C":
                    contData = "결과로 질서의 나무, 오르도가 나왔군요! 질서를 바탕으로 대칭적으로 뻗어나가는 가지와 강인한 뿌리와 잎사귀가 완벽한 조화를 이루는 이 나무는 평온함과 안정감을 줍니다. 늘 미리 준비하고 대비하며, 맡은 일을 꼼꼼하고 성실하게 수행하는 사람들에게 어울리는 나무죠. 오르도는 강한 책임감과 신뢰를 바탕으로 사람들에게 든든한 지원군이 되어주는 아주 매력적인 나무입니다.";
                    imgTypeData = 103;
                    statusData = 1;
                    break;
                default:
                    contData = "알 수 없는 카테고리 - 임시 설명 텍스트";
                    imgTypeData = 999;
                    statusData = 1;
                    break;
            }

            // 실제 전송 TODO:: Save Game Result Data 
            //sendResultData.SendGameResult(contData, imgTypeData, statusData);
        }


        /// <summary>
        /// 특정 열매(fruitIndex)를 클릭했을 때,
        /// treeFeatureText 에 해당 열매의 '설명'을 표시한다.
        /// </summary>
        public void ShowFruitDescription(int fruitIndex)
        {
            // 안전 검사
            if (fruitDescTexts == null || fruitIndex < 0 || fruitIndex >= fruitDescTexts.Length)
            {
                Debug.LogWarning($"ShowFruitDescription: 유효하지 않은 fruitIndex {fruitIndex}");
                return;
            }

            // 열매 설명을 treeFeatureText 영역에 표시
            treeFeatureText.text = fruitDescTexts[fruitIndex].text;
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
                _ => Color.white
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
