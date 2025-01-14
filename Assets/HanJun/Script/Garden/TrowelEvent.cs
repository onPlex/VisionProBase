using UnityEngine;
using YJH;
using UnityEngine.Events;
using System.Collections;

namespace Jun
{
    public class TrowelEvent : ProductionEvent
    {
        private Animator animator;

        private Coroutine playAnim = null;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void PlayAnimation()
        {
            if (playAnim != null)
            {
                StopCoroutine(playAnim);
                playAnim = StartCoroutine(PlayAnimationCoroutine());
            }
            else
            {
                playAnim = StartCoroutine(PlayAnimationCoroutine());
            }
        }

        private IEnumerator PlayAnimationCoroutine()
        {
            animator.SetBool("IsPlay", true);
            yield return new WaitForSeconds(GetAnimationClipLength(animator, "gardening_trowel_ani_digging") * 3);
            animator.SetBool("IsPlay", false);
            onComplete?.Invoke();
        }

    }
}