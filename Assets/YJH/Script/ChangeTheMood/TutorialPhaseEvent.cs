using UnityEngine;
using TMPro;

namespace YJH.ChangeTheMood
{
    public class TutorialPhaseEvent : MonoBehaviour
    {
        [SerializeField]
        PhaseManager phaseManager;

        [Header("Step")]
        [SerializeField]
        GameObject Step1;
        [SerializeField]
        GameObject Step2;

        [Header("Sex")]
        [SerializeField]
        GameObject Boy;
        [SerializeField]
        GameObject Girl;

        [Header("UI")]
        [SerializeField]
        TMP_Text CharacterInfoText;
  [SerializeField]
        TMP_Text DiaryNextPhaseText;
        /**/


        private void OnEnable()
        {
            if (phaseManager.SelectedSex == PhaseManager.Sex.Boy)
            {
                Boy.SetActive(true);
                Girl.SetActive(false);
            }
            else
            {
                Boy.SetActive(false);
                Girl.SetActive(true);
            }

            // Determine sex text
            string sexText = phaseManager.SelectedSex == PhaseManager.Sex.Boy ? "남자" : phaseManager.SelectedSex == PhaseManager.Sex.Girl ? "여자" : "선택 안 함";

            // Update CharacterInfoText
            CharacterInfoText.text = $"이름 : {phaseManager.SelectedNickname}\n" +
                                     $"성별 : {sexText}\n" +
                                     $"나이 : 11살\n" +
                                     $"학교 : 방실초등학교\n" +
                                     $"성적 : 우수\n" +
                                     $"목표 : 이번 학기, 우리 반 소통 에이스는 바로 나 !";



            DiaryNextPhaseText.text = $"{phaseManager.SelectedNickname}의 하루를 바꿔볼까요?";
        }

        public void OnStep2()
        {
            Step1.SetActive(false);
            Step2.SetActive(true);
        }
    }
}