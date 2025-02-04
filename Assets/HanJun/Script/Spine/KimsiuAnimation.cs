using UnityEngine;

namespace Jun
{
    public class KimsiuAnimation : RawImageAnimation
    {
        public void PlayAngryAnimation() => PlayAnimation("angry");
        public void PlayEmbarrassedAnimation() => PlayAnimation("embarrased");
        public void PlayGoodAnimation() => PlayAnimation("good");
        public void PlayHelloAnimation() => PlayAnimation("hello");
        public void PlaySadAnimation() => PlayAnimation("sad");
    }
}
