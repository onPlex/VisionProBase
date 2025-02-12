using Jun;
using UnityEngine;

namespace YJH
{
    public class ShellBasket : MonoBehaviour
    {
        [Header("Tutorial")]
        [SerializeField] private GameObject TutorialPhase3;
        [SerializeField] private GameObject TutorialPhase4;
        [SerializeField] private GameObject TutorialShell;
        [SerializeField] private GameObject[] Shells;
        [SerializeField] private MainContentManager mainManager;

        private void Start()
        {
            // MainContentManager에서 Phase 변경 이벤트에 대한 리스너 등록
            mainManager.OnPhaseChanged += UpdateShellsActivation;

            // 초기 활성화 상태 업데이트
            UpdateShellsActivation();
        }

        private void OnDestroy()
        {
            // 이벤트 리스너 해제 (메모리 누수 방지)
            if (mainManager != null)
            {
                mainManager.OnPhaseChanged -= UpdateShellsActivation;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("ShellTutorial"))
            {
                Debug.Log("ShellTutorial Enter");
                TutorialPhase3.SetActive(false);
                TutorialPhase4.SetActive(true);
                TutorialShell.SetActive(false);
                SoundManager.Instance.PlayEffect("SE 2_b");
            }
            else if (other.gameObject.CompareTag("Shell"))
            {
                ShellInfo shell = other.GetComponent<ShellInfo>();
                if (shell != null)
                {
                    SoundManager.Instance.PlayEffect("SE 2_b");

                    // ShellInfo.Career → int 변환 후, 등록
                    int cIndex = (int)shell.Career;
                    mainManager.RegisterShellSelection(cIndex);
                }
            }
        }

        /// <summary>
        /// currentPhase에 따라 Shells 배열의 활성화 상태를 업데이트
        /// </summary>
        private void UpdateShellsActivation()
        {
            int currentPhase = mainManager.GetCurrentPhase() - 1;

            if (currentPhase < 0)
            {
                return;
            }
            else
            {
                Shells[currentPhase].SetActive(true);                
            }
        }
    }
}
