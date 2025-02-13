using UnityEngine;
using DG.Tweening; // DOTween 네임스페이스 추가

public class ResultShowEvent : MonoBehaviour
{
    [SerializeField] private GameObject ShowEventObj;
    [SerializeField] private GameObject ResultBoardObj;
    [SerializeField] private Animator JewelBoxAnim; // 보석 상자 애니메이터
    [SerializeField] private GameObject KeyObj;
    [SerializeField] private Transform TargetPoint;

    [SerializeField] private float moveDuration = 1.5f; // 이동 시간
    [SerializeField] private float fadeDuration = 1f; // 페이드 효과 시간
    [SerializeField] private float rotateDuration = 0.8f; // 회전 시간
    [SerializeField] private float delayAfterOpen = 2f; // 보석 상자 오픈 후 대기 시간

    public void OnClickKeyShow()
    {
        if (KeyObj == null || TargetPoint == null)
        {
            Debug.LogError("KeyObj 또는 TargetPoint가 설정되지 않았습니다.");
            return;
        }

        // KeyObj가 비활성화되어 있다면 활성화
        KeyObj.SetActive(true);

        // 키 오브젝트 이동 및 페이드 인 애니메이션 실행
        KeyObj.transform.DOMove(TargetPoint.position, moveDuration)
            .SetEase(Ease.OutBack) // 부드러운 가속 감속 이동
            .OnComplete(() =>
            {
                // 이동 완료 후 현재 회전값에서 Z축 90도 회전
                KeyObj.transform.DOLocalRotate(Vector3.forward * 90, rotateDuration, RotateMode.LocalAxisAdd)
                    .SetEase(Ease.OutBack)
                    .OnComplete(() =>
                    {
                        // JewelBoxAnim의 "Open" 트리거 발동
                        if (JewelBoxAnim != null)
                        {
                            JewelBoxAnim.SetTrigger("Open");
                        }

                        // 2초 후 ShowEventObj 비활성화, ResultBoardObj 활성화
                        DOVirtual.DelayedCall(delayAfterOpen, () =>
                        {
                            if (ShowEventObj != null) ShowEventObj.SetActive(false);
                            if (ResultBoardObj != null) ResultBoardObj.SetActive(true);
                        });
                    });
            });
    }
}
