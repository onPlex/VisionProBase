using UnityEngine;
using Jun;
using UnityEngine.Events;

public class ShellTitleButton : SpatialUIButton
{
    public UnityEvent OnPress;

    [SerializeField] private GameObject desc;


    [SerializeField] ShellBehavior shellBehavior;
    public override void Press()
    {
        base.Press();
        OnPress?.Invoke();
    }

    // 외부에서 이벤트를 동적으로 등록할 수 있는 메서드
    public void AddListener(UnityAction action)
    {
        OnPress.AddListener(action);
    }

    public void RemoveListener(UnityAction action)
    {
        OnPress.RemoveListener(action);
    }


    //  public void SelectShellEvent()
    // {
    //     shellBehavior.SetAnimationEvent(true, ShellOpened);
    // }

    // private void ShellOpened()
    // {
    //     _boxCollider.enabled = false;
    //      title.SetActive(true);
    // }
}
