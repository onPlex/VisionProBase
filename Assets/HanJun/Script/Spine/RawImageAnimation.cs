using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Jun
{
    [System.Serializable]
    public class AnimationClipData
    {
        public string animationName; // 애니메이션 이름
        public Texture2D[] frames; // 해당 애니메이션의 프레임
    }

    public class RawImageAnimation : MonoBehaviour
    {
        public RawImage rawImage; // UI에서 사용할 RawImage
        public int framesPerSecond = 60; // FPS 설정 (예: 60, 30, 24 등)

        private string idleAnimationName = "idle"; // 기본적으로 실행될 Idle 애니메이션
        public List<AnimationClipData> animationClips = new List<AnimationClipData>(); // Inspector에서 관리

        private Dictionary<string, Texture2D[]> animations = new Dictionary<string, Texture2D[]>(); // 내부 관리용
        private string currentAnimation = null;
        private int currentFrame = 0;
        private bool isPlaying = false;
        private Coroutine coroutine = null;

        void Start()
        {
            // Inspector에서 추가한 애니메이션을 Dictionary에 저장
            foreach (var clip in animationClips)
            {
                if (!animations.ContainsKey(clip.animationName))
                {
                    animations.Add(clip.animationName, clip.frames);
                }
            }

            // Idle 애니메이션이 존재하는지 확인하고 실행
            if (animations.ContainsKey(idleAnimationName))
            {
                PlayAnimation(idleAnimationName); // 시작할 때 항상 Idle 실행
            }
        }

        private void OnEnable() {
            
             // Idle 애니메이션이 존재하는지 확인하고 실행
            if (animations.ContainsKey(idleAnimationName))
            {
                PlayAnimation(idleAnimationName); // 시작할 때 항상 Idle 실행
            }
        }

        private void OnDisable() {
            StopAnimation();
        }

        /// <summary>
        /// 특정 애니메이션 시작 (마지막 프레임에서 Idle로 돌아감)
        /// </summary>
        protected void PlayAnimation(string name)
        {
            if(!gameObject.activeInHierarchy) return;
            if (!animations.ContainsKey(name)) return;

            currentAnimation = name;
            currentFrame = 0;
            isPlaying = true;

            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
            coroutine = StartCoroutine(PlayAnimationCoroutine());
        }

        private IEnumerator PlayAnimationCoroutine()
        {
            float frameTime = 1f / framesPerSecond; // FPS를 시간 단위로 변환

            while (isPlaying && animations.ContainsKey(currentAnimation))
            {
                Texture2D[] frames = animations[currentAnimation];
                if (frames.Length == 0) yield break;

                rawImage.texture = frames[currentFrame]; // 현재 프레임 적용
                rawImage.SetNativeSize();

                if (currentFrame >= frames.Length - 1) // 마지막 프레임이면 Idle 실행
                {
                    PlayAnimation(currentAnimation);
                    yield break;
                }

                currentFrame++; // 다음 프레임으로 이동
                yield return new WaitForSeconds(frameTime); // FPS에 따른 시간 대기
            }
        }

        /// <summary>
        /// 애니메이션 정지 (즉시 Idle 애니메이션 실행)
        /// </summary>
        public void StopAnimation()
        {
            isPlaying = false;
            if (coroutine != null) StopCoroutine(coroutine);
            PlayAnimation(idleAnimationName);
        }
    }
}