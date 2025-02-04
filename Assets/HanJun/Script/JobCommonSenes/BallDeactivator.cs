using UnityEngine;

namespace Jun
{
    public class BallDeactivator : MonoBehaviour
    {
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Basket"))
            {
                // 충돌 시 풀로 반환
                BallPool.Instance.ReturnBall(gameObject);
            }
        }
    }
}