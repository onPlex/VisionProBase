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
                    jobItemDec = "드론이 사회, 개인에 미치는 영향을 평가하여 적절한 윤리적 가이드라인과 규정을 제시한다.";
                    break;
                case Profession.GeneticCounselor:
                    jobItemName = "유전상담사";
                    jobItemDec = "유전학적인 정보를 기초로 건강과 질병 위험을 평가하고 상담을 제공한다.";
                    break;
                case Profession.SportsPsychologist:
                    jobItemName = "스포츠 심리상담원";
                    jobItemDec = "운동 선수들의 여러 문제 상황 대처 방법이나 예방법을 교육하거나 심리상담을 진행한다.";
                    break;
                case Profession.DigitalCurator:
                    jobItemName = "디지털 큐레이터";
                    jobItemDec = "인터넷의 수많은 정보 중 가치 있는 정보를 선별하여 콘텐츠로 만들고 제공한다.";
                    break;
                case Profession.AIExpert:
                    jobItemName = "인공지능 전문가";
                    jobItemDec = "머신 러닝, 딥 러닝 등 컴퓨터 공학분야의 전문지식으로 스스로 사고하는 능력을 가진 컴퓨터시스템을 개발한다.";
                    break;
                case Profession.UrbanPlanner:
                    jobItemName = "도시계획가";
                    jobItemDec = "도시를 효율적으로 발전시키기 위해 다양한 측면에서 도시를 계획하고 설계한다.";
                    break;
                case Profession.SelfDrivingEngineer:
                    jobItemName = "무인자동차 엔지니어";
                    jobItemDec = "다양한 소프트웨어 시스템을 통합하여 자율주행 자동차의 설계, 개발 및 유지보수를 담당한다.";
                    break;
                case Profession.NanomedicineExpert:
                    jobItemName = "나노의약품 전문가";
                    jobItemDec = "나노기술을 응용하여 인류에게 혁신적인 의약품을 개발하고 약물 효과 증대를 위한 연구 및 제조를 담당한다.";
                    break;
                case Profession.RobotDesigner:
                    jobItemName = "로봇 디자이너";
                    jobItemDec = "로봇의 기능과 목적을 분석하여 로봇의 외형을 구상하고 디자인한다.";
                    break;
                case Profession.BioMedicalEngineer:
                    jobItemName = "생의학 엔지니어";
                    jobItemDec = "생명과학과 공학을 융합하여 의료, 바이오의약품, 유전자 치료 등 새로운 의학 기술과 생명과학 분야의 솔루션을 개발한다.";
                    break;
                case Profession.BigDataExpert:
                    jobItemName = "빅데이터 전문가";
                    jobItemDec = "대량의 빅데이터를 수집하여 사람들의 행동이나 시장의 변화 등을 분석하고 전략적 결정을 내린다.";
                    break;
                case Profession.PetTrainingConsultant:
                    jobItemName = "반려동물 훈련상담사";
                    jobItemDec = "반려동물의 문제 행동을 바로잡을 수 있도록 문제 행동 원인 탐색, 분석, 훈련 프로그램 설계 및 운영 등의 일을 한다.";
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
