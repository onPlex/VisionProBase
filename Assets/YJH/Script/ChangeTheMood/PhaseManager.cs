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
        // 0~5까지의 Phase Parent 오브젝트
        [SerializeField] private GameObject[] phaseParents = new GameObject[6];

        // 현재 진행중인 Phase 인덱스(0~5)
        private int currentPhase = 0;

        [Header("Background")]
        [SerializeField]
        MeshRenderer BackGroundMeshRenderer;
        [SerializeField]
        Material[] BackGroundMats;

        [Header("Select")]
        private Sex selectedSex;
        private string selectedNickname;

        [Tooltip("0~5 boy Nickname, 6~11 boy Nickname")]
        private readonly string[] nicknames = { "태오", "현우", "리안", "다린", "준우", "하람", "채아", "루나", "미카", "하린", "유나", "지안" };

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
                int startIndex = selectedSex == Sex.Boy ? 0 : (selectedSex == Sex.Girl ? 6 : -1);
                int endIndex = selectedSex == Sex.Boy ? 5 : (selectedSex == Sex.Girl ? 11 : -1);

                if (startIndex == -1 || endIndex == -1)
                {
                    Debug.LogWarning("Sex가 선택되지 않음");
                    return;
                }

                int nicknameIndex = System.Array.IndexOf(nicknames, value);
                if (nicknameIndex >= startIndex && nicknameIndex <= endIndex)
                {
                    selectedNickname = value;
                }
                else
                {
                    Debug.LogWarning("잘못된 닉네임 선택");
                }

            }
        }

        private void Start()
        {
            // 초기 셋업
            InitializePhases();
            SelectedSex = Sex.NoneSelect;
        }

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
                //CalculateFinalCareer();

                // 결과 Phase로 이동
                //GoToResultPhase();
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
            if (selectIndex >= 0 && selectIndex < nicknames.Length)
            {
                SelectedNickname = nicknames[selectIndex];
            }
            else
            {
                Debug.LogWarning("Invalid index for nickname selection");
            }
        }
    }
}
