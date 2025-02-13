using UnityEngine;
using TMPro;
using System;

namespace YJH
{
    public class MainContentManager : MonoBehaviour
    {
        [Header("Phases")]
        [SerializeField] private GameObject[] phaseParents = new GameObject[6];

        [Header("UI")]
        [SerializeField] TMP_Text TMP_PhaseText;
        [SerializeField] private GameObject PhaseTextObj;
        [SerializeField] private GameObject ReturnButtonObj;

        [Header("Result")]
        [SerializeField] private Jun.Dolphin dolphin;
        [SerializeField] private GameObject Basket;
        [SerializeField] private GameObject ShellCountUI;

        // -------------------------------------------------------------
        // 디버깅/로그에 도움이 되도록 SerializeField로 확인
        // -------------------------------------------------------------
        [SerializeField]
        private int _currentPhase = 0;

        public event Action<int> OnPhaseChanged;

        // R,I,A,S,E,C (0,1,2,3,4,5)
        private int[] careerSelectedCounts = new int[6];

        public ShellInfo.CareerType finalCareer;

        public int CurrentPhase
        {
            get => _currentPhase;
            private set
            {
                // setter 호출 시점 디버그
                int oldVal = _currentPhase;
                Debug.Log($"[CurrentPhase setter] Called with value={value}, oldVal={oldVal}");

                // 0 ~ (phaseParents.Length - 1) 범위로 보정
                int clampedValue = Mathf.Clamp(value, 0, phaseParents.Length - 1);
                Debug.Log($"[CurrentPhase setter] clampedValue={clampedValue}, phaseParents.Length={phaseParents.Length}");

                if (_currentPhase != clampedValue)
                {
                    Debug.Log($"[CurrentPhase setter] Phase changing from {oldVal} to {clampedValue}");

                    // 이전 Phase 비활성화
                    if (_currentPhase >= 0 && _currentPhase < phaseParents.Length)
                    {
                        if (phaseParents[_currentPhase] != null)
                        {
                            Debug.Log($"[CurrentPhase setter] Deactivating old phase index={_currentPhase}");
                            phaseParents[_currentPhase].SetActive(false);
                        }
                    }

                    _currentPhase = clampedValue;

                    // 새 Phase 활성화
                    if (phaseParents[_currentPhase] != null)
                    {
                        Debug.Log($"[CurrentPhase setter] Activating new phase index={_currentPhase}");
                        phaseParents[_currentPhase].SetActive(true);
                    }

                    // UI 갱신
                    UpdatePhaseUI();

                    OnPhaseChanged?.Invoke(_currentPhase);
                }
                else
                {
                    // 값이 같으니 변경 없음
                    Debug.Log($"[CurrentPhase setter] No change (oldVal=={oldVal}, clampedValue=={clampedValue})");
                }
            }
        }

        private void Awake()
        {
            Debug.Log("[MainContentManager] Awake() called");
        }

        private void Start()
        {
            Debug.Log("[MainContentManager] Start() called, setting CurrentPhase=0");
            CurrentPhase = 0; // 초기 세팅
        }

        /// <summary>
        /// 특정 phaseIndex로 전환(이전 Phase 비활성화 → Shell 리셋 → 새 Phase 활성화)
        /// </summary>
        private void ActivatePhase(int newPhase)
        {
            // 범위 보정
            int clampedValue = Mathf.Clamp(newPhase, 0, phaseParents.Length - 1);

            // 1) 이전 Phase 비활성화
            if (_currentPhase >= 0 && _currentPhase < phaseParents.Length)
            {
                var oldObj = phaseParents[_currentPhase];
                if (oldObj != null)
                    oldObj.SetActive(false);
            }

            // 2) ShellPhase 초기화 (활성화 전에)
            var newObj = phaseParents[clampedValue];
            if (newObj != null)
            {
                // ShellPhase 컴포넌트가 있다면 Reset
                var shellPhase = newObj.GetComponent<ShellPhase>();
                if (shellPhase != null)
                {
                    //TODO::
                   // shellPhase.ResetPhaseShells();
                }
            }

            // 3) 실제 Phase 변경
            _currentPhase = clampedValue;

            // 4) 새 Phase 활성화
            if (newObj != null)
                newObj.SetActive(true);

            // 5) UI 업데이트 & 이벤트 호출
            UpdatePhaseUI();
            OnPhaseChanged?.Invoke(_currentPhase);

            Debug.Log($"[ActivatePhase()] old={_currentPhase}, new={clampedValue}");
        }


