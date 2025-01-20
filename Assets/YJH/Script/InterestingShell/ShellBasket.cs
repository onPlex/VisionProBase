using Jun;
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

    [SerializeField] private MainContentManager mainManager;

    [SerializeField] ParticleSystem particle;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ShellTutorial"))
        {
            Debug.Log("ShellTutorial Enter");
            TutorialPhase3.SetActive(false);
            TutorialPhase4.SetActive(true);
            TutorialShell.SetActive(false);

            particle.Play();
            SoundManager.Instance.PlayEffect("SE 2_b");
        }
        else if (other.gameObject.CompareTag("Shell"))
        {
            ShellInfo shell = other.GetComponent<ShellInfo>();
            if (shell != null)
            {
                particle.Play();
                SoundManager.Instance.PlayEffect("SE 2_b");
                // ShellInfo.Career → int 변환
                int cIndex = (int)shell.Career;
                mainManager.RegisterShellSelection(cIndex);
            }
        }
    }

}
