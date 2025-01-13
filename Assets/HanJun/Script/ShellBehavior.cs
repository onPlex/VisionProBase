using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace Jun
{
    public class ShellBehavior : SpatialUIButton
    {
        public UnityEvent OnPress;
        [SerializeField] GameObject _descObject;
        [SerializeField] GameObject _titleObject;
        private Animator _anim;
        private BoxCollider _boxCollider;

        private void Awake()
        {
            _anim = GetComponent<Animator>();
            _boxCollider = GetComponent<BoxCollider>();
        }

        public override void Press()
        {
            base.Press();
            OnPress?.Invoke();
        }

        // 외부에서 이벤트를 동적으로 등록할 수 있는 메서드
        public void AddListener(UnityAction action)
        {
            OnPress.AddListener(action);
        }

        public void AllResetEvent()
        {
            EnabeldDescObject(false);
            EnabeldTitleObject(false);
            EnabledCollider(false);
        }

        public void RemoveListener(UnityAction action)
        {
            OnPress.RemoveListener(action);
        }

        public void EnabeldDescObject(bool enabled)
        {
            _descObject.gameObject.SetActive(enabled);
        }

        public void EnabeldTitleObject(bool enabled)
        {
            _titleObject.gameObject.SetActive(enabled);
        }

        public void EnabledCollider(bool enabled)
        {
            _boxCollider.enabled = enabled;
        }

        public void SetAnimationEvent(bool isSelected, UnityAction OnComplete = null)
        {
            _anim.SetBool("isOpen", isSelected);

            StartCoroutine(DelayFunc(OnComplete));
        }

        private IEnumerator DelayFunc(UnityAction OnComplete)
        {
            yield return new WaitForSeconds(1f);
            OnComplete?.Invoke();
        }

    }
}