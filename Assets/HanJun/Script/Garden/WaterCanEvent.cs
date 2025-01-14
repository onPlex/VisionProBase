using System.Collections;
using UnityEngine;

namespace Jun
{
    public class WaterCanEvent : ProductionEvent
    {

        private Animator animator;

        //gardening_watercan_ani_water

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void PlayAnimation()
        {
            animator.SetTrigger("Water");
            StartCoroutine(DelayEvent());
        }

        private IEnumerator DelayEvent()
        {
            yield return new WaitForSeconds(GetAnimationClipLength(animator, "gardening_watercan_ani_water"));
            onComplete?.Invoke();
        }


    }
}