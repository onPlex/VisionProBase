using System.Collections;
using UnityEngine;

namespace Jun
{
    public class TreasureBox : ProductionEvent
    {
        [SerializeField] private Animator animator;

        public void PlayAnimation()
        {
            animator.SetBool("IsOpen", true);
            StartCoroutine(DelayEvent());
        }

        private IEnumerator DelayEvent()
        {
            yield return new WaitForSeconds(GetAnimationClipLength(animator, "TreasureBox_ani_open"));
            //yield return new WaitForSeconds(2f);
            onComplete?.Invoke();
        }
    }
}