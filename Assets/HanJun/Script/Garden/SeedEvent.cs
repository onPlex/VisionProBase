using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using YJH;

namespace Jun
{
    public class SeedEvent : ProductionEvent
    {
        [SerializeField] private ParticleSystem particle;
        public float tiltDuration = 2f; // 기울어지는 데 걸리는 시간
        private float maxTiltAngle = -90f; // 최대 기울기 각도

        private bool isTilting = false; // 기울이기 상태 확인용

        // 기울어지는 연출을 시작하는 함수
        public void StartTiltAndThrow()
        {
            if (!isTilting)
            {
                StartCoroutine(TiltAndThrow());
            }
        }

        // 코루틴으로 기울이기와 Sphere 생성 및 포물선 이동 구현
        private IEnumerator TiltAndThrow()
        {
            isTilting = true;

            // 초기 회전 값
            Quaternion startRotation = transform.rotation;
            Quaternion endRotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, maxTiltAngle);

            float elapsedTime = 0f;

            // 지정된 시간 동안 기울이기
            while (elapsedTime < tiltDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / tiltDuration;

                // 선형 보간으로 회전
                transform.rotation = Quaternion.Lerp(startRotation, endRotation, t);

                yield return null;
            }

            AudioManager.Instance.PlaySFX("soundevent5");
            yield return new WaitForSeconds(0.1f);
            AudioManager.Instance.PlaySFX("soundevent5");
            yield return new WaitForSeconds(0.1f);
            AudioManager.Instance.PlaySFX("soundevent5");
            isTilting = false;
            particle.Play();
            AudioManager.Instance.PlaySFX("soundevent5");
            yield return new WaitForSeconds(2f);
            AudioManager.Instance.PlaySFX("soundevent5");
            onComplete?.Invoke();

        }

        // Sphere 생성 및 포물선 이동
        // private IEnumerator ThrowSeed()
        // {
        //     // Sphere 생성
        //     GameObject sphere = Instantiate(spherePrefab, spawnPoint.position, Quaternion.identity);

        //     // 초기 위치와 목표 위치 설정
        //     Vector3 startPoint = spawnPoint.position;
        //     Vector3 endPoint = startPoint + (transform.forward * 5f); // 던지는 방향으로 목표 지점 설정
        //     float elapsedTime = 0f;
        //     float duration = 1f; // 던지는 데 걸리는 시간

        //     // 포물선을 그리며 이동
        //     while (elapsedTime < duration)
        //     {
        //         elapsedTime += Time.deltaTime;

        //         // Lerp를 사용해 좌우 이동 보간
        //         float t = elapsedTime / duration;
        //         Vector3 position = Vector3.Lerp(startPoint, endPoint, t);

        //         // 포물선의 높이를 추가
        //         position.y += arcHeight * Mathf.Sin(t * Mathf.PI);

        //         sphere.transform.position = position;

        //         yield return null;
        //     }

        //     // 바닥에 도착한 후 Rigidbody 활성화 (추가 물리 효과)
        //     Rigidbody rb = sphere.GetComponent<Rigidbody>();
        //     if (rb != null)
        //     {
        //         rb.isKinematic = false; // 물리 활성화
        //     }

        //     onComplete?.Invoke();
        // }
    }
}