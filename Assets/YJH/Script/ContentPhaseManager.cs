using UnityEngine;

namespace YJH
{
    public enum EContent_Phase
    {
        Title,
        Prologue,
        Tutorial,
        MainContent,
        ContentEnd,
        Result

    }


    public class ContentPhaseManager : MonoBehaviour
    {

        EContent_Phase eContent_Phase;

        [SerializeField]
        GameObject Title;

        [SerializeField]
        GameObject Prologue;

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
    }
}