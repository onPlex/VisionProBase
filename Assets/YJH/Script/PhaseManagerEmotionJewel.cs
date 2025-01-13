using System.Collections;
using UnityEngine;


namespace YJH
{
    public class PhaseManagerEmotionJewel : ContentPhaseManager
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            InitPhase();
        }

        public override void OnGameStart() // == OnPrologueStart
        {
            TitleObj.SetActive(false);
            PrologueObj.SetActive(true);
        }

        public override void OnGamePrologueEnd()
        {
            // == OnTutorialStart
            Stage1StartButton.SetActive(true);
        }

        /// <summary>
        /// Stage1
        /// </summary>
        public override void OnStage1Start()
        {
            PrologueObj.SetActive(false);
            Stage1Obj.SetActive(true);
        }

        public override void OnStage2()
        {
            Debug.Log("OnStage2");
            Stage1Obj.SetActive(false);
            Stage2Obj.SetActive(true);
        }

        public override void OnStage3()
        {
            Debug.Log("OnStage3");
            Stage2Obj.SetActive(false);
            Stage3Obj.SetActive(true);
        }

        public override void OnStage4()
        {
            Debug.Log("OnStage4");
            Stage3Obj.SetActive(false);
            Stage4Obj.SetActive(true);
        }

        //TutorialEnd1
        public void OnStageFinishEvent1()
        {
            AudioManager.Instance.PlaySFX("TutorialEnd1");
            new WaitForSecondsRealtime(10f);
        }

        //TutorialEnd2
        public void OnStageFinishEvent2()
        {
            AudioManager.Instance.PlaySFX("TutorialEnd2");
            new WaitForSecondsRealtime(10f);
        }

        //TutorialEnd3
        public void OnStageFinishEvent3()
        {
            AudioManager.Instance.PlaySFX("TutorialEnd3");
            new WaitForSecondsRealtime(10f);
        }

        //TutorialEnd4
        public void OnStageFinishEvent4()
        {
            AudioManager.Instance.PlaySFX("TutorialEnd4");
            new WaitForSecondsRealtime(10f);
        }


        public override void OnResult()
        {
            Debug.Log("OnResult");
            Stage4EndObj.SetActive(false);
            ResultObj.SetActive(true);
        }
    }
}

