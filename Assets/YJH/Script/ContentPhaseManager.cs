using UnityEngine;
using UnityEngine.Rendering;

namespace YJH
{
    public enum EContent_Phase
    {
        Title,
        Prologue,
        MainContent,
        ContentEnd,
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
            eContent_Phase = EContent_Phase.Title;
        }

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

        public void OnGameMainContentStart()
        {
            if (eContent_Phase == EContent_Phase.Prologue)
            {
                eContent_Phase = EContent_Phase.MainContent;
                Prologue.SetActive(false);
                MainContent.SetActive(true);
            }
            else
            {
                Debug.LogWarning("Need To Check EContent_Phase");
            }
        }
    }
}