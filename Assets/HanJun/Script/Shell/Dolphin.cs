using UnityEngine;
using DG.Tweening;
using System.Collections;

namespace Jun
{
    public class Dolphin : ProductionEvent
    {
        [SerializeField] private Transform startPoint;
        [SerializeField] private Transform midPoint;
        [SerializeField] private Transform endPoint; 
        [SerializeField] private Ease ease;

        private Coroutine coroutine = null;

        public void PlayAnimation()
        {
            SoundManager.Instance.PlayNarration("38_dolphin");
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                coroutine = StartCoroutine(DelayEvent());
            }
            else
            {
                coroutine = StartCoroutine(DelayEvent());
            }
        }

        private IEnumerator DelayEvent()
        {
            // 돌고래 시작 위치를 startPoint로 설정
            transform.position = startPoint.position;

            // startPoint → midPoint 이동
            var tween1 = transform.DOPath(
                new Vector3[] { midPoint.position }, 
                3.5f, 
                PathType.CatmullRom // 부드러운 곡선 이동
            ).SetEase(ease);

            yield return tween1.WaitForCompletion();

            // midPoint 도착 시 이벤트 실행 및 대기
            onComplete?.Invoke();
            yield return new WaitForSeconds(0.5f);

            // midPoint → endPoint 이동
            var tween2 = transform.DOPath(
                new Vector3[] { endPoint.position }, 
                3.5f, 
                PathType.CatmullRom // 부드러운 곡선 이동
            ).SetEase(ease);

            yield return tween2.WaitForCompletion();

            // 이동 완료 후 객체 비활성화
            gameObject.SetActive(false);
        }
    }
}
