using UnityEngine;

namespace YJH
{
    public enum EContent_Phase
    {
        Title,
        Prologue,  //(Stage1 Start)
        Stage1,
        Stage1End,
        Stage2Start,
        Stage2,
        Stage2End,
        Stage3Start,
        Stage3,
        Stage3End,
        Stage4Start,
        Stage4,
        Stage4End,
        Result
    }


    public class ContentPhaseManager : MonoBehaviour
    {

        EContent_Phase eContent_Phase;
        [Header("Title")]
        [SerializeField]
        GameObject TitleObj;

        [Header("Prologue")]
        [SerializeField]
        GameObject PrologueObj;
        [SerializeField]
        GameObject Stage1StartButton;

        [Header("Stage1")]
        [SerializeField]
        GameObject Stage1Obj;

        [Header("Stage1End")]
        [SerializeField]
        GameObject Stage1EndObj;

        [Header("Stage2Start")]
        [SerializeField]
        GameObject Stage2StartObj;

        [Header("Stage2")]
        [SerializeField]
        GameObject Stage2Obj;

        [Header("Stage2End")]
        [SerializeField]
        GameObject Stage2EndObj;

        [Header("Stage3Start")]
        [SerializeField]
        GameObject Stage3StartObj;

        [Header("Stage3")]
        [SerializeField]
        GameObject Stage3Obj;

        [Header("Stage3End")]
        [SerializeField]
        GameObject Stage3EndObj;


        [Header("Stage4Start")]
        [SerializeField]
        GameObject Stage4StartObj;

        [Header("Stage4")]
        [SerializeField]
        GameObject Stage4Obj;

        [Header("Stage4End")]
        [SerializeField]
        GameObject Stage4EndObj;



        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            InitPhase();
        }

        /// <summary>
        ///Prologue
        /// </summary>
        public void OnGameStart()
        {
            if (eContent_Phase == EContent_Phase.Title)
            {
                eContent_Phase = EContent_Phase.Prologue;
                TitleObj.SetActive(false);
                PrologueObj.SetActive(true);
            }
            else
            {
                Debug.LogWarning("Need To Check EContent_Phase");
            }

        }

        public void OnGamePrologueEnd()
        {
            Stage1StartButton.SetActive(true);
        }

        /// <summary>
        /// Stage1
        /// </summary>
        public void OnStage1Start()
        {
            if (eContent_Phase == EContent_Phase.Prologue)
            {
                eContent_Phase = EContent_Phase.Stage1;
                PrologueObj.SetActive(false);
                Stage1Obj.SetActive(true);
            }
            else
            {
                Debug.LogWarning("Need To Check EContent_Phase");
            }
        }

        public void OnStage1Finish()
        {
            Debug.Log("OnStage1Finish");

            //씨앗&모종삽​ 아이콘 생성​
            //안내 메시지 생성​
            // {} => 모종삽​ 아이콘 터치​  ~ - > 모종삽이 구덩이를 파는 연출​

            //안내 메시지 생성
            //씨앗​ 아이콘 터치​
            // 씨앗이 구덩이에 심어지는 연출​ - '빛 파티클 연출' - 나무 자라는 연출 진행​
        }

        public void OnStage2Start()
        {
            Debug.Log("OnStage2Start");
            //플루트 내래이션 연출​
            //안내 메시지 생성​ - 6가지 문항에 답변하여 아이템을 획득하세요. ​
            //시작하기 버튼 생성
            // 
        }

         public void OnStage2End()
        {
            Debug.Log("OnStage2End");
          
        }

         public void OnStage3Start()
        {
            Debug.Log("OnStage3Start");
            //플루트 내래이션 연출​
            //안내 메시지 생성​ - 6가지 문항에 답변하여 아이템을 획득하세요. ​
            //시작하기 버튼 생성
            // 
        }

         public void OnStage3End()
        {
            Debug.Log("OnStage3End");
          
        }

         public void OnStage4Start()
        {
            Debug.Log("OnStage4Start");
            //플루트 내래이션 연출​
            //안내 메시지 생성​ - 6가지 문항에 답변하여 아이템을 획득하세요. ​
            //시작하기 버튼 생성
            // 
        }

         public void OnStage4End()
        {
            Debug.Log("OnStage4End");
          
        }

        private void InitPhase()
        {
            eContent_Phase = EContent_Phase.Title;
            TitleObj.SetActive(true);
            PrologueObj.SetActive(false);
            Stage1Obj.SetActive(false);
        }
    }
}