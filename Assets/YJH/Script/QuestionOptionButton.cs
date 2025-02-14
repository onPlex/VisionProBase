using UnityEngine;
using TMPro;
using System.Collections;
using YJH.EmotionJewel;

namespace YJH
{
    public class QuestionOptionButton : SpatialButtonEvent
    {
        private int optionIndex;
        public System.Action<int> OnOptionSelected;

        [SerializeField]
        TMP_Text textMesh;

        [SerializeField]
        bool IsEmotionJewel = true;
        AnswerButton answerButton;

        [Header("Option")]
        [SerializeField]
        float ButtonDelayTime = 3f;

        void Start()
        {
            answerButton = GetComponent<AnswerButton>();
        }

        /// <summary>
        /// Sets the option text for the button (e.g., TextMesh or TextMeshPro).
        /// </summary>
        /// <param name="text">Option text.</param>
        public void SetOptionText(string text)
        {
            //textMesh = GetComponentInChildren<TMP_Text>(); // Replace with TextMeshPro if needed
            if (textMesh != null)
            {
                textMesh.text = text;
            }
        }

        /// <summary>
        /// Sets the index of this option.
        /// </summary>
        /// <param name="index">Option index.</param>
        public void SetOptionIndex(int index)
        {
            optionIndex = index;
        }

        private bool hasPressed = false;

        public override void Press()
        {
            if (hasPressed) return;
            hasPressed = true;

            if (IsEmotionJewel)
            {
                if (answerButton) answerButton.OnClickEffect();
                else
                {
                    answerButton = GetComponent<AnswerButton>();
                    answerButton.OnClickEffect();
                }
            }

            // Trigger the action
            OnOptionSelected?.Invoke(optionIndex);

            // Allow pressing again after a short delay
            if (gameObject.activeInHierarchy) StartCoroutine(ResetPressDelay(ButtonDelayTime));
        }


        private IEnumerator ResetPressDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            hasPressed = false;
        }
    }
}