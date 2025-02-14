using TMPro;
using UnityEngine;

namespace YJH.ChangeTheMood
{
    public class PhaseManager : MonoBehaviour
    {
        public enum Sex
        {
            NoneSelect,
            Boy,
            Girl
        }

        [Header("Phases")]
        [SerializeField] private GameObject[] phaseParents = new GameObject[6];
        private int currentPhase = 0;

        [Header("Background")]
        [SerializeField]
        MeshRenderer BackGroundMeshRenderer;
        [SerializeField]
        Material[] BackGroundMats;

        [Header("Select")]
        private Sex selectedSex;
        private string selectedNickname;

        [Tooltip("Boy Nicknames")]
        private readonly string[] boyNicknames = { "태오", "현우", "리안", "다린", "준우", "하람" };

        [Tooltip("Girl Nicknames")]
        private readonly string[] girlNicknames = { "채아", "루나", "미카", "하린", "유나", "지안" };

        [SerializeField]
        TMP_Text[] NickNameListUI;

        [Header("Result")]
        [SerializeField]
        private SendResultData sendResultData;

        public Sex SelectedSex
        {
            get => selectedSex;
            private set
            {
                if (value == Sex.NoneSelect || value == Sex.Boy || value == Sex.Girl)
                {
                    selectedSex = value;
                    selectedNickname = ""; // 성별이 변경될 때 닉네임 초기화
                    UpdateNicknameListUI();
                }
                else
                {
                    Debug.LogWarning("Invalid sex selection");
                }
            }
        }

        public string SelectedNickname
        {
            get => selectedNickname;
            private set
            {
                if (selectedSex == Sex.NoneSelect)
                {
                    Debug.LogWarning("성별을 먼저 선택해야 합니다.");
                    return;
                }

                string[] nicknameList = selectedSex == Sex.Boy ? boyNicknames : girlNicknames;

                if (System.Array.Exists(nicknameList, nickname => nickname == value))
                {
                    selectedNickname = value;
                }
                else
                {
                    Debug.LogWarning($"잘못된 닉네임 선택: {value}");
                }
            }
        }

        private void Start()
        {
            InitializePhases();
            SelectedSex = Sex.NoneSelect;
        }

        private void InitializePhases()
        {
            for (int i = 0; i < phaseParents.Length; i++)
            {
                if (phaseParents[i] != null)
                    phaseParents[i].SetActive(false);
            }
            ActivateCurrentPhase();
        }

        private void ActivateCurrentPhase()
        {
            if (currentPhase < 0 || currentPhase >= phaseParents.Length)
            {
                Debug.LogWarning("Phase 범위 에러");
                return;
            }

            if (phaseParents[currentPhase] != null)
                phaseParents[currentPhase].SetActive(true);

            if (currentPhase == 1)
            {
                BackGroundMeshRenderer.material = BackGroundMats[0];
            }
            else if (currentPhase == 5)
            {
                BackGroundMeshRenderer.material = BackGroundMats[1];
            }
            else if (currentPhase == 6)
            {
                BackGroundMeshRenderer.material = BackGroundMats[2];
            }
            else if (currentPhase == 9)
            {
                BackGroundMeshRenderer.material = BackGroundMats[0];
            }
        }

        public void NextPhase()
        {
            if (phaseParents[currentPhase] != null)
                phaseParents[currentPhase].SetActive(false);

            currentPhase++;

            if (currentPhase < phaseParents.Length)
            {
                ActivateCurrentPhase();
            }
            else
            {
                // 모든 Phase 끝났을 때 처리 (추가 구현 필요)
            }
        }

        public void SelectSex(int selectIndex)
        {
            if (selectIndex >= 0 && selectIndex < System.Enum.GetValues(typeof(Sex)).Length)
            {
                SelectedSex = (Sex)selectIndex;
            }
            else
            {
                Debug.LogWarning("Invalid index for sex selection");
            }
        }

        public void SelectNickname(int selectIndex)
        {
            string[] nicknameList = selectedSex == Sex.Boy ? boyNicknames : girlNicknames;

            if (selectedSex == Sex.NoneSelect)
            {
                Debug.LogWarning("성별을 먼저 선택해야 합니다.");
                return;
            }

            if (selectIndex >= 0 && selectIndex < nicknameList.Length)
            {
                SelectedNickname = nicknameList[selectIndex];
            }
            else
            {
                Debug.LogWarning("Invalid index for nickname selection");
            }
        }

         private void UpdateNicknameListUI()
        {
            string[] nicknameList = selectedSex == Sex.Boy ? boyNicknames : girlNicknames;

            if (NickNameListUI == null || NickNameListUI.Length == 0)
            {
                Debug.LogWarning("NicknameListUI가 설정되지 않았습니다.");
                return;
            }

            for (int i = 0; i < NickNameListUI.Length; i++)
            {
                if (i < nicknameList.Length)
                {
                    NickNameListUI[i].text = nicknameList[i]; // 닉네임 업데이트
                    NickNameListUI[i].gameObject.SetActive(true); // 사용 가능한 닉네임만 활성화
                }
                else
                {
                    NickNameListUI[i].gameObject.SetActive(false); // 사용하지 않는 UI는 비활성화
                }
            }
        }
    }
}
