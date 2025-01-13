using Unity.PolySpatial.InputDevices;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

namespace Jun
{
    public class ShellInputManager : MonoBehaviour
    {
        // TouchPhase m_LastTouchPhase;
        ShellBehavior m_SelectedObject;

        bool _toggleValue = false;

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
                    if (primaryTouchData.targetObject.TryGetComponent(out ShellBehavior shellObject))
                    {
                        Debug.Log("touch");
                        m_SelectedObject = shellObject;
                        if (!_toggleValue)
                        {
                            _toggleValue = true;
                            m_SelectedObject.Select(true);
                            EnableCollider(false);
                        }
                        else
                        {
                            m_SelectedObject.Select(false);
                        }
                    }
                }
            }
        }

        public void EnableCollider(bool enabled)
        {
            if (m_SelectedObject != null)
                m_SelectedObject.GetComponent<BoxCollider>().enabled = enabled;
        }
    }
}
