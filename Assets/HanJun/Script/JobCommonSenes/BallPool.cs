using UnityEngine;
using System.Collections.Generic;

namespace Jun
{
    // ★씬에 빈 오브젝트 "BallPoolManager"를 만들고 붙이기 (싱글톤 형태)
    public class BallPool : MonoBehaviour
    {
        public static BallPool Instance { get; private set; }

        [Header("Pool Prefabs (5 Levels)")]
        // 5가지 타입의 볼 Prefab (index 0~4)
        public GameObject[] spherePrefabs;

        [Header("Pool Size for Each Type")]
        // 각 타입별 초기 생성 개수 (예: [10,10,10,10,10])
        public int[] poolSizes;

        // 각 타입별 큐
        private Queue<GameObject>[] poolQueues;

        private void Awake()
        {
            // 싱글톤
            if (Instance == null) Instance = this;
            else
            {
                Destroy(gameObject);
                return;
            }

            // 풀 초기화
            InitializePools();
        }

        private void InitializePools()
        {
            int length = spherePrefabs.Length;
            poolQueues = new Queue<GameObject>[length];

            for (int i = 0; i < length; i++)
            {
                poolQueues[i] = new Queue<GameObject>();

                for (int j = 0; j < poolSizes[i]; j++)
                {
                    GameObject obj = Instantiate(spherePrefabs[i]);
                    obj.SetActive(false);

                    // 볼 타입 정보 부착
                    BallType ballType = obj.GetComponent<BallType>();
                    if (ballType == null)
                    {
                        ballType = obj.AddComponent<BallType>();
                    }
                    ballType.typeIndex = i;

                    poolQueues[i].Enqueue(obj);
                }
            }
        }

        /// <summary>
        /// 지정된 타입의 볼을 하나 꺼내온다
        /// </summary>
        public GameObject GetPooledBall(int typeIndex)
        {
            // 큐에 남아 있는 볼이 있으면 재활용
            if (poolQueues[typeIndex].Count > 0)
            {
                GameObject obj = poolQueues[typeIndex].Dequeue();
                obj.SetActive(true);
                return obj;
            }
            else
            {
                // 없다면 새로 Instantiate
                GameObject newObj = Instantiate(spherePrefabs[typeIndex]);
                newObj.SetActive(true);
                BallType ballType = newObj.GetComponent<BallType>();
                if (ballType == null)
                    ballType = newObj.AddComponent<BallType>();

                ballType.typeIndex = typeIndex;
                return newObj;
            }
        }

        /// <summary>
        /// 사용이 끝난 볼을 풀로 돌려놓기
        /// </summary>
        public void ReturnBall(GameObject ball)
        {
            ball.SetActive(false);
            // 타입 인덱스 확인
            BallType ballType = ball.GetComponent<BallType>();
            int typeIndex = ballType.typeIndex;

            poolQueues[typeIndex].Enqueue(ball);
        }
    }
}