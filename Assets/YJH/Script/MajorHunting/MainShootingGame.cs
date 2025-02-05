using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

namespace YJH.MajorHunting
{
    [System.Serializable]
    public struct MissionData
    {
        public string name; // 미션 이름
        public string desc; // 미션 설명

        public MissionData(string name, string desc)
        {
            this.name = name;
            this.desc = desc;
        }
    }


    public class MainShootingGame : MonoBehaviour
    {
        // PhaseManager로부터 전달된 2개 ID (Round1, Round2)
        [SerializeField]
        PhaseManager phaseManager;
        private string round1ID;
        private string round2ID;

        // 현재 라운드 (1 또는 2)
        private int currentRound = 1;

        // 60초 카운트다운
        private float timeLeft = 60f;
        private bool isGameRunning = false;  // 카운트다운 진행 여부

        //총알 관련
        private int bulletCount = 5; // 남은 총알 수
        [SerializeField]
        private GameObject[] bulletIndicators; // 씬에 배치된 5개 총알 UI

        // ▼ 라운드 내에 맞춘 정답 개수 추적
        private int correctCountInRound = 0;
        [Header("UI")]
        [SerializeField]
        TMP_Text MissionList1Name;
        [SerializeField]
        TMP_Text MissionList2Name;
        [SerializeField]
        TMP_Text MissionList3Name;
        [SerializeField]
        TMP_Text MissionList1Dec;
        [SerializeField]
        TMP_Text MissionList2Dec;
        [SerializeField]
        TMP_Text MissionList3Dec;
        [SerializeField]
        TMP_Text TimerText;

        [Header("PopUp")]
        [SerializeField]
        GameObject FailPopUp;
        [SerializeField]
        GameObject countdownPopup;
        [SerializeField]
        TMP_Text countdownText;
        [SerializeField]
        GameObject successPopup;

        [Header("Shooting Targets (9개 버튼)")]
        [SerializeField] private List<ShootingTarget> shootingTargets;

        [Header("Result")]
        [SerializeField] private ResultManager resultManager;

        private void OnEnable()
        {
            List<string> selectedIDs = phaseManager.GetSelectedButtonIDs();
            if (selectedIDs.Count >= 2)
            {
                round1ID = selectedIDs[0];
                round2ID = selectedIDs[1];
                InitGameEnvironment(round1ID, round2ID);
            }
            else
            {
                Debug.LogWarning("선택된 버튼이 2개 미만입니다. 기본값으로 설정합니다.");
                round1ID = "robot";
                round2ID = "bio";
            }

        }

        /// <summary>
        /// PhaseManager에서 2개의 선택된 button ID를 받아,
        /// 라운드1, 라운드2 환경을 준비한다.
        /// </summary>
        public void InitGameEnvironment(string firstSelectedID, string secondSelectedID)
        {
            round1ID = firstSelectedID;
            round2ID = secondSelectedID;

            currentRound = 1;
            SetupRoundEnvironment(currentRound, round1ID);
            StartRound();

            // --- 총알 5발은 게임 전체 통합이므로, 여기서 한 번만 초기화 ---
            bulletCount = 5;
            ResetBulletUI(); // 5개 총알 UI 활성화
                             // 라운드 타이머 등 리셋
            timeLeft = 60f;
            correctCountInRound = 0;
            isGameRunning = true; // 라운드 시작
        }

        private void ResetBulletUI()
        {
            if (bulletIndicators != null)
            {
                for (int i = 0; i < bulletIndicators.Length; i++)
                {
                    if (bulletIndicators[i] != null)
                        bulletIndicators[i].SetActive(true);
                }
            }
        }

        private void AssignMissionsToShootingTargets(MissionData[] correctDatas, MissionData[] wrongDatas)
        {
            if (shootingTargets == null || shootingTargets.Count < 9)
            {
                Debug.LogWarning("shootingTargets가 9개 미만!");
                return;
            }

            // 정답 3개 (이름만 추출)
            List<string> correctList = new List<string>();
            foreach (var c in correctDatas)
            {
                correctList.Add(c.name);
            }

            // 오답 6개 (이름만 추출)
            List<string> wrongList = new List<string>();
            foreach (var w in wrongDatas)
            {
                wrongList.Add(w.name);
            }

            // 합치기(총 9개)
            List<string> totalMissions = new List<string>();
            totalMissions.AddRange(correctList); // 3개
            totalMissions.AddRange(wrongList);   // 6개 → 총 9개

            // 셔플
            Shuffle(totalMissions);

            // ShootingTarget에 대입
            for (int i = 0; i < 9; i++)
            {
                shootingTargets[i].SetMissionName(totalMissions[i]);
            }
        }

