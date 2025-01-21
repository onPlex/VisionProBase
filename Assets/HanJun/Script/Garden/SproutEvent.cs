using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using YJH;

namespace Jun
{
    public class SproutEvent : ProductionEvent
    {
        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            PlayScaleAnimation();
        }

        private void PlayScaleAnimation()
        {
            animator.SetTrigger("Scale");
            StartCoroutine(DelayAnimationEvent());
        }

        private IEnumerator DelayAnimationEvent()
        {
           
            yield return new WaitForSeconds(GetAnimationClipLength(animator, "Sprout_ani"));
            onComplete?.Invoke();
        }
    }
}