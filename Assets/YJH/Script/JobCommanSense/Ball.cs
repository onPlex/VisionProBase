using UnityEngine;

namespace YJH.JobCommonSense
{


    [RequireComponent(typeof(Rigidbody))]
    public class Ball : MonoBehaviour
    {
        [Header("General")]
        [Tooltip("몇 초 뒤에 자동으로 파괴할지 (0 이하이면 파괴하지 않음)")]
        public float lifetime = 10f;

        [Header("Guided Mode Settings")]
        [Tooltip("True일 경우, Rigidbody를 사용하지 않고 target으로 직선 이동한다.")]
        public bool isGuided = false;

        [Tooltip("유도 모드로 이동할 타겟 (isGuided=true일 때 필요)")]
        public Transform target;

        [Tooltip("유도 모드 이동 시, 당긴 거리(distance)에 곱할 스케일 팩터")]
        public float guidedSpeedFactor = 10f;

        // 내부에서 사용할 참조
        private Rigidbody m_Rigidbody;
        private float m_Speed;          // 유도 모드에서 실제 이동 속도
        private bool m_Launched = false; // 이미 발사(초기화) 되었는지 여부

        void Awake()
        {
            m_Rigidbody = GetComponent<Rigidbody>();
        }

        void Start()
        {
            // lifeTime이 0보다 크면 일정 시간이 지난 뒤 오브젝트 파괴
            if (lifetime > 0f)
                Destroy(gameObject, lifetime);
        }

        void Update()
        {
            // 유도 모드인 경우, 발사 후 매 프레임 타겟까지 이동
            if (isGuided && m_Launched && target != null)
            {
                // 현재 위치에서 타겟 위치로 이동
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    target.position,
                    m_Speed * Time.deltaTime
                );

                // 간단히 타겟에 도달했는지 판정
                float distance = Vector3.Distance(transform.position, target.position);
                if (distance < 0.01f)
                {
                    // 타겟에 도착 시, 필요하다면 추가 처리 (파괴 등)
                    Destroy(gameObject);
                }
            }
        }

        /// <summary>
        /// 슬링으로 당긴 후 발사될 때 호출되는 초기화 함수
        /// </summary>
        /// <param name="direction">물리 기반 발사 시 적용할 방향(정규화).</param>
        /// <param name="power">슬링으로 당긴 거리(또는 파워) 값.</param>
        public void Launch(Vector3 direction, float power)
        {
            // 물리 기반(Rigidbody) 발사 vs 유도 모드로 분기
            if (!isGuided)
            {
                // (1) 기존 물리 기반 발사
                // 적절한 스케일링으로 power를 속력에 반영
                // 예: power가 3이고, baseSpeed가 10이면 -> velocity = 30
                m_Rigidbody.linearVelocity = direction * (power * 10f);
            }
            else
            {
                // 발사 순간, target을 향한 방향을 계산하고 해당 방향+물리로 쏴준다
                if (target != null)
                {
                    Vector3 directionToTarget = (target.position - transform.position).normalized;
                    // (power * guidedSpeedFactor)로 속도를 결정
                    m_Rigidbody.linearVelocity = directionToTarget * (power * guidedSpeedFactor);
                }
                else
                {
                    // 타겟이 없으면 그냥 비유도 방식과 동일하게 처리하거나
                    // 원하는 디폴트 처리를 해도 됨
                    m_Rigidbody.linearVelocity = direction * (power * 10f);
                }
            }

            m_Launched = true;
        }
    }
}