using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Jun
{
    public class TreeEvent : ProductionEvent
    {
        [SerializeField] private GameObject treeSmall;

        public void PlayAnimation()
        {
            treeSmall.SetActive(true);
            StartCoroutine(DelayEvent());
        }

        private IEnumerator DelayEvent()
        {
            yield return new WaitForSeconds(2f);
            onComplete?.Invoke();
        }

    }
}