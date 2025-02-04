using UnityEngine;

namespace Jun
{
    public class LeftTarget : MonoBehaviour
    {
        [SerializeField] private OXQuizManager oXQuizManager;
        [SerializeField] private ParticleSystem hit;

        private void OnCollisionEnter(Collision collision)
        {
            // 공이 부딪혔다면 X 선택 처리
            if (oXQuizManager != null)
            {
                hit.Play();
                oXQuizManager.OnSelectO();
            }
        }
    }
}
