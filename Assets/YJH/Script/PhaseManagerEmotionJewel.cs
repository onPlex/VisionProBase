using System.Collections;
using UnityEngine;
using TMPro;

namespace YJH.EmotionJewel
{
    public class PhaseManagerEmotionJewel : MonoBehaviour
    {
        private enum Phase
        {
            Title,
            Prologue,
            Tutorial,
            Stage1,
            Stage2,
            Stage3,
            Stage4,
            Result
        }

        [Header("DebugTEXT")]
        [SerializeField]
        private TMP_Text resultDebugText;

        [SerializeField]
        private EmotionJewelResult emotionJewelResult;

        [Header("Phase Objects")]
        [Tooltip("Title - Prologue - Tutorial - Stage1 - Stage2 - Stage3 - Result")]
        [SerializeField]
        private GameObject[] phaseObjects;

        private void Start()
        {


            InitPhase();
        }

        /// <summary>
        /// 모든 오브젝트 비활성화 후 초기 단계 활성화
        /// </summary>
        private void InitPhase()
        {
            SetActivePhase(Phase.Title);
        }

        private void SetActivePhase(Phase phase)
        {
            for (int i = 0; i < phaseObjects.Length; i++)
            {
                if (phaseObjects[i])
                    phaseObjects[i].SetActive(i == (int)phase);
            }
        }

        // ============ Phase 진행 함수 ============
        public void OnPrologueStart()
        {
            SetActivePhase(Phase.Prologue);
        }

        public void OnGamePrologueEnd()
        {
            OnTutorialStart();
        }

        public void OnTutorialStart()
        {
            SetActivePhase(Phase.Tutorial);
        }

        public void OnStage1Start()
        {
            SetActivePhase(Phase.Stage1);
        }

        public void OnStage2()
        {
            StartCoroutine(HandleStageTransition(Phase.Stage1, Phase.Stage2, "TutorialEnd1", 0));
        }

        public void OnStage3()
        {
            StartCoroutine(HandleStageTransition(Phase.Stage2, Phase.Stage3, "TutorialEnd2", 1));
        }

        public void OnStage4()
        {
            StartCoroutine(HandleStageTransition(Phase.Stage3, Phase.Stage4, "TutorialEnd3", 2));
        }

        public void OnResult()
        {
            StartCoroutine(HandleStageTransition(Phase.Stage4, Phase.Result, "TutorialEnd4", 3));
        }


        // ============ 공통적인 스테이지 전환 처리 ============
        private IEnumerator HandleStageTransition(Phase from, Phase to, string sfx, int resultIndex)
        {
            emotionJewelResult.StoreBoardResult(resultIndex);
            SetActivePhase(from);

            AudioManager.Instance.PlaySFX(sfx);
            yield return new WaitForSecondsRealtime(1f);

            Debug.Log($"Transitioning to {to}");
            SetActivePhase(to);
            emotionJewelResult.CalculateFinalResult();
        }
    }
}
