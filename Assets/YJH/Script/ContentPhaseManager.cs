using UnityEngine;

namespace YJH
{
    /*
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
    }*/


    public class ContentPhaseManager : MonoBehaviour
    {

        //EContent_Phase eContent_Phase;
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
            TitleObj.SetActive(false);
            PrologueObj.SetActive(true);
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
            PrologueObj.SetActive(false);
            Stage1Obj.SetActive(true);
        }

        public void OnStage1End()
        {
            Debug.Log("OnStage1Finish");

            Stage1Obj.SetActive(false);
            Stage1EndObj.SetActive(true);

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

              Stage1EndObj.SetActive(false);
            Stage2StartObj.SetActive(true);
            //플루트 내래이션 연출​
            //안내 메시지 생성​ - 6가지 문항에 답변하여 아이템을 획득하세요. ​
            //시작하기 버튼 생성
            // 
        }

        public void OnStage2()
        {
            Debug.Log("OnStage2");
            Stage2StartObj.SetActive(false);
            Stage2Obj.SetActive(true);

        }

        public void OnStage2End()
        {
            Debug.Log("OnStage2End");
             Stage2Obj.SetActive(false);
            Stage2EndObj.SetActive(true);
            //[물뿌리개] 아이콘이 생성
            //안내 메시지 생성​  - 마법 정원에 해가 뜰 때, 물뿌리개를 들어 땅 쪽으로 기울여 보세요!​
            // 물뿌리개 애니메이션 - 나무에 물을 뿌리는 애니메이션 연출 진행​
            //빛 파티클 연출​ + 어린 나무가 자라서 작은 나무가 되는 연출 진행​ + 주변에 잡초(풀)도 같이 자람​
            // 나무 자람 연출​
            // 연출 완료시 Stage 3 시작
        }

        public void OnStage3Start()
        {
            Debug.Log("OnStage3Start");
            Stage2EndObj.SetActive(false);
            Stage3StartObj.SetActive(true);
            //플루트 내래이션 연출​ ( 흥미 나무를 잘 기르기 위해서는 주변에 자라난 필요 없는 잡초를 과감히 뽑아주어야 해요. ​) , ( 그래야 흥미 나무가 더 아름답고 건강하게 자라거든요.​)
            //안내 메시지 생성​ - 6가지 문항에 답변하여 아이템을 획득하세요. ​
            //시작하기 버튼 생성

        }

        public void OnStage3()
        {
            Debug.Log("OnStage3");
            Stage3StartObj.SetActive(false);
            Stage3Obj.SetActive(true);

        }

        public void OnStage3End()
        {
            Debug.Log("OnStage3End");
             Stage3Obj.SetActive(false);
            Stage3EndObj.SetActive(true);

            //제초 가위​ 아이콘 생성​
            // 안내 메시지 생성​ ( 제초 가위로 잡초를 잘라주세요.​)
            // 제초 가위​  애니메이션 생성​ (나무 주변으로 풀이 자란 상태에서 나무 빼고 나머지를제거하는 연출​ ) + (나무 주변으로 풀이 자란 상태에서 나무 빼고 나머지를제거하는 연출​)
            //빛 파티클 연출
            // 연출 완료시 Stage 4 시작
        }

        public void OnStage4Start()
        {
            Debug.Log("OnStage4Start");
            Stage3EndObj.SetActive(false);
            Stage4StartObj.SetActive(true);
            //플루트 내래이션 연출​
            //안내 메시지 생성​ - 6가지 문항에 답변하여 아이템을 획득하세요. ​
            //시작하기 버튼 생성
            // 
        }

        public void OnStage4()
        {
            Debug.Log("OnStage4");
            Stage4StartObj.SetActive(false);
            Stage4Obj.SetActive(true);
        }

        public void OnStage4End()
        {
            Debug.Log("OnStage4End");
            Stage4Obj.SetActive(false);
            Stage4EndObj.SetActive(true);
        }

        private void InitPhase()
        {
            // eContent_Phase = EContent_Phase.Title;
            TitleObj.SetActive(true);
            PrologueObj.SetActive(false);
            Stage1Obj.SetActive(false);
        }
    }
}