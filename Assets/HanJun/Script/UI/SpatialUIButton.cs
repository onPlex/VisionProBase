using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Jun
{
    public class SpatialUIButton : MonoBehaviour
    {
        [SerializeField] AudioSource m_AudioSource;

        Coroutine m_DelayCoroutine;
        bool m_Delay;

        public virtual void Press()
        {
            if (m_AudioSource != null)
                PlayAudioPinchStart();
        }

        void PlayAudioPinchStart()
        {
            m_AudioSource.volume = 0.25f;
            m_AudioSource.Play();

            if (m_DelayCoroutine != null)
                StopCoroutine(m_DelayCoroutine);

            m_DelayCoroutine = StartCoroutine(Delay(0.5f));
        }

        IEnumerator Delay(float time)
        {
            m_Delay = true;
            yield return new WaitForSeconds(time);
            m_Delay = false;
        }
    }
}