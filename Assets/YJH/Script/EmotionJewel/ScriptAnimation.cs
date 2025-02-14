using UnityEngine;
using DG.Tweening;

namespace YJH
{
    public class ScriptAnimation : MonoBehaviour
    {
        [SerializeField] private float scaleMultiplier = 1.2f; // 커지는 배율
        [SerializeField] private float duration = 0.5f; // 애니메이션 지속 시간

        private Vector3 originalScale;

        void Start()
        {
            originalScale = transform.localScale;
        }

        public void PlayScaleAnimation()
        {
            transform.DOScale(originalScale * scaleMultiplier, duration / 2)
                .SetEase(Ease.OutQuad)
                .OnComplete(() =>
                    transform.DOScale(originalScale, duration / 2).SetEase(Ease.InQuad)
                );
        }
    }
}