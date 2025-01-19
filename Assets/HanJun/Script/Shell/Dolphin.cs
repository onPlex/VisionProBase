using UnityEngine;
using DG.Tweening;
using System.Collections;

namespace Jun
{
    public class Dolphin : ProductionEvent
    {
        [SerializeField] private Transform target;
        [SerializeField] private Ease ease;

        public void PlayAnimation()
        {
            StartCoroutine(DelayEvent());
        }

        private IEnumerator DelayEvent()
        {
            var tween = transform.DOMove(target.position, 3f).SetEase(ease);
            yield return tween.WaitForCompletion();
            onComplete?.Invoke();
        }
    }
}