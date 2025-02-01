using System.Collections;
using UnityEngine;

namespace Jun
{
    public class JobCommonSenseController : MonoBehaviour
    {
        [SerializeField] private GameObject[] Phases;
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

            Phases[index].SetActive(true);
        }

        private IEnumerator StartTimerCoroutine()
        {
            yield return new WaitForSeconds(roundTimer);
            SetPhaseIndex(3);
        }
    }
}