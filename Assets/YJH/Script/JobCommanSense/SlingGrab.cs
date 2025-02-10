using UnityEngine;
using UnityEngine.UI;
using PolySpatial.Samples;


namespace YJH.JobCommonSense
{
    [RequireComponent(typeof(PieceSelectionBehavior))]
    public class SlingGrab : MonoBehaviour
    {
        [Header("UI")]
        public Slider powerGauge; // 파워 게이지 (Optional)

        // ▼ 새롭게 추가: 발사 대상 바스켓 오브젝트들
        [Header("Basket Targets")]
        public GameObject Basket_O;
        public GameObject Basket_X;

        [Header("Return Settings")]
        [Tooltip("원래 위치로 복귀하는 속도 계수")]
        public float returnSpeed = 20f;

        [Header("Power Gauge Settings")]
        [Tooltip("오브젝트를 당길 수 있는 최대 거리(슬라이더 100%)")]
        public float maxPullDistance = 3f;

        [Header("Bullet Settings")]
        public GameObject bulletPrefab;   // 발사할 총알 프리팹
        public float bulletSpeed = 10f;   // 총알 속도(또는 힘 계수)

        private PieceSelectionBehavior m_PieceSelectionBehavior;

        // 시작 시점의 위치/회전값을 저장
        private Vector3 m_InitialPosition;
        private Quaternion m_InitialRotation;

        // 이전 프레임에 선택 상태였는지 추적
        private bool m_PrevSelected;

        void Awake()
        {
            m_PieceSelectionBehavior = GetComponent<PieceSelectionBehavior>();
        }

        void Start()
        {
            // 초기 위치/회전을 기록
            m_InitialPosition = transform.position;
            m_InitialRotation = transform.rotation;

            // 시작 시점에 슬라이더 초기화
            if (powerGauge)
                powerGauge.value = 0f;

            // 현재 상태를 m_PrevSelected에 동기화
            m_PrevSelected = m_PieceSelectionBehavior.IsCurrentlySelected;
        }

        void Update()
        {
            bool isSelected = m_PieceSelectionBehavior.IsCurrentlySelected;

            // (A) 선택되고 있는 동안 - 파워 게이지 업데이트
            if (isSelected)
            {
                float distance = Vector3.Distance(transform.position, m_InitialPosition);
                // 0~1 범위로 정규화
                float normalizedValue = distance / maxPullDistance;
                normalizedValue = Mathf.Clamp01(normalizedValue);

                if (powerGauge)
                    powerGauge.value = normalizedValue;
            }
            // (B) 선택이 해제된 동안 - 원위치로 복귀
            else
            {
                // 1) 필요하다면 파워 게이지를 0으로 되돌리기
                if (powerGauge)
                    powerGauge.value = 0f;

                // 2) 오브젝트를 서서히 원위치로 복귀
                transform.position = Vector3.Lerp(
                    transform.position,
                    m_InitialPosition,
                    Time.deltaTime * returnSpeed
                );
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    m_InitialRotation,
                    Time.deltaTime * returnSpeed
                );
            }

            // (C) "놓는 순간" 감지 후 총알 발사
            // 이전 프레임에는 true였고, 이번 프레임에 false라면 "놓았다"는 의미
            if (m_PrevSelected && !isSelected)
            {
                FireBullet();
            }

            // Update 끝에 현재 선택 상태를 저장
            m_PrevSelected = isSelected;
        }

        void FireBullet()
        {
            if (!bulletPrefab)
            {
                Debug.LogWarning("[SlingGrab] Bullet prefab is not assigned!");
                return;
            }

            // 1) 총알 생성
            GameObject bulletObj = Instantiate(bulletPrefab, transform.position, transform.rotation);

            // 2) Ball 컴포넌트 가져오기
            var bullet = bulletObj.GetComponent<Ball>();
            if (!bullet)
            {
                Debug.LogWarning("[SlingGrab] Bullet prefab has no Ball component!");
                return;
            }

            // 3) 슬링 당긴 거리
            float distance = Vector3.Distance(transform.position, m_InitialPosition);

            // 4) Basket 선택 로직: X축 비교
            //    "현재 X좌표" vs "초기 위치 X좌표"
            GameObject chosenTarget = null;
            if (transform.position.x > m_InitialPosition.x)
            {
                chosenTarget = Basket_O;
            }
            else
            {
                chosenTarget = Basket_X;
            }

            if (chosenTarget != null)
            {
                // 유도 모드
                bullet.isGuided = true;

                // Ball에서 target으로 이동하도록 설정
                bullet.target = chosenTarget.transform;
            }
            else
            {
                // 만약 이 로직에서 chosenTarget이 null일 일은 없지만
                // 가정상 null 처리
                bullet.isGuided = false;
            }

            // 5) 발사 방향 계산(물리 기반으로 활용하지는 않지만, Launch 시 매개변수 필요)
            Vector3 direction = (m_InitialPosition - transform.position).normalized;

            // 6) Ball 클래스 Launch 호출
            bullet.Launch(direction, distance);

            // + 추가적으로 bulletSpeed도 고려하려면, distance를 더 스케일링할 수도 있음
            // bullet.Launch(direction, distance * bulletSpeedFactor); 등

        }
    }
}