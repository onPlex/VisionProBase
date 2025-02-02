using UnityEngine;


namespace YJH.MajorHunting
{
    public class PhaseManager : MonoBehaviour
    {
        [Header("Title")]
        [SerializeField]
        private GameObject TitleObj;

        [Header("Intro")]
        [SerializeField]
        private GameObject IntroObj;

        [Header("Main1")]
        [SerializeField]
        private GameObject Main1Obj;
        private GameObject Main1PopUp;


        //Title GameStart Button
        public void OnStepToIntro()
        {
            TitleObj.SetActive(false);
            IntroObj.SetActive(true);
        }

        public void OnStepToMain1()
        {
            IntroObj.SetActive(false);
            Main1Obj.SetActive(true);
        }

        //Close Main1 Popup
        public void OnStepToMain1Content()
        {
            Main1PopUp.SetActive(false);
        }
    }
}