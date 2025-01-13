using UnityEngine;
using System.Collections;
using UnityEngine.Events;

namespace Jun
{
    public class ShellThrow : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private float _height = 5f;
        [SerializeField] private float _duration = 1.5f;

        private bool _isThrowing = false;
        private Coroutine _throwCoroutine = null;
        public void Throw(UnityAction onComplete = null)
        {
            if (!_isThrowing)
            {
                if (_throwCoroutine == null) StartCoroutine(ThrowCoroutine(onComplete));
                else StopCoroutine(_throwCoroutine);
            }
        }

        private IEnumerator ThrowCoroutine(UnityAction onComplete)
        {
            _isThrowing = true;

            Vector3 startPoint = transform.position; // 시작 위치
            Vector3 endPoint = _target.position; // 바구니 위치

            float elapsedTime = 0f;

            while (elapsedTime < _duration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / _duration;

                // 포물선 계산
                Vector3 currentPos = CalculateParabola(startPoint, endPoint, _height, t);
                transform.position = currentPos;

                yield return null; // 한 프레임 대기
            }

            // 이동 완료 후 위치를 정확히 타겟에 맞춤
            transform.position = endPoint;
            _isThrowing = false;
            onComplete?.Invoke();
        }

        private Vector3 CalculateParabola(Vector3 start, Vector3 end, float height, float t)
        {
            // 직선 위치 계산
            Vector3 midPoint = Vector3.Lerp(start, end, t);

            // 높이 추가
            float parabola = 4 * height * (t - t * t); // 포물선의 높이 계산
            return new Vector3(midPoint.x, midPoint.y + parabola, midPoint.z);
        }
    }
}