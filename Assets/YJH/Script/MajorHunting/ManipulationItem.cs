using UnityEngine;
using TMPro;
using System;

namespace YJH.MajorHunting
{

     public enum Profession
    {
        DroneEthicist,        // 드론윤리학자
        GeneticCounselor,     // 유전상담사
        SportsPsychologist,   // 스포츠 심리상담원
        DigitalCurator,       // 디지털 큐레이터
        AIExpert,             // 인공지능 전문가
        UrbanPlanner,         // 도시계획가
        SelfDrivingEngineer,  // 무인자동차 엔지니어
        NanomedicineExpert    // 나노의약품 전문가
    }
    public class ManipulationItem : MonoBehaviour
    {
         // 1) 문자열 대신 enum으로 교체
        [SerializeField]
        private Profession professionData;

        [SerializeField]
        private TMP_Text UI_Text;

        private void Start()
        {
            // 2) enum 값에 따라 실제 표시할 텍스트 반환
            UI_Text.text = ConvertProfessionToKorean(professionData);
        }

        // 3) enum -> 한글 문자열 변환용 메서드
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
                default:
                    return "(알 수 없음)";
            }
        }
    }
}