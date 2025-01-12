using UnityEngine;
using TMPro;
using System.Collections;

namespace YJH
{
    public class QuestionOptionButton : SpatialButtonEvent
    {
        private int optionIndex;
        public System.Action<int> OnOptionSelected;

        [SerializeField]
        TMP_Text textMesh;

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

            // 실제 실행
            OnOptionSelected?.Invoke(optionIndex);

            // 예: 0.2초 후 다시 눌릴 수 있게 허용
            StartCoroutine(ResetPressDelay(0.2f));
        }

        private IEnumerator ResetPressDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            hasPressed = false;
        }
    }
}