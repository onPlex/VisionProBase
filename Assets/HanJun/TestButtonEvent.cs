using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TestButtonEvent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] RectTransform rectTransform;
    private Vector3 originalScale;
    public Vector3 hoverScale = new Vector3(1.2f, 1.2f, 1f); // 호버 시 버튼 크기 (기본 크기보다 20% 커짐)

    private void Start()
    {
        // 원래 버튼 크기를 저장해둡니다.
        originalScale = rectTransform.localScale;
    }

    // 마우스가 버튼에 올라갔을 때 호출되는 함수
    public void OnPointerEnter(PointerEventData eventData)
    {
        // 버튼의 크기를 호버 크기로 변경
        rectTransform.localScale = hoverScale;
        Debug.Log("Mouse Entered Button - Scale Increased");
    }

    // 마우스가 버튼을 떠났을 때 호출되는 함수
    public void OnPointerExit(PointerEventData eventData)
    {
        // 버튼의 크기를 원래 크기로 되돌림
        rectTransform.localScale = originalScale;
        Debug.Log("Mouse Exited Button - Scale Restored");
    }
}
