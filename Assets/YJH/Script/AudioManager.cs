using UnityEngine;
using System.Collections.Generic;

namespace YJH
{
    public class AudioManager : MonoBehaviour
    {
        // Singleton instance
        public static AudioManager Instance { get; private set; }

        [SerializeField]
        // Dictionary to store audio clips
        private Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();

        // AudioSource for playing sound effects
        private AudioSource sfxSource;

        // AudioSource for background music
        private AudioSource bgmSource;

        void Awake()
        {
            // Enforce singleton pattern
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            //DontDestroyOnLoad(gameObject);

            // Initialize audio sources
            sfxSource = gameObject.AddComponent<AudioSource>();
            bgmSource = gameObject.AddComponent<AudioSource>();
            bgmSource.loop = true; // Background music loops by default
        }

        /// <summary>
        /// Loads an audio clip into the manager.
        /// </summary>
        /// <param name="clipName">The name used to identify the audio clip.</param>
        /// <param name="clip">The AudioClip to load.</param>
        public void LoadAudioClip(string clipName, AudioClip clip)
        {
            if (!audioClips.ContainsKey(clipName))
            {
                audioClips.Add(clipName, clip);
            }
        }

        /// <summary>
        /// Plays a sound effect.
        /// </summary>
        /// <param name="clipName">The name of the audio clip to play.</param>
        public void PlaySFX(string clipName)
        {
            if (audioClips.TryGetValue(clipName, out AudioClip clip))
            {
                sfxSource.PlayOneShot(clip);
            }
            else
            {
                Debug.LogWarning($"Audio clip '{clipName}' not found.");
            }
        }

        /// <summary>
        /// Plays background music.
        /// </summary>
        /// <param name="clipName">The name of the audio clip to play.</param>
        public void PlayBGM(string clipName)
        {
            if (audioClips.TryGetValue(clipName, out AudioClip clip))
            {
                if (bgmSource.clip != clip)
                {
                    bgmSource.clip = clip;
                    bgmSource.Play();
                }
            }
            else
            {
                Debug.LogWarning($"Audio clip '{clipName}' not found.");
            }
        }

        /// <summary>
        /// Stops the background music.
        /// </summary>
        public void StopBGM()
        {
            bgmSource.Stop();
        }

        /// <summary>
        /// Sets the volume for sound effects.
        /// </summary>
        /// <param name="volume">Volume value (0.0 to 1.0).</param>
        public void SetSFXVolume(float volume)
        {
            sfxSource.volume = Mathf.Clamp01(volume);
        }

        /// <summary>
        /// Sets the volume for background music.
        /// </summary>
        /// <param name="volume">Volume value (0.0 to 1.0).</param>
        public void SetBGMVolume(float volume)
        {
            bgmSource.volume = Mathf.Clamp01(volume);
        }

        /// <summary>
        /// Stops all audio and clears loaded clips. Useful when changing scenes.
        /// </summary>
        public void ClearAudio()
        {
            sfxSource.Stop();
            bgmSource.Stop();
            audioClips.Clear();
        }
    }
}