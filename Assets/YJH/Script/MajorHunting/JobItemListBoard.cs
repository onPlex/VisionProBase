using System;
using System.Collections.Generic;
using UnityEngine;

namespace YJH.MajorHunting
{
    /// <summary>
    /// JobItem 하나와, 그 아이템을 퀴즈화할지 여부를 묶어 두는 데이터 구조
    /// </summary>
    [Serializable]
    public class JobItemEntry
    {
        public JobItem jobItem;   // 참조할 JobItem
        public bool quizMode;     // Inspector에서 체크하면 퀴즈 모드로 활용
    }

    public class JobItemListBoard : MonoBehaviour
    {
        // Inspector에서 JobItem + quizMode 를 세트로 관리
        [SerializeField]
        private List<JobItemEntry> jobItemEntries;
        public List<JobItemEntry> JobItemEntries => jobItemEntries;


        [Header("MapSelectionButton")]
        [SerializeField]
        private BoxCollider mapSelectBoxCollider;
        public BoxCollider MapSelectBoxCollider => mapSelectBoxCollider;


        /// <summary>
        /// jobItemEntries에서 quizMode = true인 JobItem만 퀴즈 상태로 만들고,
        /// quizMode = false인 아이템은 일반 상태로 전환
        /// </summary>
        public void ApplyQuizSettings()
        {
            foreach (var entry in jobItemEntries)
            {
                // JobItem이 null이 아닐 때만
                if (entry.jobItem != null)
                {
                    // quizMode 여부에 따라 JobItem을 빈칸(퀴즈 모드) 또는 원래 이름으로 설정
                    entry.jobItem.SetQuizMode(entry.quizMode);
                }
            }
        }

        /// <summary>
        /// 정답 체크 함수
        /// (직접 사용하거나 ManipulationItem 쪽에서 접근)
        /// - jobItem 의 Profession 이 전달받은 profession(도구의 Profession)과 같다면 정답
        /// - 정답 시, jobItem 의 이름을 공개
        /// </summary>
        /// <returns>정답 여부 반환</returns>
        public bool CheckAnswer(JobItem jobItem, Profession profession)
        {
            if (jobItem != null && jobItem.GetProfession() == profession)
            {
                jobItem.RevealName();
                return true;
            }
            return false;
        }


    }
}
