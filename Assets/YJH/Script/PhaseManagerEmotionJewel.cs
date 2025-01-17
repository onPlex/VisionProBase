using System.Collections;
using UnityEngine;
using TMPro;

namespace YJH
{
    public class PhaseManagerEmotionJewel : ContentPhaseManager
    {
        [Header("DebugTEXT")]
        [SerializeField]
        TMP_Text resultDebugText;
        //[SerializeField]
        //private GameObject ResultDebugTextRootGameObj;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
           // ResultDebugTextRootGameObj.SetActive(false); // forDEBUG
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
            Stage1Obj.SetActive(false);
            AudioManager.Instance.PlaySFX("TutorialEnd1");
           // ResultDebugTextRootGameObj.SetActive(true); // forDEBUG
            yield return new WaitForSecondsRealtime(10f);

            Debug.Log("OnStage2");
          //  ResultDebugTextRootGameObj.SetActive(false); // forDEBUG
            Stage2Obj.SetActive(true);
            yield return null;
        }

        //TutorialEnd2
        public IEnumerator IStageFinishEvent2()
        {
            Stage2Obj.SetActive(false);
            AudioManager.Instance.PlaySFX("TutorialEnd2");
           // ResultDebugTextRootGameObj.SetActive(true); // forDEBUG
            yield return new WaitForSecondsRealtime(10f);

            Debug.Log("OnStage3");
           // ResultDebugTextRootGameObj.SetActive(false); // forDEBUG
            Stage3Obj.SetActive(true);
            yield return null;
        }



        //TutorialEnd3
        public IEnumerator IStageFinishEvent3()
        {
            Stage3Obj.SetActive(false);
            AudioManager.Instance.PlaySFX("TutorialEnd3");
           // ResultDebugTextRootGameObj.SetActive(true); // forDEBUG
            yield return new WaitForSecondsRealtime(10f);

            Debug.Log("OnStage4");
            //ResultDebugTextRootGameObj.SetActive(false); // forDEBUG
            Stage4Obj.SetActive(true);
            yield return null;
        }


        //TutorialEnd4
        public IEnumerator IStageFinishEvent4()
        {
            Stage4Obj.SetActive(false);
            AudioManager.Instance.PlaySFX("TutorialEnd4");
           // ResultDebugTextRootGameObj.SetActive(true); // forDEBUG
            yield return new WaitForSecondsRealtime(10f);

            Debug.Log("OnStage4");
           // ResultDebugTextRootGameObj.SetActive(false); // forDEBUG
            ResultObj.SetActive(true);
            yield return null;
        }
        public override void OnResult()
        {
            Debug.Log("OnResult");
            StartCoroutine(IStageFinishEvent4());
        }
    }
}

