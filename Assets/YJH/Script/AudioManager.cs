using UnityEngine;
using System.Collections.Generic;

namespace YJH
{
    /// <summary>
    /// Inspector에 표시할 AudioClip 정보용 구조체
    /// </summary>
    [System.Serializable]
    public struct AudioClipInfo
    {
        public string clipName;      // Inspector에서 편집할 키 이름
        public AudioClip audioClip;  // Inspector에서 등록할 실제 사운드 파일
    }

    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [Header("Inspector에서 등록할 오디오 리스트")]
        [SerializeField]
        private List<AudioClipInfo> audioClipInfos = new List<AudioClipInfo>();

        // 런타임 접근용 Dictionary
        private Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();

        // BGM, SFX용 AudioSource
        private AudioSource sfxSource;
        private AudioSource bgmSource;

        void Awake()
        {
            // Singleton 패턴
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            //DontDestroyOnLoad(gameObject);

            // AudioSource 초기화
            sfxSource = gameObject.AddComponent<AudioSource>();
            bgmSource = gameObject.AddComponent<AudioSource>();
            bgmSource.loop = true;

            // List -> Dictionary 로드
            foreach (var info in audioClipInfos)
            {
                if (!string.IsNullOrEmpty(info.clipName) && info.audioClip != null)
                {
                    if (!audioClips.ContainsKey(info.clipName))
                    {
                        audioClips.Add(info.clipName, info.audioClip);
                    }
                    else
                    {
                        Debug.LogWarning($"중복된 clipName이 있습니다: {info.clipName}");
                    }
                }
            }
        }

        /// <summary>
        /// SFX 재생
        /// </summary>
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
        /// BGM 재생
        /// </summary>
        public void PlayBGM(string clipName)
        {
            if (audioClips.TryGetValue(clipName, out AudioClip clip))
            {
                // 이미 다른 BGM이 재생 중이거나, 재생 요청된 clip이 다를 경우에만 교체
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
        /// BGM 정지
        /// </summary>
        public void StopBGM()
        {
            bgmSource.Stop();
        }

        /// <summary>
        /// SFX 볼륨 설정
        /// </summary>
        public void SetSFXVolume(float volume)
        {
            sfxSource.volume = Mathf.Clamp01(volume);
        }

        /// <summary>
        /// BGM 볼륨 설정
        /// </summary>
        public void SetBGMVolume(float volume)
        {
            bgmSource.volume = Mathf.Clamp01(volume);
        }

        /// <summary>
        /// 모든 오디오 정지 및 딕셔너리 초기화
        /// </summary>
        public void ClearAudio()
        {
            sfxSource.Stop();
            bgmSource.Stop();
            audioClips.Clear();
        }
    }
}
