using UnityEngine;
using TMPro;

namespace YJH.MajorHunting
{
    public class ManipulationItem : MonoBehaviour
    {
        [SerializeField]
        private Profession professionData;

        [SerializeField]
        private TMP_Text UI_Text;

        // JobItemBoardManager를 참조 (인스펙터에서 할당 또는 Find로 가져오기)
        [SerializeField]
        private JobItemBoardManager jobItemBoardManager;

        private void Start()
        {
            UI_Text.text = ConvertProfessionToKorean(professionData);
        }

        private void OnTriggerEnter(Collider other)
        {
            // 충돌한 대상에서 JobItem 컴포넌트 탐색
            JobItem jobItem = other.GetComponent<JobItem>();
            if (jobItem != null && jobItemBoardManager != null)
            {
                // JobItemBoardManager에 정답 체크 (모든 보드 대상)
                bool isCorrect = jobItemBoardManager.CheckAnswer(jobItem, professionData);
                if (isCorrect)
                {
                    Debug.Log("정답 처리 완료!");
                    gameObject.SetActive(false);
                }
                else
                {
                    Debug.Log("오답입니다.");
                }
            }
        }

        private string ConvertProfessionToKorean(Profession profession)
        {
            switch (profession)
            {
                case Profession.DroneEthicist:
                    return "드론윤리학자";
                case Profession.GeneticCounselor:
                    return "유전상담사";
                case Profession.SportsPsychologist:
                    return "스포츠\n심리상담원";
                case Profession.DigitalCurator:
                    return "디지털\n큐레이터";
                case Profession.AIExpert:
                    return "인공지능\n전문가";
                case Profession.UrbanPlanner:
                    return "도시계획가";
                case Profession.SelfDrivingEngineer:
                    return "무인자동차\n엔지니어";
                case Profession.NanomedicineExpert:
                    return "나노의약품\n전문가";
                case Profession.RobotDesigner:
                    return "로봇\n디자이너";
                case Profession.BioMedicalEngineer:
                    return "생의학\n엔지니어";
                case Profession.BigDataExpert:
                    return "빅데이터\n전문가";
                case Profession.PetTrainingConsultant:
                    return "반려동물\n훈련상담사";
                case Profession.NoneSelected:
                    return "(미선택)";
                default:
                    return "(알 수 없음)";
            }
        }
    }
}
