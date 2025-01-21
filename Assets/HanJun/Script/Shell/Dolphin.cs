using UnityEngine;
using DG.Tweening;
using System.Collections;
using Unity.Entities.UniversalDelegates;

namespace Jun
{
    public class Dolphin : ProductionEvent
    {
        [SerializeField] private Transform target;
        [SerializeField] private Transform target2;
        [SerializeField] private GameObject particle = null;
        [SerializeField] private Ease ease;

        private Coroutine coroutine = null;

        public void PlayAnimation()
        {
            SoundManager.Instance.PlayNarration("38_dolphin");
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                coroutine = StartCoroutine(DelayEvent());
            }
            else
            {
                coroutine = StartCoroutine(DelayEvent());
            }

        }

        private IEnumerator DelayEvent()
        {
            var tween = transform.DOMove(target.position, 7f).SetEase(ease);
            yield return tween.WaitForCompletion();
            if (particle != null) particle.SetActive(true);
            onComplete?.Invoke();
            yield return new WaitForSeconds(0.5f);
            var tween2 = transform.DOMove(target2.position, 2f).SetEase(ease);
            yield return tween2.WaitForCompletion();
            gameObject.SetActive(false);
        }
    }
}