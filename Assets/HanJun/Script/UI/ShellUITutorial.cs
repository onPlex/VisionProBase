using UnityEngine;
using UnityEngine.Events;

namespace Jun
{
    public class ShellUITutorial : SpatialUIButton
    {
        [SerializeField] private UnityEvent onPress;
        [SerializeField] private GameObject _shellDescObj;
        [SerializeField] private ShellBehavior _shellBehavior;

        public override void Press()
        {
            base.Press();
            onPress?.Invoke();

            _shellBehavior.SetAnimationEvent(true, DelayStart);
        }

        private void DelayStart()
        {
            _shellDescObj.SetActive(true);
            _shellBehavior.EnabledCollider(true);
        }
    }
}