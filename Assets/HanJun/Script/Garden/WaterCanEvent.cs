using System.Collections;
using UnityEngine;
using YJH;

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

        void OnEnable()
        {
            PlayAnimation();
        }

        public void PlayAnimation()
        {
            animator.SetTrigger("Water");
            StartCoroutine(DelayEvent());
        }

        private IEnumerator DelayEvent()
        {
            AudioManager.Instance.PlaySFX("soundevent6");
           
            yield return new WaitForSeconds(GetAnimationClipLength(animator, "gardening_watercan_ani_water"));
            onComplete?.Invoke();
        }


    }
}