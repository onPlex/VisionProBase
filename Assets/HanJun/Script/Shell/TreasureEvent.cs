using UnityEngine;
using UnityEngine.Events;

namespace Jun
{
    public class TreasureEvent : SpatialUIButton
    {
        [SerializeField] private UnityEvent onPress;

        public override void Press()
        {
            base.Press();
            onPress?.Invoke();
        }
    }
}