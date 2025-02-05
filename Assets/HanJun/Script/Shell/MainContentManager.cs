using UnityEngine;
using TMPro;

namespace YJH
{


    public class MainContentManager : MonoBehaviour
    {
        [Header("Phases")]
        // 0~5까지의 Phase Parent 오브젝트
        [SerializeField] private GameObject[] phaseParents = new GameObject[6];

        // 현재 진행중인 Phase 인덱스(0~5)
        private int currentPhase = 0;



        // Career 선택 카운트 (R=0, I=1, A=2, S=3, E=4, C=5)
        private int[] careerSelectedCounts = new int[6];

        [Header("UI")]
        [SerializeField]
        TMP_Text TMP_PhaseText;


        [Header("Result")]
        [SerializeField] Jun.Dolphin dolphin;
        [SerializeField] GameObject Basket;
        [SerializeField] GameObject ShellCountUI;

        public ShellInfo.CareerType finalCareer;
        //[Header("API")]

        //[SerializeField]
        //SendResultData sendResultData;
        private void Start()
        {
            // 초기 셋업
            //InitializePhases();
        }

        /// <summary>
        /// 모든 PhaseParent를 비활성화 후, 현재 phaseParent만 활성화
        /// </summary>
        private void InitializePhases()
        {
            // 전체 PhaseParent 비활성화
            for (int i = 0; i < phaseParents.Length; i++)
            {
                if (phaseParents[i] != null)
                    phaseParents[i].SetActive(false);
            }
            // 현재 Phase만 활성화
            ActivateCurrentPhase();
        }

        /// <summary>
        /// phaseParents[currentPhase]만 활성화
        /// </summary>
        private void ActivateCurrentPhase()
        {
            if (currentPhase < 0 || currentPhase >= phaseParents.Length)
            {
                Debug.LogWarning("Phase 범위 에러");
                return;
            }

            if (phaseParents[currentPhase] != null)
                phaseParents[currentPhase].SetActive(true);

            // currentPhase는 0~5이므로, 실제 표기는 1~6이 되도록 (currentPhase + 1)
            if (TMP_PhaseText != null)
            {
                TMP_PhaseText.text = $"{(currentPhase)}/{phaseParents.Length}";
            }
        }

        /// <summary>
        /// ShellBasket 등에서 Shell이 선택되었다고 알림받으면,
        /// 해당 Shell의 CareerType을 카운트하고 다음 Phase로 넘어감
        /// </summary>
        public void RegisterShellSelection(int careerIndex)
        {
            if (careerIndex < 0 || careerIndex >= 6)
            {
                Debug.LogWarning("Invalid careerIndex");
                return;
            }

            // 카운트 증가
            careerSelectedCounts[careerIndex]++;

            // 다음 Phase
            NextPhase();
        }

        /// <summary>
        /// Phase 1 증가 후, 끝났으면 결과 산출
        /// </summary>
        private void NextPhase()
        {
            // 현재 PhaseParent 비활성화
            if (phaseParents[currentPhase] != null)
                phaseParents[currentPhase].SetActive(false);

            currentPhase++;

            if (currentPhase < phaseParents.Length)
            {
                // 새 Phase 활성화
                ActivateCurrentPhase();
            }
            else
            {
                // 모든 Phase 끝났으면 결과 계산
                CalculateFinalCareer();

                // 결과 Phase로 이동
                GoToResultPhase();
            }
        }

        /// <summary>
        /// R,I,A,S,E,C 중 최다 선택된 항목 판별
        /// </summary>
        private void CalculateFinalCareer()
        {
            int maxIndex = 0;
            for (int i = 1; i < 6; i++)
            {
                if (careerSelectedCounts[i] > careerSelectedCounts[maxIndex])
                    maxIndex = i;
            }
            finalCareer = (ShellInfo.CareerType)maxIndex;
            Debug.Log($"가장 많이 선택된 유형: {finalCareer}");
            // TODO: 결과 UI 표시, 씬 전환 등

            // 추가: 최종 결과를 SendResultData로 전달 (임시/더미 값 예시)
            // if (sendResultData != null)
            // {
            //     SendFinalCareerResult(finalCareer);
            // }
            // else
            // {
            //     Debug.LogWarning("[CalculateFinalCareer] sendResultData가 할당되어 있지 않습니다.");
            // }

            SendFinalCareerResult(finalCareer);
        }

