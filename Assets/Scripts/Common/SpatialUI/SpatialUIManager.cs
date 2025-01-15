using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using Unity.PolySpatial.InputDevices;

namespace Novaflo.Common
{
    public class SpatialUIManager : MonoBehaviour
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

                if (activeTouches[0].phase == TouchPhase.Began &&
                primaryTouchData.targetObject != null &&
                primaryTouchData.targetObject.scene == gameObject.scene)
                {
                    if (primaryTouchData.targetObject.TryGetComponent(out SpatialUIButton button))
                    {
                        button.OnPress();
                    }
                }
            }
        }
    }
}