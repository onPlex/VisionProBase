using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using YJH;

namespace Jun
{
    public class TreeEvent : ProductionEvent
    {
        [SerializeField] private GameObject treeSmall;
        [SerializeField] private GameObject treeBig;

        public void PlayAnimationBig()
        {
            treeSmall.SetActive(false);
            treeBig.SetActive(true);
            StartCoroutine(DelayEventBig());
        }

        private IEnumerator DelayEventBig()
        {
            yield return new WaitForSeconds(4f);
            onComplete?.Invoke();
        }



        public void PlayAnimation()
        {
            treeSmall.SetActive(true);          
            StartCoroutine(DelayEvent());
        }

        private IEnumerator DelayEvent()
        {
            yield return new WaitForSeconds(4f);
            onComplete?.Invoke();
        }

    }
}