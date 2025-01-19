using UnityEngine;
using TMPro;
using UnityEngine.Events;
using System.Collections;

public class ShellCopiedLogic : MonoBehaviour
{
    [SerializeField] private GameObject title;
    [SerializeField] private GameObject dec;

    [SerializeField] private BoxCollider ManipulationBoxCollider;


    private Animator _anim;
    private BoxCollider _boxCollider;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _boxCollider = GetComponent<BoxCollider>();
    }

    public void SelectShellEvent()
    {
        SetAnimationEvent(true, ShellOpened);
    }

    private void ShellOpened()
    {
        _boxCollider.enabled = false;
        title.SetActive(true);
    }

    public void OnClickTitleButton()
    {
        dec.SetActive(true);
        ManipulationBoxCollider.enabled = true;
    }


    #region  Anim
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
    #endregion
}
