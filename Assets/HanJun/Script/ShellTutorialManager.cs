using UnityEngine;
using TMPro;

namespace Jun
{
    public class ShellTutorialManager : MonoBehaviour
    {
        [SerializeField] private GameObject tutorialCountUI;
        [SerializeField] TMP_Text tutorialCountText;
        [SerializeField] private ShellBehavior shellBehavior;
        [SerializeField] private GameObject desc;
        [SerializeField] private GameObject title;
        [SerializeField] private GameObject tutorial2View;
        [SerializeField] private GameObject tutorial3View;

        public void SelectShellEvent()
        {
            title.SetActive(false);
            desc.SetActive(false);
            tutorial2View.SetActive(false);
            shellBehavior.SetAnimationEvent(false, MoveToBasket);
        }

        private void MoveToBasket()
        {
            tutorialCountUI.SetActive(true);
            this.gameObject.GetComponent<ShellThrow>().ThrowToTarget(() =>
            {
                shellBehavior.gameObject.SetActive(false);
                tutorialCountText.text = "1/6";
                tutorial3View.SetActive(true);
            }, 1f);
        }
    }
}