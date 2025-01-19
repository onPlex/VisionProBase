using PolySpatial.Samples;
using UnityEngine;
using UnityEngine.Events;

namespace Jun
{
    public class MakeButton : HubButton
    {
        [SerializeField] private UnityEvent PressEvent;

        public override void Press()
        {
            base.Press();
            PressEvent.Invoke();
        }
    }
}