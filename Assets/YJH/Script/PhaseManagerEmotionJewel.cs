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
            Stage1End,
            Stage2,
            Stage2End,
            Stage3,
            Stage3End,
            Stage4,
            Stage4End,
            Result
        }

        // [Header("DebugTEXT")]
        // [SerializeField]
        // private TMP_Text resultDebugText;

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

        public void OnStage1End()
        {
            StartCoroutine(HandleStageTransition(Phase.Stage1, Phase.Stage1End, "TutorialEnd1", 0));
        }

        public void OnStage2()
        {
            StartCoroutine(HandleStageTransition(Phase.Stage1End, Phase.Stage2, "TutorialEnd1", 0));
        }

         public void OnStage2End()
        {
            StartCoroutine(HandleStageTransition(Phase.Stage2, Phase.Stage2End, "TutorialEnd1", 0));
        }


        public void OnStage3()
        {
            StartCoroutine(HandleStageTransition(Phase.Stage2End, Phase.Stage3, "TutorialEnd2", 1));
        }

         public void OnStage3End()
        {
            StartCoroutine(HandleStageTransition(Phase.Stage3, Phase.Stage3End, "TutorialEnd1", 0));
        }


        public void OnStage4()
        {
            StartCoroutine(HandleStageTransition(Phase.Stage3End, Phase.Stage4, "TutorialEnd3", 2));
        }

         public void OnStage4End()
        {
            StartCoroutine(HandleStageTransition(Phase.Stage4, Phase.Stage4End, "TutorialEnd1", 0));
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
