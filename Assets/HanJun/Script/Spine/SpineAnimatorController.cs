using UnityEngine;
using System.Collections;
using Spine.Unity;
using Spine;

namespace Jun
{
    public class SpineAnimatorController : MonoBehaviour
    {
        [SpineAnimation("idle")]
        public string idleAnimation;

        [Tooltip("idle애니메이션을 제외한 나머지 애니매이션 추가시, 순서대로 숫자키 1번부터 자동으로 매핑되어 숫자키 누를시 해당 애니매이션 재생")]
        [SpineAnimation] public string[] animations;
        private KeyCode[] keyCodes;

        private string currentAnimation;

        SkeletonAnimation skeletonAnimation;

        void Awake()
        {
            skeletonAnimation = GetComponent<SkeletonAnimation>();

        }

        private void Start()
        {
            // KeyCode 배열 초기화 및 숫자 키 매핑
            InitializeKeyCodes();

            // 기본 애니메이션 재생
            if (!string.IsNullOrEmpty(idleAnimation))
            {
                PlayAnimation(idleAnimation, true);
            }
        }


        void Update()
        {
            for (int i = 0; i < keyCodes.Length; i++)
            {
                if (Input.GetKeyDown(keyCodes[i]))
                {
                    PlayAnimation(animations[i], false);
                }

            }
        }

        /// <summary>
        /// Spine 애니메이션 재생 메서드
        /// </summary>
        /// <param name="animationName">재생할 애니메이션 이름</param>
        /// <param name="loop">애니메이션 반복 여부</param>
        public void PlayAnimation(string animationName, bool loop)
        {
            if (currentAnimation == animationName) return; // 이미 재생 중이면 실행하지 않음

            var trackEntry = skeletonAnimation.state.SetAnimation(0, animationName, loop);
            currentAnimation = animationName;

            // Complete 이벤트 등록
            trackEntry.Complete += OnAnimationComplete;
        }

        /// <summary>
        /// 애니메이션 재생 완료 이벤트 핸들러
        /// </summary>
        /// <param name="trackEntry">완료된 트랙</param>
        private void OnAnimationComplete(TrackEntry trackEntry)
        {
            // 현재 애니메이션이 loop가 아니고, idle이 아닐 경우 idle로 전환
            if (!trackEntry.Loop && currentAnimation != idleAnimation)
            {
                PlayAnimation(idleAnimation, true);
            }
        }

        /// <summary>
        /// KeyCode 배열 초기화 (1부터 숫자 키 할당)
        /// </summary>
        private void InitializeKeyCodes()
        {
            keyCodes = new KeyCode[animations.Length];
            for (int i = 0; i < animations.Length; i++)
            {
                // 숫자 키 1~9에 대응하는 KeyCode를 순서대로 할당
                keyCodes[i] = KeyCode.Alpha1 + i;

                // 디버그용 출력
                // Debug.Log($"KeyCode[{i}] = {keyCodes[i]} (Animation: {animations[i]})");
            }
        }


    }
}