using UnityEngine;
using TMPro;
using DG.Tweening; // DOTween 네임스페이스 추가

namespace YJH.EmotionJewel
{
    public class AnswerButton : MonoBehaviour
    {
        [SerializeField]
        Animator animator;
        [SerializeField]
        TMP_Text tMProText;

        [SerializeField]
        private float fadeDuration = 1f; // 페이드 효과 시간

        public void OnClickEffect()
        {
            if (animator) animator.SetTrigger("Play");

            if (tMProText != null)
            {
                // 알파 값 0(FadeOut) → 알파 값 1(FadeIn)
                tMProText.DOFade(0f, fadeDuration) // 2초 동안 페이드 아웃
                    .OnComplete(() =>
                    {
                        tMProText.DOFade(1f, fadeDuration); // 2초 동안 페이드 인
                    });
            }
        }
    }
}
