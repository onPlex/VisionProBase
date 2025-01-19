using System.Collections;
using UnityEngine;


namespace YJH
{
    public class SunflowerFairy : MonoBehaviour
    {
        [Header("Target Info")]
        [SerializeField] private Transform target;           // 이동할 목적지
        [SerializeField] private DialogueButton dialogueButton; // 이벤트 완료 알릴 DialogueButton

        [Header("Timing Settings")]
        [SerializeField] private float moveDuration = 2f;      // 타겟까지 이동/회전에 걸리는 시간(초)
        [SerializeField] private float rotateBackDuration = 1f; // 원래 방향 복귀 회전에 걸리는 시간(초)

        // 초기 바라보는 방향(회전값)을 기억해두기 위함
        private Quaternion originalRotation;

        void Awake()
        {
            // 현재 오브젝트가 바라보는 초기 방향 저장
            originalRotation = transform.rotation;
        }

        /// <summary>
        /// DialogueButton의 EventOnTrigger에서 이 함수를 연결해서 호출하면 된다.
        /// </summary>
        public void MoveToTargetAndCompleteEvent()
        {
            StartCoroutine(MoveRoutine());
        }

        /// <summary>
        /// 타겟까지 이동 후, 원래 방향으로 복귀. 그 후 MarkEventComplete() 호출.
        /// </summary>
        private IEnumerator MoveRoutine()
        {
            // 1) 이동에 필요한 준비
            Vector3 startPos = transform.position;         // 시작 위치
            Vector3 endPos = target.position;            // 도착 위치
            Quaternion startRot = transform.rotation;         // 이동 전 현재 회전
            Quaternion targetRot = Quaternion.LookRotation(endPos - startPos, Vector3.up);
            // 단순화: 목표 방향을 바라보도록 회전 (Yaw 중심)

            // 2) 목적지까지 이동하며 방향 전환
            float timer = 0f;
            while (timer < moveDuration)
            {
                timer += Time.deltaTime;
                float t = Mathf.Clamp01(timer / moveDuration);

                // 위치 보간
                transform.position = Vector3.Lerp(startPos, endPos, t);
                // 회전 보간 (시작 회전 → targetRot)
                transform.rotation = Quaternion.Slerp(startRot, targetRot, t);

                yield return null;
            }

            // 도착 지점에 정확히 맞춰주기
            transform.position = endPos;
            transform.rotation = targetRot;

            // (예시) 잠깐 대기 - 필요하다면 여기서 추가 지연 시간도 변수화 가능
            //yield return new WaitForSeconds(0.1f);

            // 3) 원래 방향으로 회전 복귀
            Quaternion currentRot = transform.rotation;
            timer = 0f;
            while (timer < rotateBackDuration)
            {
                timer += Time.deltaTime;
                float t = Mathf.Clamp01(timer / rotateBackDuration);

                transform.rotation = Quaternion.Slerp(currentRot, originalRotation, t);

                yield return null;
            }
            transform.rotation = originalRotation;  // 최종 보정

            // 4) 모든 작업이 끝난 뒤 -> DialogueButton에 이벤트 완료 신호 알림
            if (dialogueButton != null)
            {
                dialogueButton.MarkEventComplete();
            }
            else
            {
                Debug.LogWarning("DialogueButton reference is missing on SunflowerFairy.");
            }
        }
    }
}