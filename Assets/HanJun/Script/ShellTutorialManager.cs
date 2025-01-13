using UnityEngine;
using TMPro;

namespace Jun
{
    public class ShellTutorialManager : MonoBehaviour
    {
        [SerializeField] TMP_Text _countText;
        [SerializeField] private ShellBehavior shellBehavior;
        [SerializeField] private GameObject _nextUI;

        public void SetEvent()
        {
            shellBehavior.SetAnimationEvent(false, SetText);
        }

        private void SetText()
        {
            _countText.text = "1/6";
            shellBehavior.gameObject.SetActive(false);
            _nextUI.SetActive(true);
        }
    }
}