        private void GoToResultPhase()
        {
            // TODO: 결과 화면(Phase) 활성화, 씬 전환, UI 표시 등
            // ex) resultPhaseObject.SetActive(true);
            // ex) SceneManager.LoadScene("ResultScene");
            // ...

            Basket.SetActive(false);
            ShellCountUI.SetActive(false);
            dolphin.gameObject.SetActive(true);
            if (dolphin) dolphin.PlayAnimation();
        }

        /// <summary>
        /// finalCareer(최고 선택된 항목)에 따른 더미 데이터 전송
        /// </summary>
        private void SendFinalCareerResult(ShellInfo.CareerType career)
        {
            // 더미 데이터 (컨텐츠, 이미지타입, 상태코드)
            string contData = "기본 더미 내용";
            int imgTypeData = -1;
            int statusData = 0;

            // career 인덱스: R=0, I=1, A=2, S=3, E=4, C=5
            switch (career)
            {
                case ShellInfo.CareerType.Realistic:
                    contData = "뚝딱진주 : 솔직하고 성실한 성격을 가지고 있고, 몸을 많이 움직이는 활동을 좋아해요. 새로운 아이디어를 생각하기 보다는 기계나 도구를 다루는 일을 좋아해요. 나만의 텃밭 가꾸기, DIY 활동, 요리 활동을 추천해요.";
                    imgTypeData = 1;
                    statusData = 1;
                    break;
                case ShellInfo.CareerType.Investigative:
                    contData = "궁금진주 : 호기심이 많고 독립적이며 새로운 정보를 알아가는 것을 좋아해요. 세심히 관찰하며 문제를 창의적으로 해결하는 데 흥미를 느끼고, 끊임없이 배우고 탐구하는 것을 즐겨요. 챗GPT 같은 대화형 인공지능 서비스를 활용하여 학교나 학원에서 배운 내용에 대한 새로운 정보를 탐색해 보세요.";
                    imgTypeData = 102;
                    statusData = 1;
                    break;
                case ShellInfo.CareerType.Artistic:
                    contData = "상상진주 : 상상력이 많고 감정이 풍부한 성격을 가지고 있어요. 개성 있는 독특한 방법으로 스스로를 표현하는 것을 좋아하죠. 글쓰기, 음악, 미술을 좋아하고 나만의 세계로 빠져드는 것을 즐겨요. 이런 활동들을 통해 나만의 작품을 만들거나, 노래/춤/연기 등 예술활동을 할 수 있는 동아리에 참여해 보세요. ";
                    imgTypeData = 3;
                    statusData = 1;
                    break;
                case ShellInfo.CareerType.Social:
                    contData = "친절진주 : 이해심 많고 사교적인 성격을 가지고 있어요. 어려운 상황에 처한 사람을 도와주는 것을 좋아하죠. 친구들과 사이좋게 지내고 함께 활동하는 것을 즐겨요. 어려운 이웃을 보살피는 봉사활동에 꾸준히 참여해 보세요.";
                    imgTypeData = 4;
                    statusData = 1;
                    break;
                case ShellInfo.CareerType.Enterprising:
                    contData = "열정진주 : 사람들과 잘 어울리고 리더십 있는 성격이에요. 친구들을 설득하고 이끄는 것을 좋아해요. 말을 자신 있게 잘하고 모든 일에 열심히 참여하는 편이에요. 학급 행사와 같은 행사를 직접 계획하고 사람들을 모아 실행해 보세요.";
                    imgTypeData = 5;
                    statusData = 1;
                    break;
                case ShellInfo.CareerType.Conventional:
                    contData = "꼼꼼진주 : 책임감이 강하고 정직한 성격이에요. 스스로 계획을 세우고 꾸준히 실천하는 것을 좋아해요. 친구들과의 약속이나 학교 규칙, 질서를 잘 지켜요. 하루, 일주일, 한 달 등 기간별로 계획을 세우고 실천해 보세요.";
                    imgTypeData = 6;
                    statusData = 1;
                    break;
            }

            // 실제 전송
            //sendResultData.SendGameResult(contData, imgTypeData, statusData);
            ResultDataStorage.Instance.Game1ContData = contData;
            ResultDataStorage.Instance.Game1ImgTypeData = imgTypeData;
            ResultDataStorage.Instance.Game1StatusData = statusData;
        }

    }
}