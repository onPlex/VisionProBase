using System.Collections;
using UnityEngine;
using TMPro;

namespace YJH.EmotionJewel
{
    public class EmotionJewelTutorial : MonoBehaviour
    {

        [SerializeField]
        GameObject AnswerButtons;

        [SerializeField]
        TMP_Text QuestionBoardText;

        [SerializeField]
        PhaseManagerEmotionJewel phaseManagerEmotionJewel;
      
       public void OnTutorialEnd()
        {
            StartCoroutine(ITutorialEnd());
        }

        IEnumerator ITutorialEnd()
        {
            // 선택 버튼 사라짐 연출
            AnswerButtons.SetActive(false);
            //튜토리얼 완료​ 안내 문구 표시​ (잘 했어요! 이제 질문에 맞춰 원하는 답을 선택해봐요!​)
            QuestionBoardText.text = "잘 했어요! 이제 질문에 맞춰 원하는 답을 선택해봐요!";
            yield return new WaitForSeconds(2.0f);

            phaseManagerEmotionJewel.OnStage1Start();
        }
    }
}