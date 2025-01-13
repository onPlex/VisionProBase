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
            StartCoroutine(IStageFinishEvent1());
        }

        public override void OnStage3()
        {
             StartCoroutine(IStageFinishEvent2());
        }

        public override void OnStage4()
        {
            StartCoroutine(IStageFinishEvent3());
        }

        //TutorialEnd1
        public IEnumerator IStageFinishEvent1()
        {
            AudioManager.Instance.PlaySFX("TutorialEnd1");
            yield return new WaitForSecondsRealtime(10f);

            Debug.Log("OnStage2");
            Stage1Obj.SetActive(false);
            Stage2Obj.SetActive(true);
            yield return null;
        }

        //TutorialEnd2
        public IEnumerator IStageFinishEvent2()
        {
            AudioManager.Instance.PlaySFX("TutorialEnd2");
            yield return new WaitForSecondsRealtime(10f);

            Debug.Log("OnStage3");
            Stage2Obj.SetActive(false);
            Stage3Obj.SetActive(true);
            yield return null;
        }



        //TutorialEnd3
        public IEnumerator IStageFinishEvent3()
        {
            AudioManager.Instance.PlaySFX("TutorialEnd3");
            yield return new WaitForSecondsRealtime(10f);

            Debug.Log("OnStage4");
            Stage3Obj.SetActive(false);
            Stage4Obj.SetActive(true);
            yield return null;
        }


        //TutorialEnd4
        public IEnumerator IStageFinishEvent4()
        {
            //TODO :: On Working
            yield return null;
        }
        public override void OnResult()
        {
            Debug.Log("OnResult");
            Stage4EndObj.SetActive(false);
            ResultObj.SetActive(true);
        }
    }
}

