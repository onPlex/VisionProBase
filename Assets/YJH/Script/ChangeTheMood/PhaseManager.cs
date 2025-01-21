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

        [Header("Select")]
        Sex SelectedSex;

        string[] Nickname = {"닉네임1", "닉네임2", "닉네임3", "닉네임4", "닉네임5", "닉네임6", "닉네임7", "닉네임8", "닉네임9"};

        [Header("Result")]

        [SerializeField]
        SendResultData sendResultData;


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
            SelectedSex = (Sex)selectIndex;
        }

    }
}
