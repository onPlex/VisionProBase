using UnityEngine;

public class ShellBasket : MonoBehaviour
{

    [Header("Tutorial")]
    [SerializeField]
    GameObject TutorialPhase3;
    [SerializeField]
    GameObject TutorialPhase4;

     [SerializeField]
    GameObject TutorialShell;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ShellTutorial"))
        {
            Debug.Log("ShellTutorial Enter");
            TutorialPhase3.SetActive(false);
            TutorialPhase4.SetActive(true);
            TutorialShell.SetActive(false);
        }
        else if (other.gameObject.CompareTag("Shell"))
        {
            Debug.Log("Shell Enter");
        }
    }
}
