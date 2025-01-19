using UnityEngine;
using System.Collections;
using UnityEngine.Events;

namespace Jun
{
    public class ShellThrow : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private float _height = 5f;
        // [SerializeField] private float _duration = 1.5f;

        private Vector3 originalPosition; // 조개의 원래 위치
        private bool isMoving = false; // 중복 실행 방지

        private Coroutine throwCoroutine = null;
        private Coroutine returnCoroutine = null;

        void Awake()
        {
            originalPosition = transform.position; // 초기 위치 저장
        }

        // 바구니로 던지는 메서드
        public void ThrowToTarget(UnityAction onComplete = null, float duration = 1.5f)
        {
            if (!isMoving)
            {
                if (throwCoroutine != null)
                {
                    StopCoroutine(throwCoroutine);
                    throwCoroutine = StartCoroutine(MoveToTarget(_target.position, onComplete, duration));
                }
                else throwCoroutine = StartCoroutine(MoveToTarget(_target.position, onComplete, duration));
            }
        }

        // 제자리로 돌아가는 메서드
        public void ReturnToOriginal(UnityAction onComplete = null, float duration = 0f)
        {
            if (!isMoving)
            {
                if (returnCoroutine != null)
                {
                    StopCoroutine(returnCoroutine);
                    returnCoroutine = StartCoroutine(MoveToTarget(originalPosition, onComplete, duration));
                }
                else returnCoroutine = StartCoroutine(MoveToTarget(originalPosition, onComplete, duration));
            }
        }

        private IEnumerator MoveToTarget(Vector3 destination, UnityAction onComplete, float duration)
        {
            isMoving = true;

            Vector3 startPoint = transform.position; // 현재 위치를 시작점으로 설정
            Vector3 endPoint = destination; // 도착점 설정
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / duration;

                // 포물선 계산
                Vector3 currentPos = CalculateParabola(startPoint, endPoint, _height, t);
                transform.position = currentPos;

                yield return null; // 한 프레임 대기
            }

            // 이동 완료 후 정확한 위치 설정
            transform.position = endPoint;
            isMoving = false;
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