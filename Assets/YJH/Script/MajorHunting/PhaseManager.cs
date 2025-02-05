using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;


namespace YJH.MajorHunting
{

    public enum Profession
    {
        NoneSelected,

        DroneEthicist,        // 드론윤리학자
        GeneticCounselor,     // 유전상담사
        SportsPsychologist,   // 스포츠 심리상담원
        DigitalCurator,       // 디지털 큐레이터
        AIExpert,             // 인공지능 전문가
        UrbanPlanner,         // 도시계획가
        SelfDrivingEngineer,  // 무인자동차 엔지니어
        NanomedicineExpert,   // 나노의약품 전문가
        RobotDesigner,        // 로봇 디자이너
        BioMedicalEngineer,   // 생의학 엔지니어
        BigDataExpert,        // 빅데이터 전문가
        PetTrainingConsultant // 반려동물 훈련상담사
    }



    public class PhaseManager : MonoBehaviour
    {
        [Header("Title")]
        [SerializeField]
        private GameObject TitleObj;

        [Header("Intro")]
        [SerializeField]
        private GameObject IntroObj;

        [Header("Main1")]
        [SerializeField]
        private GameObject Main1Obj;
        [SerializeField]
        private GameObject Main1PopUp;
        [SerializeField]
        private GameObject Main1Content;

        [Header("Main2")]
        [SerializeField]
        private GameObject Main2PopUp;
        [SerializeField]
        private GameObject Main1DropObject;
        public JobItemBoardManager JobItemBoardManager;

        // 4가지 버튼 ID를 명시적으로 상수로 정의
        private const string ID_ROBOT = "robot";
        private const string ID_BIO = "bio";
        private const string ID_HUB = "hub";
        private const string ID_LIFE = "life";
        // key: 버튼 ID, value: 선택되었는지 여부
        private Dictionary<string, bool> buttonStates = new Dictionary<string, bool>();

        [SerializeField]
        private GameObject MapSelectConfirmButton;

        [Header("Main3")]
        [SerializeField]
        private GameObject Main3Obj;
        [SerializeField]
        private GameObject Main3PopUp;
        [SerializeField]
        private GameObject Main3Content;

        [SerializeField]
        private TMP_Text Round1Text;

        [SerializeField]
        private TMP_Text Round2Text;

        [Header("Result")]
        [SerializeField]
        private GameObject ResultObj;

        [SerializeField]
        private GameObject Main360;
        [SerializeField]
        private GameObject Result360;

        private void Awake()
        {
            // 4개 버튼을 미리 Dictionary에 등록(초기값 false)
            // -> 나중에 새 버튼ID가 늘어나면 추가 가능
            buttonStates[ID_ROBOT] = false;
            buttonStates[ID_BIO] = false;
            buttonStates[ID_HUB] = false;
            buttonStates[ID_LIFE] = false;
        }


        //Title GameStart Button
        public void OnStepToIntro()
        {
            TitleObj.SetActive(false);
            IntroObj.SetActive(true);
        }

        public void OnStepToMain1()
        {
            IntroObj.SetActive(false);
            Main1Obj.SetActive(true);
        }

        //Close Main1 Popup
        public void OnStepToMain1Content()
        {
            Main1PopUp.SetActive(false);
            Main1Content.SetActive(true);
        }

        public void OnStepToMain2()
        {
            Main2PopUp.SetActive(true);
            Main1Content.SetActive(false);
            // 여기에 코루틴 실행 -> 3초 후 Main2PopUp 비활성화
            StartCoroutine(HideMain2PopUpAfterDelay());
        }

        // 3초 뒤 Main2PopUp을 자동 비활성화시키는 코루틴
        private IEnumerator HideMain2PopUpAfterDelay()
        {
            // 3초 대기
            yield return new WaitForSeconds(3f);


            OnStepToMain2Content();
        }

        public void OnStepToMain2Content()
        {
            // Main2PopUp 비활성화
            Main2PopUp.SetActive(false);
            Main1Content.SetActive(true);
            Main1DropObject.SetActive(false);
            JobItemBoardManager.SetMapSelectCollidersActive(true);
        }

        public void OnStepToMain3()
        {
            Main1Obj.SetActive(false);
            Main3Obj.SetActive(true);

            StartCoroutine(HideMain3PopUpAfterDelay());
        }

        private IEnumerator HideMain3PopUpAfterDelay()
        {
            // 3초 대기
            yield return new WaitForSeconds(3f);
            Main3PopUp.SetActive(false);
            Main3Content.SetActive(true);

        }

