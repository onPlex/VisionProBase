using UnityEngine;
using UnityEngine.UI;

public class TestButtonEvent : MonoBehaviour
{
    [SerializeField] RectTransform rectTransform;
    private Vector3 originalScale;
    public Vector3 hoverScale = new Vector3(1.2f, 1.2f, 1f); // 호버 시 버튼 크기 (기본 크기보다 20% 커짐)

    private void Start()
    {
        originalScale = rectTransform.localScale;
    }

    public void OnOver()
    {
        rectTransform.localScale = hoverScale;
        Debug.Log("Mouse Entered Button - Scale Increased");
    }

    public void OnOut()
    {
        rectTransform.localScale = originalScale;
        Debug.Log("Mouse Exited Button - Scale Restored");
    }
}
