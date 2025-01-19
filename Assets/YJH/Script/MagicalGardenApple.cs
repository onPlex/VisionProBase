using UnityEngine;
using DG.Tweening; // DOTween 네임스페이스

public class MagicalGardenApple : MonoBehaviour
{
    [Header("Floating Settings")]
    [SerializeField] private float floatDistance = 0.5f;  // 위아래 움직일 거리
    [SerializeField] private float floatDuration = 1.5f;  // 한 번의 '올라가기' 혹은 '내려가기'에 걸리는 시간

    private Vector3 originalPosition;

    private void Start()
    {
        // 시작 시점의 위치 기억
        originalPosition = transform.position;

        // 둥실둥실 떠오르는 애니메이션 시작
        StartFloating();
    }

    private void StartFloating()
    {
        // 위로 floatDistance만큼 이동했다가
        // 다시 아래로 원복하는 요요(Yoyo) 형태의 무한 반복 애니메이션
        transform.DOMoveY(originalPosition.y + floatDistance, floatDuration)
                 .SetEase(Ease.InOutSine)      // 부드럽게 오르내리도록 이징 설정
                 .SetLoops(-1, LoopType.Yoyo); // 무한 반복(-1), 요요(갔다가 돌아옴)
    }
}
