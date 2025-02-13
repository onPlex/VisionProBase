using UnityEngine;


namespace YJH.EmotionJewel
{
    public class Letter : MonoBehaviour
    {

        [SerializeField]
        EmotionJewelPrologue emotionJewelPrologue;


        public void OnLetterDisappearComplete()
        {
           if(emotionJewelPrologue) emotionJewelPrologue.OnProcessStep2();
        }
    }
}
