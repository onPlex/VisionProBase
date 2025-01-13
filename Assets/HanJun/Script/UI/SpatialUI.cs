using UnityEngine;
using UnityEngine.Events;

namespace Jun
{
    public class SpatialUI : SpatialUIButton
    {
        // UnityEvent를 선언하여 Inspector에서 이벤트를 등록 가능
        [SerializeField] private UnityEvent onPress;

        public override void Press()
        {
            base.Press();
            Debug.Log("SpatialButtonListener Pressed");
            // UnityEvent 호출
            onPress?.Invoke();
        }

        // 외부에서 이벤트를 동적으로 등록할 수 있는 메서드
        public void AddListener(UnityAction action)
        {
            onPress.AddListener(action);
        }

        public void RemoveListener(UnityAction action)
        {
            onPress.RemoveListener(action);
        }
    }
}