using UnityEngine;
using UnityEngine.UI;


namespace Jun
{
    // [RequireComponent(typeof(Rigidbody))]
    public class BallSelectionBehavior : MonoBehaviour
    {
        [Header("UI & Power Settings")]
        public Slider powerGauge;            // 파워 게이지
        public float maxChargeTime = 2f;     // 게이지 최대 충전 시간

        [Header("References")]
        public float metalSphereVelocity;    // 기본 발사 힘(계수)

        [Header("Targets")]
        public Transform leftTarget;  // 왼쪽 타겟
        public Transform rightTarget; // 오른쪽 타겟

        [Header("Movement Settings")]
        // public float pullSpeed = 0.2f;   // 뒤로 당기는 속도
        public float rotationSpeed = 100.0f; // 마우스 좌우 회전 속도

        [Header("Ball Level (0~4)")]
        // 현재 사용하는 볼의 "레벨(타입)" (0 ~ 4, 총 5단계)
        public int currentSphereLevel = 0;

        public int selectingPointer { get; private set; } = -1;

        // 내부 상태 변수
        private Rigidbody rb;
        private GameObject metalSphere;
        private float pulled = -4f;    // 당길 수 있는 최대(-z)
        private int i = 1;            // 공을 한 번만 가져오기 위한 플래그
        private float z = -2f;        // 당기는 정도
        private float chargeTime = 0f; // 게이지 충전
        private float startAngle;      // 시작 시점의 Y회전값(중앙 기준)
        bool isSelected = false;

        public bool isTouch = true;

        void Start()
        {
            // BallManager 오브젝트의 초기 Y회전값 저장 → 왼/오른쪽 판별에 사용
            startAngle = transform.eulerAngles.y;
        }

        public void SetSelected(int pointer)
        {
            selectingPointer = pointer;
            isSelected = (pointer >= 0);

            // 잡혔으면 kinematic
            if (rb) rb.isKinematic = isSelected;

            if (isSelected)
            {
                // 초기화
                chargeTime = 0f;
                powerGauge.value = 0f;
                z = -2f;
            }
        }

        /// <summary>
        /// Vision OS에서 계산한 deltaY(이번 프레임 회전량) 파라미터로 받음
        /// </summary>
        public void ChargeGauge(float deltaY)
        {
            if (!isTouch) return;
            if (!isSelected) return;

            // 1) 공을 한 번만 가져오기
            if (i == 1)
            {
                // ★ 오브젝트 풀에서 현재 레벨의 볼 가져오기
                metalSphere = BallPool.Instance.GetPooledBall(currentSphereLevel);

                // 위치/회전 초기화
                metalSphere.transform.position = new Vector3(0, 1.2f, 0);
                metalSphere.transform.rotation = Quaternion.identity;

                // BallManager의 자식으로 설정
                metalSphere.transform.parent = this.transform;

                // Rigidbody 캐싱 & 상태 초기화
                rb = metalSphere.GetComponent<Rigidbody>();
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;

                i = 0;
            }

            // // 2) 마우스 좌우 이동에 따른 Y축 회전
            float rotationAmount = deltaY * rotationSpeed;
            transform.Rotate(0, rotationAmount, 0);

            // 3) 뒤로 당기는 동작
            z -= 0.1f;
            float updatedZ = (z >= pulled) ? (z + 1.7f) : (pulled + 1.7f);
            metalSphere.transform.localPosition = new Vector3(0f, 1.2f, updatedZ);

            // 4) 게이지 충전
            chargeTime += Time.deltaTime;
            powerGauge.value = Mathf.Clamp01(chargeTime / maxChargeTime);
        }

        /// <summary>
        /// 놓는 순간 발사
        /// </summary>
        public void Shoot()
        {
            if (!isSelected) return;

            float gaugeValue = powerGauge.value;

            // 게이지/상태 초기화
            chargeTime = 0f;
            powerGauge.value = 0f;
            i = 1;

            // 부모 해제, 중력 켜기
            metalSphere.transform.parent = null;
            rb.useGravity = true;

            // 1) 왼쪽/오른쪽 타겟 판별
            float angleDiff = Mathf.DeltaAngle(startAngle, transform.eulerAngles.y);
            Transform chosenTarget = (angleDiff < 0f) ? leftTarget : rightTarget;

            // 2) 타겟까지의 방향 벡터
            Vector3 direction = (chosenTarget.position - metalSphere.transform.position).normalized;

            // 3) 기본 힘 계산 (z를 얼마나 당겼나에 따라)
            float baseForce = (z >= pulled) ? (metalSphereVelocity * -z) : (metalSphereVelocity * 15f);

            // 4) 게이지 70% 이상 vs 70% 미만
            if (gaugeValue >= 0.7f)
            {
                // -------------------------
                // (A) 70% 이상: 정확히 타겟에 꽂히도록 (직선 발사)
                // -------------------------
                rb.useGravity = false; // 중력 비활성 → 곧장 직선 이동
                float speed = 20f;     // 원하는 속도

                // velocity 직접 설정 (직선으로 타겟 중심에 도달)ßß
                rb.linearVelocity = direction * speed;
            }
            else

            {
                // -------------------------
                // (B) 70% 미만: 약한 힘 + 중력 적용 → 도중에 떨어짐
                // -------------------------
                rb.useGravity = true;
                float weakMultiplier = 0.15f;  // 힘 감소 비율
                float finalForce = baseForce * weakMultiplier;

                rb.AddForce(direction * finalForce, ForceMode.Impulse);
            }

            // 5) z값 초기화 (다음 발사 대비)
            z = -2f;

            // 선택 해제
            SetSelected(-1);
        }


        /// <summary>
        /// 특정 조건(점수 등)으로 호출하여 볼 레벨 업그레이드
        /// </summary>
        public void UpgradeSphere(int score)
        {
            currentSphereLevel = score;
            // currentSphereLevel++;
            if (currentSphereLevel >= 5) // 0 ~ 4까지만
            {
                currentSphereLevel = 4;
            }
            Debug.Log("Sphere Upgraded! Current Level = " + currentSphereLevel);
        }
    }
}