        private void Shuffle(List<string> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                int rnd = Random.Range(i, list.Count);
                var tmp = list[i];
                list[i] = list[rnd];
                list[rnd] = tmp;
            }
        }

        private void Update()
        {
            if (!isGameRunning) return;

            timeLeft -= Time.deltaTime;
            if (timeLeft <= 0f)
            {
                OnFail("시간 초과");
            }
            else
            {
                TimerText.text = $"Timer : {Mathf.CeilToInt(timeLeft)}초";
            }
        }

        /// <summary>
        /// 라운드 시작 세팅: 시간/총알 리셋, UI 갱신, isGameRunning = true
        /// </summary>
        private void StartRound()
        {
            timeLeft = 60f;
            bulletCount = 5;
            isGameRunning = true;

            // 총알 UI 초기화 (5개 모두 활성화)
            if (bulletIndicators != null)
            {
                for (int i = 0; i < bulletIndicators.Length; i++)
                {
                    if (bulletIndicators[i] != null)
                        bulletIndicators[i].SetActive(true);
                }
            }

            Debug.Log($"[MainShootingGame] Round {currentRound} 시작 - 60초 카운트다운, 총알 5발");
        }


        private void SetupRoundEnvironment(int round, string mapID)
        {
            Debug.Log($"[MainShootingGame] Round {round}, Map: {mapID}");

            // 1) 정답 미션 3개 가져오기
            if (!missionDatabase.TryGetValue(mapID, out MissionData[] correctMissions))
            {
                Debug.LogError($"[SetupRoundEnvironment] missionDatabase에 '{mapID}' 없음");
                return;
            }

            // 2) 오답 미션 6개 가져오기
            if (!wrongMissionDatabase.TryGetValue(mapID, out MissionData[] wrongMissions))
            {
                Debug.LogError($"[SetupRoundEnvironment] wrongMissionDatabase에 '{mapID}' 없음");
                return;
            }

            // 3) UI에 표시할 3개 미션 업데이트
            UpdateMissionListUI(mapID);

            // 4) ShootingTarget들에 '정답 3개 + 오답 6개' 섞어서 분배
            AssignMissionsToShootingTargets(correctMissions, wrongMissions);
        }


        /// <summary>
        /// 플레이어가 적을 격파/목표 달성 등으로 라운드를 클리어했을 때 호출
        /// </summary>
        public void OnRoundClear()
        {
            isGameRunning = false;
            Debug.Log($"[MainShootingGame] Round {currentRound} 클리어, 3개 정답 맞춤!");

            // 모든 ShootingTarget을 초기 상태로 리셋
            foreach (var target in shootingTargets)
            {
                if (target != null)
                {
                    target.ResetEffect(); // NormalBoard 활성, Green/Red 비활성
                }
            }

            if (currentRound == 1)
            {
                // 라운드1 클리어 → "카운트다운 팝업" 표시 및 4초간 카운트다운
                if (countdownPopup != null)
                {
                    countdownPopup.SetActive(true);
                    StartCoroutine(Co_ShowCountdown()); // 카운트다운 시작
                }
            }
            else
            {
                // 라운드2 클리어 → "성공 팝업" 표시, 게임 종료
                if (successPopup != null)
                {
                    successPopup.SetActive(true);
                    resultManager.ShowFinalResults();
                }
            }
        }


        private void StoreCorrectAnswer(string jobName)
        {
            string mapID = currentRound == 1 ? round1ID : round2ID;

            if (missionDatabase.TryGetValue(mapID, out MissionData[] missions))
            {
                foreach (var mission in missions)
                {
                    if (mission.name == jobName)
                    {
                        // 🎯 정답을 ResultManager에 저장 (라운드 정보 포함)
                        string major = GetMajorForJob(jobName);
                        string majorDesc = GetMajorDescription(major);
                        resultManager.AddCorrectAnswer(currentRound, mission.name, mission.desc, major, majorDesc);
                        return; // 저장 후 종료
                    }
                }
            }

            Debug.LogWarning($"StoreCorrectAnswer: '{jobName}'에 대한 정보를 찾을 수 없습니다.");
        }
        private IEnumerator Co_ShowCountdown()
        {
            if (countdownPopup != null)
            {
                countdownText = countdownPopup.GetComponentInChildren<TMP_Text>();
                if (countdownText != null)
                {
                    countdownText.text = "3"; // 3초 남음
                    yield return new WaitForSeconds(1f);

                    countdownText.text = "2"; // 2초 남음
                    yield return new WaitForSeconds(1f);

                    countdownText.text = "1"; // 1초 남음
                    yield return new WaitForSeconds(1f);

                    countdownText.text = "Start!"; // 시작!
                    yield return new WaitForSeconds(1f);
                }
            }

            // 카운트다운 종료 후 팝업 닫기 및 Round2 시작
            if (countdownPopup != null)
            {
                countdownPopup.SetActive(false);
            }

            // Round2 설정
            currentRound = 2;
            correctCountInRound = 0;

            // 라운드2 타이머 재설정
            timeLeft = 60f;

            // "isGameRunning" 다시 활성화 (Round2 진행 시작)
            SetupRoundEnvironment(currentRound, round2ID);
            isGameRunning = true;
        }