        public void RegisterShellSelection(int careerIndex)
        {
            Debug.Log($"[RegisterShellSelection()] careerIndex={careerIndex}");
            if (careerIndex < 0 || careerIndex >= 6)
            {
                Debug.LogWarning("Invalid careerIndex");
                return;
            }

            // 카운트 증가
            careerSelectedCounts[careerIndex]++;
            Debug.Log($"careerSelectedCounts[{careerIndex}]={careerSelectedCounts[careerIndex]}");

            NextPhase();
        }
        public void ReturnToPreviousPhase()
        {
            Debug.Log($"[ReturnToPreviousPhase()] called, CurrentPhase={_currentPhase}");
            if (_currentPhase > 0)
            {
                int newPhase = _currentPhase - 1;
                Debug.Log($"[ReturnToPreviousPhase()] -> newPhase={newPhase}");
                ActivatePhase(newPhase);
            }
            else
            {
                Debug.Log("이미 0단계이므로 더 이상 이전 단계로 돌아갈 수 없습니다.");
            }
        }

        private void NextPhase()
        {
            int newPhase = _currentPhase + 1;
            Debug.Log($"[NextPhase()] current={_currentPhase}, newPhase={newPhase}");

            if (newPhase < phaseParents.Length)
            {
                ActivatePhase(newPhase);
            }
            else
            {
                Debug.Log("[NextPhase()] 모든 Phase를 완료했습니다. 결과를 산출합니다.");
                CalculateFinalCareer();
                GoToResultPhase();
            }
        }


        private void UpdatePhaseUI()
        {
            Debug.Log($"[UpdatePhaseUI()] _currentPhase={_currentPhase}");

            // 0단계라면 이전버튼 숨김
            if (_currentPhase == 0)
            {
                if (PhaseTextObj != null)
                    PhaseTextObj.transform.localPosition = new Vector3(0, 0, 0);

                if (ReturnButtonObj != null)
                    ReturnButtonObj.SetActive(false);
            }
            else
            {
                if (PhaseTextObj != null)
                    PhaseTextObj.transform.localPosition = new Vector3(-0.5f, 0, 0);

                if (ReturnButtonObj != null)
                {
                    ReturnButtonObj.transform.localPosition = new Vector3(0.5f, 0, 0);
                    ReturnButtonObj.SetActive(true);
                }
            }

            // PhaseText
            if (_currentPhase < 0 || _currentPhase >= phaseParents.Length)
            {
                Debug.LogWarning("[UpdatePhaseUI()] Phase 범위 에러");
                return;
            }

            if (TMP_PhaseText != null)
            {
                TMP_PhaseText.text = $"{_currentPhase}/{phaseParents.Length}";
            }
        }

        private void CalculateFinalCareer()
        {
            int maxIndex = 0;
            for (int i = 1; i < 6; i++)
            {
                if (careerSelectedCounts[i] > careerSelectedCounts[maxIndex])
                    maxIndex = i;
            }
            finalCareer = (ShellInfo.CareerType)maxIndex;
            Debug.Log($"[CalculateFinalCareer()] 가장 많이 선택된 유형: {finalCareer}");

            SendFinalCareerResult(finalCareer);
        }

        private void GoToResultPhase()
        {
            Debug.Log("[GoToResultPhase()] 결과 화면으로 전환");
            if (Basket != null) Basket.SetActive(false);
            if (ShellCountUI != null) ShellCountUI.SetActive(false);
            if (dolphin != null)
            {
                dolphin.gameObject.SetActive(true);
                dolphin.PlayAnimation();
            }
        }

        private void SendFinalCareerResult(ShellInfo.CareerType career)
        {
            // ... (생략) ...
            Debug.Log($"[SendFinalCareerResult()] 전송할 경력 타입={career}");
            //ResultDataStorage.Instance.Game1ContData = ...
            // etc.
        }

        public int GetCurrentPhase()
        {
            return CurrentPhase;
        }
    }
}
