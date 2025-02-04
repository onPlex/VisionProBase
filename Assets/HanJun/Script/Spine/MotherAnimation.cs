using UnityEngine;

namespace Jun
{
    public class MotherAnimation : RawImageAnimation
    {
        public void PlayAngryAnimation() => PlayAnimation("angry");
        public void PlaySadAnimation() => PlayAnimation("sad");
    }
}