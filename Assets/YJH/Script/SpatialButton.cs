using Unity.PolySpatial.InputDevices;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

namespace YJH
{
    public class SpatialButton : MonoBehaviour
    {
        void OnEnable()
        {
            EnhancedTouchSupport.Enable();
        }

        void Update()
        {
            var activeTouches = Touch.activeTouches;

            if (activeTouches.Count > 0)
            {
                var primaryTouchData = EnhancedSpatialPointerSupport.GetPointerState(activeTouches[0]);
                if (activeTouches[0].phase == TouchPhase.Began && primaryTouchData.targetObject != null && primaryTouchData.targetObject.scene == gameObject.scene)
                {
                    if (primaryTouchData.targetObject.TryGetComponent(out SpatialButtonEvent button))
                    {
                        button.Press();
                    }
                }
            }
        }
    }
}