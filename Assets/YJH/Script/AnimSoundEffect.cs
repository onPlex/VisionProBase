using UnityEngine;
using YJH;

public class AnimSoundEffect : MonoBehaviour
{
    public void PlayEffectSoundByName(string clipName)
    {
        AudioManager.Instance.PlaySFX(clipName);
    }
}
