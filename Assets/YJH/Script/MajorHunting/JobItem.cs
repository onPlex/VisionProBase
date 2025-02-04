using UnityEngine;
using TMPro;

namespace YJH.MajorHunting
{
    public class JobItem : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField]
        private Profession job;
        private string jobItemName;
        private string jobItemDec;

        [Header("UI")]
        [SerializeField]
        private TMP_Text jobNameText;
        [SerializeField]
        private TMP_Text jobDecText;

        // 퀴즈 모드 여부를 나타내는 플래그(필요하다면 public 프로퍼티로도 가능)
        private bool isQuizMode = false;

        // --- 정답 처리 여부를 추적하기 위한 필드 ---
        private bool isAnswered = false;
        public bool IsAnswered => isAnswered;

        private void Start()
        {
            // 초기 직업 정보 설정
            SetJobInfo(job);

            // 혹시 이 시점부터 퀴즈 모드라면, 이름을 숨김
            if (isQuizMode && jobNameText != null)
            {
                jobNameText.text = "빈칸";
            }
        }

        /// <summary>
        /// job 에 따라 JobItemName, JobItemDec 를 설정하고
        /// TMPro 텍스트를 갱신하는 메서드
        /// </summary>
        private void SetJobInfo(Profession selectedJob)
        {
            switch (selectedJob)
            {
                case Profession.NoneSelected:
                    jobItemName = "(미설정)";
                    jobItemDec = "(직업 정보 없음)";
                    break;
                case Profession.DroneEthicist:
                    jobItemName = "드론윤리학자";
                    jobItemDec = "드론의 윤리적 활용과 책임을 연구하는 전문가";
                    break;
                case Profession.GeneticCounselor:
                    jobItemName = "유전상담사";
                    jobItemDec = "유전 정보 분석 및 상담을 담당하는 전문 의료인";
                    break;
                case Profession.SportsPsychologist:
                    jobItemName = "스포츠 심리상담원";
                    jobItemDec = "선수들의 심리 상태를 분석하고 멘탈 관리를 돕는 전문가";
                    break;
                case Profession.DigitalCurator:
                    jobItemName = "디지털 큐레이터";
                    jobItemDec = "디지털 콘텐츠 수집, 전시 및 관리 전문가";
                    break;
                case Profession.AIExpert:
                    jobItemName = "인공지능 전문가";
                    jobItemDec = "AI 시스템을 연구 및 개발하고 적용 방안을 제시하는 전문가";
                    break;
                case Profession.UrbanPlanner:
                    jobItemName = "도시계획가";
                    jobItemDec = "도시 개발 및 구조를 종합적으로 설계하고 관리하는 전문가";
                    break;
                case Profession.SelfDrivingEngineer:
                    jobItemName = "무인자동차 엔지니어";
                    jobItemDec = "자율주행 기술을 연구ㆍ개발하고 보급을 이끄는 기술자";
                    break;
                case Profession.NanomedicineExpert:
                    jobItemName = "나노의약품 전문가";
                    jobItemDec = "나노 기술을 의약품 개발에 적용하는 연구원";
                    break;
                case Profession.RobotDesigner:
                    jobItemName = "로봇 디자이너";
                    jobItemDec = "로봇의 외형, 기구 설계 및 사용자 경험을 고려한 디자인 전문가";
                    break;
                case Profession.BioMedicalEngineer:
                    jobItemName = "생의학 엔지니어";
                    jobItemDec = "의료 기기 및 생체 공학 기술을 연구·개발하는 공학자";
                    break;
                case Profession.BigDataExpert:
                    jobItemName = "빅데이터 전문가";
                    jobItemDec = "방대한 데이터를 분석해 인사이트를 도출하고 솔루션을 제시하는 전문가";
                    break;
                case Profession.PetTrainingConsultant:
                    jobItemName = "반려동물 훈련상담사";
                    jobItemDec = "반려동물 행동 교정과 훈련 방법을 제시하며, 보호자를 상담하는 전문가";
                    break;
            }

            // UI 텍스트에 반영 (퀴즈 모드가 아닐 때만 바로 표시)
            if (!isQuizMode)
            {
                if (jobNameText != null)
                    jobNameText.text = jobItemName;
            }

            if (jobDecText != null)
                jobDecText.text = jobItemDec;
        }

        public void SetProfession(Profession newProfession)
        {
            job = newProfession;
            // 새 직업에 맞게 이름/설명을 갱신
            SetJobInfo(newProfession);

            // 만약 퀴즈 모드였으면, 이름을 "빈칸"으로 다시 세팅
            if (isQuizMode && jobNameText != null)
            {
                jobNameText.text = "빈칸";
            }
        }

        /// <summary>
        /// JobItem을 퀴즈 상태(이름 숨김) 또는 일반 상태로 전환
        /// </summary>
        public void SetQuizMode(bool quiz)
        {
            isQuizMode = quiz;

            if (quiz)
            {
                // 빈칸으로 표시
                if (jobNameText != null)
                    jobNameText.text = "빈칸";
            }
            else
            {
                // 원래 이름 복원
                if (jobNameText != null)
                    jobNameText.text = jobItemName;
            }
        }

        /// <summary>
        /// 정답 시, JobItem의 이름을 공개
        /// </summary>
        public void RevealName()
        {
            if (jobNameText != null)
                jobNameText.text = jobItemName;

            // 퀴즈 모드 해제 등 추가 처리 필요 시 작성
            isQuizMode = false;
            isAnswered = true;
        }

        /// <summary>
        /// 외부에서 JobItem의 Profession을 조회하기 위한 메서드
        /// (또는 public getter로 만들 수도 있음)
        /// </summary>
        public Profession GetProfession()
        {
            return job;
        }
    }
}
