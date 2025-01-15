using TMPro;
using UnityEngine;

public class ButtonState : MonoBehaviour
{
    public GameObject TargetButton; // 상태를 관리할 버튼 오브젝트
    public Material ActiveMaterial; // Active 상태의 Material
    public Material InactiveMaterial; // Inactive 상태의 Material
    public TMP_Text ButtonText; // 버튼의 텍스트
    public Color ActiveTextColor = Color.white; // Active 상태의 텍스트 색상
    public Color InactiveTextColor = Color.gray; // Inactive 상태의 텍스트 색상

    /// <summary>
    /// 버튼 상태를 업데이트합니다.
    /// </summary>
    /// <param name="isActive">버튼 상태 (true: Active, false: Inactive)</param>
    public void UpdateButtonState(bool isActive)
    {
        // Material 변경
        var renderer = TargetButton.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = isActive ? ActiveMaterial : InactiveMaterial;
        }

        // TMP_Text 색상 변경
        if (ButtonText != null)
        {
            ButtonText.color = isActive ? ActiveTextColor : InactiveTextColor;
        }
    }
}
