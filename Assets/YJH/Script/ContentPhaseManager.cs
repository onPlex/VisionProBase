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
        GameObject Title;

        [Header("Prologue")]
        [SerializeField]
        GameObject Prologue;
        [SerializeField]
        GameObject MainContentStartButton;

        [Header("MainContent")]
        [SerializeField]
        GameObject MainContent;

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
                Title.SetActive(false);
                Prologue.SetActive(true);
            }
            else
            {
                Debug.LogWarning("Need To Check EContent_Phase");
            }

        }

        public void OnGamePrologueEnd()
        {
            MainContentStartButton.SetActive(true);
        }

/// <summary>
/// Stage1
/// </summary>
        public void OnStage1Start()
        {
            if (eContent_Phase == EContent_Phase.Prologue)
            {
                eContent_Phase = EContent_Phase.Stage1;
                Prologue.SetActive(false);
                MainContent.SetActive(true);
            }
            else
            {
                Debug.LogWarning("Need To Check EContent_Phase");
            }
        }

        private void InitPhase()
        {
            eContent_Phase = EContent_Phase.Title;
            Title.SetActive(true);
            Prologue.SetActive(false);
            MainContent.SetActive(false);
        }
    }
}