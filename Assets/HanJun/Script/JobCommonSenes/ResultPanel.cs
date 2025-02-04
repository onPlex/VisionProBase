using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Jun
{
    public class ResultPanel : MonoBehaviour
    {
        [SerializeField] OXQuizManager oXQuizManager;
        [SerializeField] TMP_Text descText;
        [SerializeField] TMP_Text nameText;
        [SerializeField] TMP_Text scoreText;

        [SerializeField] Image charecter;
        [SerializeField] Sprite[] charecters;

        private void OnEnable()
        {
            SetResultPanel();
        }

        public void SetResultPanel()
        {
            int score = oXQuizManager.ScoreNumber;
            scoreText.text = $"<color=red>{score}</color> / 10";
            if (score <= 3)
            {
                descText.text = "직업에 대한 열정만 가득한 ";
                nameText.text = "직업 열정러";
                if (charecters.Length > 0) charecter.sprite = charecters[0];
            }
            else if (score > 3 && score <= 7)
            {
                descText.text = "다양한 직업적 역량을 탐험하는";
                nameText.text = "직업 탐험가";
                if (charecters.Length > 0) charecter.sprite = charecters[1];
            }
            else if (score >= 7 && score <= 10)
            {
                descText.text = "모든 직업의 비밀을 알고 있는 단계";
                nameText.text = "직업 마스터";
                if (charecters.Length > 0) charecter.sprite = charecters[2];
            }


        }
    }
}