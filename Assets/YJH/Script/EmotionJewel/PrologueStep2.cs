using UnityEngine;
using DG.Tweening; // DOTween 네임스페이스 추가


namespace YJH.EmotionJewel
{
    public class PrologueStep2 : MonoBehaviour
    {
        [SerializeField] private GameObject FairyObj; // 이동할 요정
        [SerializeField] private Transform[] MovePoint; // 최소 3개 이상의 포인트
        [SerializeField] private GameObject DialogueObj; // 도착 후 활성화할 오브젝트
        [SerializeField] private float moveDuration = 3f; // 이동 시간
        [SerializeField] private Ease moveEase = Ease.InOutSine; // 이동 방식

        void OnEnable()
        {
            if (FairyObj == null || MovePoint.Length < 3)
            {
                Debug.LogError("FairyObj가 없거나 MovePoint가 3개 이상 지정되지 않았습니다.");
                return;
            }

            MoveFairyAlongPath();
        }

        private void MoveFairyAlongPath()
        {
            // 이동 경로 생성
            Vector3[] pathPoints = new Vector3[MovePoint.Length];
            for (int i = 0; i < MovePoint.Length; i++)
            {
                pathPoints[i] = MovePoint[i].position;
            }

            // 요정 이동 애니메이션
            FairyObj.transform.DOPath(pathPoints, moveDuration, PathType.CatmullRom)
                .SetEase(moveEase) // 부드러운 이동
                .OnComplete(() =>
                {
                    // 도착 후 대화창 활성화
                    if (DialogueObj != null)
                    {
                        DialogueObj.SetActive(true);
                    }
                });
        }
    }
}