        public void OnStepToResult()
        {
            Main3Obj.SetActive(false);
            ResultObj.SetActive(true);
            Main360.SetActive(false);
            Result360.SetActive(true);
        }


        #region  Main2 MAP SELECTION
        /// <summary>
        /// 4개의 버튼 중 하나가 클릭될 때 호출되는 메서드
        /// (버튼 쪽에서 PhaseManager의 OnButtonClicked("Button1") 형태로 호출한다고 가정)
        /// </summary>
        public void OnButtonClicked(string buttonID)
        {
            // 만약 이미 2개가 선택 완료 상태라면, 더 이상 처리하지 않음
            // (원한다면 예외처리 없이 그냥 return)
            if (CountSelectedButtons() >= 2)
            {
                Debug.Log($"이미 2개가 선택되었으므로, '{buttonID}'은 무시됩니다.");
                return;
            }

            // Dictionary에 해당 버튼 ID가 없으면 추가 (초기값 false)
            if (!buttonStates.ContainsKey(buttonID))
            {
                buttonStates.Add(buttonID, false);
            }

            // 버튼을 true(선택됨)로 설정
            buttonStates[buttonID] = true;
            Debug.Log($"버튼 '{buttonID}'이(가) 선택됨.");

            // 2개가 되었는지 체크
            if (CountSelectedButtons() == 2)
            {
                // 2개 선택 완료 → 나머지 버튼(미선택) 클릭 불가로 만들기
                DisableUnselectedButtons();

                // 2개 선택 완료 시 추가로 진행할 함수 호출
                OnTwoButtonSelectedComplete();

                // JobItemBoardManager 통해 Colliders 비활성화
                JobItemBoardManager.SetMapSelectCollidersActive(false);

                MapSelectConfirmButton.SetActive(true);
            }
        }

        /// <summary>
        /// Dictionary에서 현재 true로 설정된(선택된) 버튼이 몇 개인지 센다
        /// </summary>
        private int CountSelectedButtons()
        {
            int count = 0;
            foreach (var kvp in buttonStates)
            {
                if (kvp.Value) count++;
            }
            return count;
        }

        /// <summary>
        /// 아직 선택되지 않은(false) 버튼들에 대해
        /// 더 이상 선택되지 않도록 처리
        /// (예: 내부적으로 상태를 잠근다거나, 클릭 로직을 끊는다거나, SetActive(false) 등)
        /// </summary>
        private void DisableUnselectedButtons()
        {
            // 모든 버튼 중에서 false인 버튼은 더 이상 선택 불가하게 처리
            foreach (var kvp in buttonStates)
            {
                if (!kvp.Value)
                {
                    Debug.Log($"버튼 '{kvp.Key}'은 선택 불가 상태로 변경");
                    // 필요하다면, 여기서 해당 버튼 오브젝트 찾아서 collider.enabled = false 등
                    // 버튼 오브젝트를 찾는 로직 (ex. GameObject.Find, Dictionary<버튼ID, GameObject> 등)
                }
            }
        }


        private void OnTwoButtonSelectedComplete()
        {
            Debug.Log("2개 버튼 선택이 완료되었습니다. 다음 콘텐츠로 진행합니다...");

            // ▼ NEW: 선택된 2개의 버튼 ID를 가져온 뒤, Round1Text / Round2Text에 표시
            List<string> selectedIDs = GetSelectedButtonIDs();
            if (selectedIDs.Count >= 2)
            {
                // 예: Round1Text에는 첫 번째 선택 ID, Round2Text에는 두 번째 ID
                Round1Text.text = ConvertButtonIdToDisplayText(selectedIDs[0]);
                Round2Text.text = ConvertButtonIdToDisplayText(selectedIDs[1]);
            }
        }

        public List<string> GetSelectedButtonIDs()
        {
            List<string> selected = new List<string>();
            foreach (var kvp in buttonStates)
            {
                if (kvp.Value) // 선택됨
                {
                    selected.Add(kvp.Key);
                }
            }
            return selected;
        }

        private string ConvertButtonIdToDisplayText(string id)
        {
            switch (id)
            {
                case "robot":
                    return "Robot Map";
                case "bio":
                    return "Bio Map";
                case "hub":
                    return "Hub Map";   // 예: "hub" -> "Hub Hub"
                case "life":
                    return "Life Map";
                default:
                    // 예외 상황: 등록되지 않은 ID
                    // 일단 대문자로 변환 + " Hub" 붙이는 일반 로직 (또는 "(Unknown)" 등)
                    return char.ToUpper(id[0]) + id.Substring(1) + " Hub";
            }
        }
    }
    #endregion
}