        /// <summary>
        /// 라운드1 클리어 후 3초 대기 → 라운드2 세팅
        /// </summary>
        private IEnumerator Co_ProceedRound2AfterDelay(float seconds)
        {
            yield return new WaitForSeconds(seconds);

            // 카운트다운 팝업 닫기
            if (countdownPopup != null)
            {
                countdownPopup.SetActive(false);
            }

            // Round2 설정
            currentRound = 2;
            correctCountInRound = 0;

            // 라운드2 타이머 재설정
            timeLeft = 60f;

            // "isGameRunning" 다시 활성화 (Round2 진행 시작)
            SetupRoundEnvironment(currentRound, round2ID);
            isGameRunning = true;
        }

        /// <summary>
        /// 총알 1발 소모
        /// </summary>
        public void ConsumeBullet()
        {
            // 이미 게임이 진행 중이 아니면 무시
            if (!isGameRunning) return;

            if (bulletCount > 0)
            {
                bulletCount--;

                // BulletIndicators[ bulletCount ]를 비활성화
                // (예: bulletCount가 4 → 4번 인덱스 비활성화)
                if (bulletIndicators != null && bulletCount >= 0 && bulletCount < bulletIndicators.Length)
                {
                    if (bulletIndicators[bulletCount] != null)
                    {
                        bulletIndicators[bulletCount].SetActive(false);
                    }
                }

                Debug.Log($"총알 사용: 남은 총알 = {bulletCount}");

                if (bulletCount <= 0)
                {
                    // 총알 소진 → 실패
                    OnFail("총알 소진");
                }
            }
        }

        /// <summary>
        /// 시간 초과 / 탄약 소진 / 기타 이유로 게임 실패 처리
        /// </summary>
        private void OnFail(string reason)
        {
            isGameRunning = false;
            Debug.Log($"[MainShootingGame] 게임 실패: {reason}");

            if (FailPopUp != null)
            {
                FailPopUp.SetActive(true);
            }
        }


        /// <summary>
        /// Retry button---Onclick 
        /// </summary>
        public void RetryGame()
        {
            // 1) 모든 ShootingTarget을 초기 상태로 되돌림
            foreach (var target in shootingTargets)
            {
                if (target != null)
                {
                    target.ResetEffect(); // NomalBoard 활성, Green/Red 비활성
                }
            }

            // 2) FailPopUp 닫기
            if (FailPopUp != null)
            {
                FailPopUp.SetActive(false);
            }

            // 3) 현재 라운드에 따라 다시 환경 세팅 + StartRound
            if (currentRound == 1)
            {
                resultManager.ClearAllAnswers();
                SetupRoundEnvironment(1, round1ID);
                StartRound();
            }
            else
            {
                resultManager.ClearRoundAnswers(2);
                SetupRoundEnvironment(2, round2ID);
                StartRound();
            }
        }

        // NEW: buttonID에 해당하는 미션 데이터를 찾아 UI에 표시
        private void UpdateMissionListUI(string buttonID)
        {
            if (missionDatabase.TryGetValue(buttonID, out MissionData[] missions))
            {
                // 3개 미션을 순서대로 UI에 반영
                // (missions.Length가 3 미만이라면 안전장치 필요)
                if (missions.Length >= 3)
                {
                    MissionList1Name.text = missions[0].name;
                    MissionList1Dec.text = missions[0].desc;

                    MissionList2Name.text = missions[1].name;
                    MissionList2Dec.text = missions[1].desc;

                    MissionList3Name.text = missions[2].name;
                    MissionList3Dec.text = missions[2].desc;
                }
                else
                {
                    Debug.LogWarning($"{buttonID}에 3개 미션 정보가 충분하지 않습니다.");
                }
            }
            else
            {
                Debug.LogWarning($"Mission Database에 {buttonID} 키가 존재하지 않습니다.");
                // 미션 UI를 비우거나 기본값으로 처리
                MissionList1Name.text = "(None)";
                MissionList1Dec.text = "(None)";

                MissionList2Name.text = "(None)";
                MissionList2Dec.text = "(None)";

                MissionList3Name.text = "(None)";
                MissionList3Dec.text = "(None)";
            }
        }

