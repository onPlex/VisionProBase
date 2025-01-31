using UnityEngine;

namespace Jun
{
    public class WomanAnimation : RawImageAnimation
    {
        public void PlayHelloAnimation() => PlayAnimation("hello");
        public void PlaySadAnimation() => PlayAnimation("sad");
        public void PlayHappyAnimation() => PlayAnimation("happy");
    }
}