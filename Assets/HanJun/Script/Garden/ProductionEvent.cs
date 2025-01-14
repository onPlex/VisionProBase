using UnityEngine;
using UnityEngine.Events;

namespace Jun
{
    public class ProductionEvent : MonoBehaviour
    {
        [SerializeField] protected UnityEvent onComplete;
        protected float GetAnimationClipLength(Animator anim, string clipName)
        {
            if (anim == null || anim.runtimeAnimatorController == null)
            {
                Debug.LogWarning("Animator 또는 AnimatorController가 설정되지 않았습니다.");
                return -1f;
            }

            // AnimatorController에서 모든 AnimationClip 가져오기
            AnimationClip[] clips = anim.runtimeAnimatorController.animationClips;
            foreach (AnimationClip clip in clips)
            {
                if (clip.name == clipName)
                {
                    return clip.length; // 애니메이션 클립 길이 반환
                }
            }

            return -1f; // 해당 이름의 애니메이션 클립을 찾을 수 없을 때
        }

    }
}
