using UnityEngine;
using UnityEngine.Events;

namespace Jun
{
    public class ShellUI : SpatialUIButton
    {
        [SerializeField] private UnityEvent onPress;
        [SerializeField] private GameObject _shellDescObj;

        // bool _isToggleEnabled = false;

        public override void Press()
        {
            base.Press();
            // _isToggleEnabled = !_isToggleEnabled;
            _shellDescObj.SetActive(true);
            onPress?.Invoke();
        }
    }
}