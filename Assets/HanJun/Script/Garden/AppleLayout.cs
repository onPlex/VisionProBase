using UnityEngine;

namespace Jun
{
    public class AppleLayout : MonoBehaviour
    {
        [Header("Circle Settings")]
        public GameObject prefab; // 생성할 프리팹
        public int numberOfObjects = 6; // 배치할 오브젝트 개수
        public float radius = 3f; // 원의 반지름
        public Vector3 centerOffset; // 중심 위치 조정

        void Start()
        {
            ArrangeObjectsInCircle();
        }

        void ArrangeObjectsInCircle()
        {
            // 중심 위치
            Vector3 center = transform.position + centerOffset;

            // 오브젝트를 원형으로 배치
            for (int i = 0; i < numberOfObjects; i++)
            {
                // 각도를 계산
                float angle = i * Mathf.PI * 2 / numberOfObjects;

                // x, z 위치 계산 (원의 방정식)
                float x = Mathf.Cos(angle) * radius;
                float z = Mathf.Sin(angle) * radius;

                // 새 위치
                Vector3 position = new Vector3(x, 0, z) + center;

                // 오브젝트 생성
                GameObject obj = Instantiate(prefab, position, Quaternion.identity, transform);

                // 오브젝트의 방향을 중심으로 향하게 설정
                obj.transform.LookAt(center);
            }
        }
    }
}
