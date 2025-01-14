using System.Collections;
using UnityEngine;

namespace Jun
{
    public class PotionEvent : ProductionEvent
    {
        private Animator animator;

        public void PlayAnimation()
        {
            /// 포션 애니매이션 재생
            StartCoroutine(DelayEvent());
        }

        private IEnumerator DelayEvent()
        {
            yield return new WaitForSeconds(2f);
            // yield return new WaitForSeconds(GetAnimationClipLength(animator, ""));

            this.gameObject.SetActive(false);

            ///애니매이션 재생 완료 후 이벤트
            onComplete?.Invoke();
        }
    }
}