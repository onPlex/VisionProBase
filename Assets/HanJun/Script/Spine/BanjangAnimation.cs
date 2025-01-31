using UnityEngine;

namespace Jun
{
    public class BanjangAnimation : RawImageAnimation
    {
        public void PlaySadAnimation() => PlayAnimation("sad");
        public void PlayAnnoyAnimation() => PlayAnimation("annoy");
    }
}