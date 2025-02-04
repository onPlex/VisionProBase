using System;
using System.Collections.Generic;
using UnityEngine;

namespace YJH.MajorHunting
{
    public class JobItemBoardManager : MonoBehaviour
    {
        [Header("관리할 JobItemListBoard 목록")]
        [SerializeField]
        private List<JobItemListBoard> boards;

        private void Start()
        {
            // 1) 모두 quizMode 설정 적용
            ApplyAllBoardsQuizSettings();
            // 2) NoneSelected인 JobItem 자동 설정
            CheckDuplicateProfessions();
        }

        /// <summary>
        /// 모든 보드에 대해, 각 JobItemEntry의 quizMode 설정을 반영해
        /// '빈칸'(퀴즈 모드) 또는 원래 이름으로 전환
        /// </summary>
        public void ApplyAllBoardsQuizSettings()
        {
            if (boards == null) return;

            foreach (var board in boards)
            {
                if (board != null)
                {
                    board.ApplyQuizSettings();
                }
            }
        }

        /// <summary>
        /// 1) 이미 Profession이 NoneSelected 가 아닌 JobItem은 그대로 둔다 (중복이라도 그대로)
        ///    → used 목록에 추가
        /// 2) Profession이 NoneSelected 인 JobItem만, 사용되지 않은 Profession으로 자동 변경
        ///    → 변경 후 used에 추가
        /// </summary>
        public void CheckDuplicateProfessions()
        {
            if (boards == null) return;

            // 이미 사용된 Profession을 기록
            HashSet<Profession> used = new HashSet<Profession>();

            // 1) NoneSelected가 *아닌* 아이템들을 먼저 'used'에 추가
            foreach (var board in boards)
            {
                if (board == null) continue;

                foreach (var entry in board.JobItemEntries)
                {
                    if (entry == null || entry.jobItem == null)
                        continue;

                    var currentProf = entry.jobItem.GetProfession();

                    // NoneSelected가 아닌 Profession은 무조건 used에 추가
                    // (중복이더라도 자동 수정은 안 함)
                    if (currentProf != Profession.NoneSelected)
                    {
                        used.Add(currentProf);
                    }
                }
            }

            // 2) NoneSelected 아이템들에 대해서만 사용되지 않은 Profession으로 할당
            foreach (var board in boards)
            {
                if (board == null) continue;

                foreach (var entry in board.JobItemEntries)
                {
                    if (entry == null || entry.jobItem == null)
                        continue;

                    var currentProf = entry.jobItem.GetProfession();

                    // NoneSelected → 아직 직업이 설정되지 않았으므로, 중복되지 않은 값 할당
                    if (currentProf == Profession.NoneSelected)
                    {
                        // FindAvailableProfession: 사용되지 않은 Profession 중 하나를 찾음
                        var newProf = FindAvailableProfession(used);
                        entry.jobItem.SetProfession(newProf);

                        Debug.LogWarning(
                            $"Board '{board.name}', JobItem '{entry.jobItem.name}'가 NoneSelected 상태였으므로, " +
                            $"'{newProf}' 로 자동 변경했습니다."
                        );

                        used.Add(newProf);
                    }
                }
            }
        }

        /// <summary>
        /// used에 없는 Profession 중 하나를 반환.
        /// 모두 소진됐으면 NoneSelected 또는 첫 번째 값 반환 (상황 맞게 조정 가능).
        /// </summary>
        private Profession FindAvailableProfession(HashSet<Profession> used)
        {
            foreach (Profession prof in Enum.GetValues(typeof(Profession)))
            {
                // NoneSelected는 할당 대상으로 쓰지 않는다고 가정
                if (prof == Profession.NoneSelected)
                    continue;

                if (!used.Contains(prof))
                {
                    return prof;
                }
            }

            Debug.LogError("사용 가능한 Profession이 더 이상 없음. NoneSelected 반환.");
            return Profession.NoneSelected;
        }

        public bool CheckAnswer(JobItem jobItem, Profession profession)
        {
            if (boards == null) return false;

            bool anyCorrect = false;
            foreach (var board in boards)
            {
                if (board == null) continue;

                // 그 보드에서 CheckAnswer 호출
                if (board.CheckAnswer(jobItem, profession))
                {
                    anyCorrect = true;
                    break; // 이미 정답 발견 → 반복 중단
                }
            }

            // 정답이었다면 "퀴즈 전체 완료 여부" 검사
            if (anyCorrect)
            {
                if (AreAllQuizItemsAnswered())
                {
                    Debug.Log("모든 퀴즈 정답 완료! 다음 콘텐츠 진행.");
                    NextContent();
                }
            }

            return anyCorrect;
        }

        public bool AreAllQuizItemsAnswered()
        {
            if (boards == null) return true; // 보드 없으면 그냥 true

            foreach (var board in boards)
            {
                if (board == null) continue;

                // 보드 내 entry들 확인
                foreach (var entry in board.JobItemEntries)
                {
                    // quizMode가 true인데 아직 isAnswered가 false면 미완료
                    if (entry.quizMode == true && entry.jobItem != null)
                    {
                        if (!entry.jobItem.IsAnswered)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// "모든 퀴즈 정답 완료" 시점에 진행할 내용 (더미 함수)
        /// 실제 게임 로직(씬 전환, UI 표시 등)에 맞춰 구현
        /// </summary>
        private void NextContent()
        {
            Debug.Log("NextContent() 호출 - 다음 콘텐츠로 진행 로직을 작성하세요.");
            // 예: 씬 전환, UI 팝업, 새로운 오브젝트 활성화 등
        }
    }
}
