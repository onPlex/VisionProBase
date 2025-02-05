using UnityEngine;
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

        // ▼ NEW: 총알 관련
        private int bulletCount = 5; // 남은 총알 수
        [SerializeField]
        private GameObject[] bulletIndicators; // 씬에 배치된 5개 총알 UI

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

        [Header("Shooting Targets (9개 버튼)")]
        [SerializeField] private List<ShootingTarget> shootingTargets; 

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
        }

       private void AssignMissionsToShootingTargets(MissionData[] correctDatas)
    {
        if (shootingTargets == null || shootingTargets.Count < 9)
        {
            Debug.LogWarning("shootingTargets가 9개 미만!");
            return;
        }

        // '정답' 미션 이름들
        List<string> correctList = new List<string>();
        foreach (var c in correctDatas)
        {
            correctList.Add(c.name); 
        }
        // '오답' 6개
        List<string> wrongList = new List<string>(wrongMissions);

        // 합치기
        List<string> totalMissions = new List<string>();
        totalMissions.AddRange(correctList); // 3개
        totalMissions.AddRange(wrongList);   // 6개 = 총 9개

        // 셔플
        Shuffle(totalMissions);

        // 9개의 ShootingTarget에 대입
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
                // ▼ TimerText 에 "Timer : 남은시간초 초" 형태로 표시
                // Mathf.CeilToInt를 써서 올림 표시하거나, int 변환 등 원하는 방식 사용 가능
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

        /// <summary>
        /// 라운드별 환경 구성(스테이지, 배경 등)
        /// </summary>
         private void SetupRoundEnvironment(int round, string mapID)
    {
        Debug.Log($"[MainShootingGame] Round {round}, Map: {mapID}");

        // 1) 맵ID에 해당하는 '정답' 미션 3개 추출
        //    (missionDatabase[mapID]가 3개의 MissionData를 갖고 있다고 가정)
        if (!missionDatabase.TryGetValue(mapID, out MissionData[] correctMissions))
        {
            Debug.LogError($"missionDatabase에 '{mapID}' 키가 없습니다.");
            return;
        }

        // 2) UI에 표시할 3개 미션명/설명 업데이트
        UpdateMissionListUI(mapID);

        // 3) ShootingTarget들에 '정답 3개 + 오답 6개'를 섞어서 분배
        AssignMissionsToShootingTargets(correctMissions);
    }

        /// <summary>
        /// 플레이어가 적을 격파/목표 달성 등으로 라운드를 클리어했을 때 호출
        /// </summary>
        public void OnRoundClear()
        {
            isGameRunning = false;
            Debug.Log($"[MainShootingGame] Round {currentRound} 클리어!");

            if (currentRound == 1)
            {
                // 라운드1 → 라운드2로 진입
                currentRound = 2;
                SetupRoundEnvironment(currentRound, round2ID);
                StartRound();
            }
            else
            {
                // 라운드2까지 모두 성공 시 게임 완료
                Debug.Log("[MainShootingGame] 모든 라운드 완료! 게임 승리");
                // TODO: 씬 전환 or UI 표시 등
            }
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

            FailPopUp.SetActive(true);
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
                SetupRoundEnvironment(1, round1ID);
                StartRound();
            }
            else
            {
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

            // 남은 총알 1발 소진
            ConsumeBullet();

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
                // ShootingTarget 정답 이펙트
                target.PlayCorrectEffect();

                // TODO: 미션 하나를 맞췄으니 어떤 로직을 수행할지?
                // ex) "3개 미션 중 1개 해결" 로직, 3개 모두 해결 시 OnRoundClear() 등
                Debug.Log($"[MainShootingGame] 미션 '{missionName}' 정답!");
            }
            else
            {
                // 오답
                target.PlayWrongEffect();
                Debug.Log($"[MainShootingGame] 미션 '{missionName}' 오답!");
            }
        }


        // 6개의 오답 미션 (예시)
        private string[] wrongMissions = 
        {
            "Wrong1", 
            "Wrong2", 
            "Wrong3", 
            "Wrong4", 
            "Wrong5", 
            "Wrong6"
        };

        //각 buttonID별 미션 정보를 담은 Dictionary
        private Dictionary<string, MissionData[]> missionDatabase = new Dictionary<string, MissionData[]>
        {
            {
                "robot", new MissionData[]
                {
                    new MissionData("Robot Attack", "로봇을 공격하는 미션입니다."),
                    new MissionData("Robot Defense", "로봇을 방어하는 미션입니다."),
                    new MissionData("Robot Recon", "로봇 지형 정찰 미션입니다.")
                }
            },
            {
                "bio", new MissionData[]
                {
                    new MissionData("Bio Hazard", "바이오 위험 지역을 정리하세요."),
                    new MissionData("Bio Sample", "바이오 샘플을 채취하세요."),
                    new MissionData("Bio Lab", "바이오 연구실을 점검하세요.")
                }
            },
            {
                "hub", new MissionData[]
                {
                    new MissionData("Hub Connection", "허브 연결 상태를 확인합니다."),
                    new MissionData("Hub Upgrade", "허브 시스템을 업그레이드하세요."),
                    new MissionData("Hub Defense", "허브를 방어하세요.")
                }
            },
            {
                "life", new MissionData[]
                {
                    new MissionData("Life Search", "생명체를 탐색하세요."),
                    new MissionData("Life Protection", "생명체를 보호하세요."),
                    new MissionData("Life Study", "생명체를 연구하세요.")
                }
            }
        };
    }
}
