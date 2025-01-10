using System.Collections;
using UnityEngine;

namespace Jun
{
    // [RequireComponent(typeof(MeshRenderer))]
    public class ShellBehavior : MonoBehaviour
    {
        [SerializeField] private Animator _anim;
        [SerializeField] private GameObject _textTitle;
        [SerializeField] private GameObject _buttonMake;

        public void Select(bool isSelected)
        {
            _anim.SetBool("isOpen", isSelected);
            StartCoroutine(DelayEvent());
        }

        private IEnumerator DelayEvent()
        {
            AnimatorStateInfo stateInfo = _anim.GetCurrentAnimatorStateInfo(0);
            float clipLength = stateInfo.length;
            Debug.Log(clipLength);
            yield return new WaitForSeconds(clipLength);
            _textTitle.SetActive(false);
            _buttonMake.SetActive(true);
        }
    }
}