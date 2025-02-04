using System.Collections;
using UnityEngine;

namespace Jun
{
    public class JobCommonSenseController : MonoBehaviour
    {
        [SerializeField] private GameObject[] Phases;
        [SerializeField] private GameObject SlingShot;
        [SerializeField] private float roundTimer = 2f;

        private void Start()
        {
            SetPhaseIndex(0);
        }

        public void SetPhaseIndex(int index = -1)
        {
            foreach (var obj in Phases)
                obj.SetActive(false);

            if (index == -1) return;

            // Tutorial
            if (index == 2 || index == 4) StartRound();
            if (index == 5) SlingShot.SetActive(false);

            Phases[index].SetActive(true);


        }

        public void StartRound()
        {
            StartCoroutine(StartTimerCoroutine());
        }

        private IEnumerator StartTimerCoroutine()
        {
            SlingShot.SetActive(false);
            yield return new WaitForSeconds(roundTimer);
            SetPhaseIndex(3);
            SlingShot.SetActive(true);
        }
    }
}