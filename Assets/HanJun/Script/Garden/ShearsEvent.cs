using System.Collections;
using UnityEngine;
using YJH;

namespace Jun
{
    public class ShearsEvent : ProductionEvent
    {
        [SerializeField] private GameObject[] shears;
        [SerializeField] private GameObject[] grass;
        [SerializeField] private Animator[] animator;

        void OnEnable()
        {
            PlayAnimation();
        }

        public void PlayAnimation()
        {
            StartCoroutine(DelayEvent());
        }

        private IEnumerator DelayEvent()
        {
            shears[0].SetActive(true);

            // yield return new WaitForSeconds(GetAnimationClipLength(animator[0], "gardening_shears_ani_cut_Notmove"));
            yield return new WaitForSeconds(2f);
            shears[0].SetActive(false);
            shears[1].SetActive(true);

            grass[0].SetActive(false);
            // yield return new WaitForSeconds(GetAnimationClipLength(animator[1], "gardening_shears_ani_cut_Notmove"));
            yield return new WaitForSeconds(2f);
            grass[1].SetActive(false);
            shears[1].SetActive(false);
            yield return new WaitForSeconds(0.5f);

            onComplete?.Invoke();
        }

        public void PlaySoundEffect()
        {
            AudioManager.Instance.PlaySFX("soundevent7");
        }
    }
}