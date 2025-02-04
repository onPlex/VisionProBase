using UnityEngine;
using System.Collections;


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
        }
    }
}