        public void OnClickMissionButton(string missionName, ShootingTarget target)
        {
            if (!isGameRunning)
            {
                Debug.Log("게임 진행 중이 아니므로 클릭을 무시합니다.");
                return;
            }

            // 현재 화면에 표시된 미션 명(3개) 중 하나와 일치하면 정답, 아니면 오답
            // MissionListXName.text 에 표시된 문자열과 비교
            bool isCorrect = false;
            if (missionName == MissionList1Name.text
                || missionName == MissionList2Name.text
                || missionName == MissionList3Name.text)
            {
                // 정답
                isCorrect = true;
            }

            if (isCorrect)
            {
                StoreCorrectAnswer(missionName);

                // 정답: 총알 소모하지 않는다
                target.PlayCorrectEffect();
                correctCountInRound++;

                // 3개 정답 모두 맞추면 라운드 클리어
                if (correctCountInRound >= 3)
                {
                    OnRoundClear();
                }
            }
            else
            {
                // 오답: 총알 소모
                target.PlayWrongEffect();
                ConsumeBullet();
            }
        }

        private Dictionary<string, string> jobToMajorMapping = new Dictionary<string, string>
{
    { "로봇디자이너", "산업디자인학과" },
    { "인공지능전문가", "인공지능공학과" },
    { "드론윤리학자", "법학과" },
    { "생의학엔지니어", "의료공학과" },
    { "나노의약품전문가", "약학과" },
    { "유전상담사", "심리학과" },
    { "빅데이터전문가", "통계학과" },
    { "도시계획가", "도시계획학과" },
    { "무인자동차엔지니어", "자동차공학과" },
    { "반려동물훈련상담사", "동물자원학과" },
    { "디지털큐레이터", "미디어커뮤니케이션학과" },
    { "스포츠심리상담원", "체육학과" }
};

        private string GetMajorForJob(string jobName)
        {
            return jobToMajorMapping.ContainsKey(jobName) ? jobToMajorMapping[jobName] : "관련 학과 없음";
        }

        private Dictionary<string, string> majorDescriptions = new Dictionary<string, string>
{
    { "산업디자인학과", "가전제품, 전자기기, 생활용품 등 다양한 제품 디자인 인재를 양성한다." },
    { "인공지능공학과", "AI 개발에 필요한 딥러닝, 정보이론 등의 커리큘럼을 구성한다." },
    { "법학과", "법률 전문가를 양성하여 사회 정의와 민주주의를 실현한다." },
    { "의료공학과", "의학과 공학을 융합하여 의료 기기 및 생체 재료를 연구한다." },
    { "약학과", "인간 건강 증진과 질병 예방을 위한 전문적인 약학 교육을 제공한다." },
    { "심리학과", "인간의 사고 과정과 행동을 이해하고 연구하는 학문을 교육한다." },
    { "통계학과", "사회, 경제, 의료 등 다양한 데이터 분석 및 통계를 연구하는 학문이다." },
    { "도시계획학과", "도시의 효율적 개발 및 지역 문제 해결을 위한 전문가를 양성한다." },
    { "자동차공학과", "자동차 설계, 제작, 유지보수 등의 기초와 실무를 연구하는 학과이다." },
    { "동물자원학과", "동물과 관련된 생명과학을 연구하며, 반려동물 및 가축 관리에 대한 교육을 제공한다." },
    { "미디어커뮤니케이션학과", "디지털 미디어 및 커뮤니케이션 이론을 기반으로 콘텐츠 기획 및 제작을 연구한다." },
    { "체육학과", "운동, 스포츠 및 신체 활동을 통해 건강과 체력을 향상시키는 학문을 연구한다." }
};

        private string GetMajorDescription(string major)
        {
            return majorDescriptions.ContainsKey(major) ? majorDescriptions[major] : "학과 설명 없음";
        }

