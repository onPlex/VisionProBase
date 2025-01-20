using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Jun
{
    public class SoundManager : Singleton<SoundManager>
    {
        [SerializeField] private AudioSource _NarrationSource;
        [SerializeField] private AudioSource _effectSource;

        [SerializeField] private List<AudioClip> _listAudioClipts;

        private Dictionary<string, AudioClip> _dicAudioClips = new Dictionary<string, AudioClip>();

        private Coroutine audioCoroutine = null;

        private void Awake()
        {
            _listAudioClipts.ForEach((clips) => _dicAudioClips.Add(clips.name, clips));
        }

        private void Start()
        {
            PlayNarration("01_Title");
        }

        public void PlayEffect(string name = "")
        {
            if (name == "" && _effectSource.isPlaying) _effectSource.Stop();

            if (_dicAudioClips.ContainsKey(name))
            {
                _effectSource.PlayOneShot(_dicAudioClips[name]);
            }
        }

        public void PlayNarration(string name = "", UnityAction OnComplete = null, float delayTime = 0.5f)
        {
            if (_NarrationSource.isPlaying || name.Equals("")) _NarrationSource.Stop();

            if (!name.Equals("") || !name.Equals("null"))
            {
                if (_dicAudioClips.ContainsKey(name))
                {
                    _NarrationSource.clip = _dicAudioClips[name];
                    _NarrationSource.Play();

                    if (audioCoroutine != null) StopCoroutine(audioCoroutine);
                    audioCoroutine = StartCoroutine(AudioLoop(OnComplete, delayTime));
                }
            }
        }

        public void PlayNarration(string name = "")
        {
            if (_NarrationSource.isPlaying || name.Equals("")) _NarrationSource.Stop();

            if (!name.Equals("") || !name.Equals("null"))
            {
                if (_dicAudioClips.ContainsKey(name))
                {
                    _NarrationSource.clip = _dicAudioClips[name];
                    _NarrationSource.Play();
                }
            }
        }

        IEnumerator AudioLoop(UnityAction OnComplete = null, float delayTime = 0.5f)
        {
            int length = (int)Mathf.Round(_NarrationSource.clip.length * 10.0f) / 10;
            while (OnComplete != null)
            {
                yield return null;
                int currentTime = (int)Mathf.Round(_NarrationSource.time * 10.0f) / 10;

                if (currentTime == length)
                {
                    yield return new WaitForSeconds(delayTime);
                    OnComplete?.Invoke();
                    break;
                }
            }
        }
    }
}