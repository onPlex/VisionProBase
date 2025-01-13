using System.Collections;
using UnityEngine;
using TMPro;
using YJH;

public class EmotionJewelPrologue : MonoBehaviour
{
    [SerializeField]
    GameObject AnswerButtons;

    [SerializeField]
    TMP_Text QuestionBoardText;

    [SerializeField]
    PhaseManagerEmotionJewel phaseManagerEmotionJewel;

    private void OnEnable()
    {
        // * 코루틴으로 연출 진행 
        //요정 등장 연출
        //요정 대사 진행​


        /*안녕하세요? 저는 강점숲의 요정, 포르테라고 해요.
        ​이 세상 모든 사람은​ 저마다 반짝반짝 빛나는​ 강점 보석을 가지고 있다는 걸,​ 
        혹시 알고 있나요?*/

        /*당신도 강점 보석을 찾을 수 있도록 도와줄게요.​ 저와 함께 강점숲으로 떠나봐요!​*/


        //대사 연출 끝나면 
    }

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