        // ▼ 맵 별로 "오답 6개"도 관리하는 사전
        private Dictionary<string, MissionData[]> wrongMissionDatabase = new Dictionary<string, MissionData[]>
        {
            {
                "robot", new MissionData[]
                {
                    new MissionData("생물학과", "로봇 관련 오답 미션1"),
                    new MissionData("음악교육과", "로봇 관련 오답 미션2"),
                    new MissionData("주거환경학과", "로봇 관련 오답 미션3"),
                    new MissionData("간호학과", "로봇 관련 오답 미션4"),
                    new MissionData("제약공학과", "로봇 관련 오답 미션5"),
                    new MissionData("언어학과", "로봇 관련 오답 미션6")
                }
            },
            {
                "bio", new MissionData[]
                {
                    new MissionData("경제학과", "바이오 관련 오답 미션1"),
                    new MissionData("소비자학과", "바이오 관련 오답 미션2"),
                    new MissionData("교육공학과", "바이오 관련 오답 미션3"),
                    new MissionData("경호학과", "바이오 관련 오답 미션4"),
                    new MissionData("환경디자인학과", "바이오 관련 오답 미션5"),
                    new MissionData("스포츠산업학과", "바이오 관련 오답 미션6")
                }
            },
            {
                "hub", new MissionData[]
                {
                    new MissionData("문헌정보학과", "허브 관련 오답 미션1"),
                    new MissionData("심리학과", "허브 관련 오답 미션2"),
                    new MissionData("기술교육과", "허브 관련 오답 미션3"),
                    new MissionData("응급구조학과", "허브 관련 오답 미션4"),
                    new MissionData("한의예과", "허브 관련 오답 미션5"),
                    new MissionData("영상디자인학과", "허브 관련 오답 미션6")
                }
            },
            {
                "life", new MissionData[]
                {
                    new MissionData("국어국문학과", "라이프 관련 오답 미션1"),
                    new MissionData("아동보육학과", "라이프 관련 오답 미션2"),
                    new MissionData("언어치료학과", "라이프 관련 오답 미션3"),
                    new MissionData("방사선학과", "라이프 관련 오답 미션4"),
                    new MissionData("무용학과", "라이프 관련 오답 미션5"),
                    new MissionData("패션디자인학과", "라이프 관련 오답 미션6")
                }
            }
        };

        //각 buttonID별 미션 정보를 담은 Dictionary
        private Dictionary<string, MissionData[]> missionDatabase = new Dictionary<string, MissionData[]>
        {
            {
                "robot", new MissionData[]
                {
                    new MissionData("로봇디자이너", "로봇의 기능과 목적을 분석하여 로봇의 외형을 구상하고 디자인한다."),
                    new MissionData("인공지능전문가", "머신 러닝, 딥 러닝 등 컴퓨터 공학분야의 전문지식으로 스스로 사고하는 능력을 가진 컴퓨터시스템을 개발한다."),
                    new MissionData("드론윤리학자", "드론이 사회, 개인에 미치는 영향을 평가하여 적절한 윤리적 가이드라인과 규정을 제시한다. ")
                }
            },
            {
                "bio", new MissionData[]
                {
                    new MissionData("생의학엔지니어", "생명과학과 공학을 융합하여 의료, 바이오의약품, 유전자 치료 등 새로운 의학 기술과 생명과학 분야의 솔루션을 개발한다."),
                    new MissionData("나노의약품전문가", "나노기술을 응용하여 인류에게 혁신적인 의약품을 개발하고 약물 효과 증대를 위한 연구 및 제조를 담당한다."),
                    new MissionData("유전상담사", "유전학적인 정보를 기초로 건강과 질병 위험을 평가하고 상담을 제공한다.")
                }
            },
            {
                "hub", new MissionData[]
                {
                    new MissionData("빅데이터전문가", "대량의 빅데이터를 수집하여 사람들의 행동이나 시장의 변화 등을 분석하고 전략적 결정을 내린다."),
                    new MissionData("도시계획가", "도시를 효율적으로 발전시키기 위해 다양한 측면에서 도시를 계획하고 설계한다."),
                    new MissionData("무인자동차엔지니어", "다양한 소프트웨어 시스템을 통합하여 자율주행 자동차의 설계, 개발 및 유지보수를 담당한다.")
                }
            },
            {
                "life", new MissionData[]
                {
                    new MissionData("반려동물훈련상담사", "반려동물의 문제 행동을 바로잡을 수 있도록 문제 행동 원인 탐색, 분석, 훈련 프로그램 설계 및 운영 등의 일을 한다."),
                    new MissionData("디지털큐레이터", "인터넷의 수많은 정보 중 가치 있는 정보를 선별하여 콘텐츠로 만들고 제공한다."),
                    new MissionData("스포츠심리상담원", "운동 선수들의 여러 문제 상황 대처 방법이나 예방법을 교육하거나 심리상담을 진행한다. ")
                }
            }
        };
    }
}
