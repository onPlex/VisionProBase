using UnityEngine;
using UnityEngine.Events;


namespace Novaflo.Common
{
     public class SpatialUIButton : MonoBehaviour
     {
          public UnityEvent unityEvent;
          public void OnPress()
          {
               unityEvent?.Invoke();
          }
     }
}
