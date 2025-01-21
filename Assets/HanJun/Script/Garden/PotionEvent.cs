using System.Collections;
using UnityEngine;
using DG.Tweening;
using NUnit.Framework.Constraints;
using YJH;

namespace Jun
{
    public class PotionEvent : ProductionEvent
    {
        [SerializeField] private GameObject particle;
        private Animator animator;

        public void PlayAnimation()
        {
            /// 포션 애니매이션 재생
            StartCoroutine(DelayEvent());
        }

        private IEnumerator DelayEvent()
        {
            var tween = transform.DORotate(new Vector3(0, 0, 90), 2f);
            yield return tween.WaitForCompletion();
            AudioManager.Instance.PlaySFX("soundevent9");
            particle.SetActive(true);
            yield return new WaitForSeconds(1f);
            AudioManager.Instance.PlaySFX("soundevent9");
            particle.SetActive(false);
            tween = transform.DORotate(new Vector3(0, 0, 0), 1.5f);
            yield return tween.WaitForCompletion();

            this.gameObject.SetActive(false);

            ///애니매이션 재생 완료 후 이벤트
            onComplete?.Invoke();
        }
    }
}