using Unity.PolySpatial.InputDevices;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.InputSystem.LowLevel;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

namespace Jun
{
    public class ShellInputManager : MonoBehaviour
    {
        // TouchPhase m_LastTouchPhase;
        ShellBehavior m_SelectedObject;

        // bool _toggleValue = false;

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
                if (activeTouches[0].phase == TouchPhase.Began)
                {
                    if (primaryTouchData.Kind == SpatialPointerKind.IndirectPinch || primaryTouchData.Kind == SpatialPointerKind.Touch)
                    {
                        if (primaryTouchData.targetObject != null)
                        {
                            if (primaryTouchData.targetObject.TryGetComponent(out ShellBehavior shellObject))
                            {
                                Debug.Log("Touch");
                                m_SelectedObject = shellObject;
                                // _toggleValue = !_toggleValue;
                                m_SelectedObject.Select(true);
                            }
                        }
                    }
                }
            }
        }
    }
}
