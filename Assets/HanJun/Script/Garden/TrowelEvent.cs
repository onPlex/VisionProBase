using UnityEngine;
using YJH;
using UnityEngine.Events;
using System.Collections;

namespace Jun
{
    public class TrowelEvent : MonoBehaviour
    {
        [SerializeField] UnityEvent onComplete;
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
            yield return new WaitForSeconds(3f);
            animator.SetBool("IsPlay", false);
            onComplete?.Invoke();
        }

    }
}