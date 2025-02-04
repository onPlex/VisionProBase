using UnityEngine;

namespace Jun
{
    public class ManAnimation : RawImageAnimation
    {
        public void PlayHelloAnimation() => PlayAnimation("hello");
        public void PlaySadAnimation() => PlayAnimation("sad");
    }
}