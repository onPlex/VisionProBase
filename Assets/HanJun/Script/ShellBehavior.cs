using System.Collections;
using TMPro;
using UnityEngine;

namespace Jun
{
    public class ShellBehavior : MonoBehaviour
    {
        [SerializeField] private Animator _anim;
        [SerializeField] private GameObject _textTitle;
        [SerializeField] private GameObject _buttonMake;
        [SerializeField] private GameObject _textDesc;
        [SerializeField] private GameObject _textInfo;

        [SerializeField] TMP_Text _shellCount;

        public void Select(bool isSelected)
        {
            _anim.SetBool("isOpen", isSelected);
            StartCoroutine(DelayEvent(isSelected));
        }

        private IEnumerator DelayEvent(bool isSelected)
        {
            if (isSelected)
            {
                _textTitle.SetActive(false);
                yield return new WaitForSeconds(0.67f);
                _buttonMake.SetActive(true);
            }
            else
            {
                _textDesc.SetActive(false);
                yield return new WaitForSeconds(1f);
                _shellCount.text = "1/6";
                _textInfo.SetActive(true);
                this.gameObject.SetActive(false);
            }
        }
    }
}