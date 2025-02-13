using System.Collections;
using UnityEngine;

namespace YJH.EmotionJewel
{
    public class EmotionJewelPrologue : MonoBehaviour
    {

        [SerializeField]
        PhaseManagerEmotionJewel phaseManagerEmotionJewel;

        [SerializeField]
        GameObject Step1Obj;
        [SerializeField]
        GameObject Step2Obj;


        public void OnProcessStep2()
        {
            Step1Obj.SetActive(false);
            Step2Obj.SetActive(true);
        }

        public void OnProloguelEnd()
        {
            StartCoroutine(IPrologueEnd());
        }

        IEnumerator IPrologueEnd()
        {

            yield return new WaitForSeconds(2.0f);
            phaseManagerEmotionJewel.OnTutorialStart();
        }
    }
}