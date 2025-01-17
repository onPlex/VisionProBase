using UnityEngine;


namespace YJH
{
    public class ResultPass : MonoBehaviour
    {

        [SerializeField] EmotionJewelResult emotionJewelResult;
        private void OnDisable()
        {
            emotionJewelResult.CalculateFinalResult();
        }
    }
}
