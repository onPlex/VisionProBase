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
            // 1) 이벤트 등록 (int 파라미터 받는 콜백)
            mainManager.OnPhaseChanged += UpdateShellsActivation;

            // 2) 씬 시작 시 현재 Phase에 맞춰 초기 활성화
            UpdateShellsActivation(mainManager.GetCurrentPhase());
        }

        private void OnDestroy()
        {

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

        private void UpdateShellsActivation(int phase)
        {
            // 기존 코드에서 ShellBasket은 "Phase - 1" 인덱스를 사용
            int shellIndex = phase - 1;
            if (shellIndex < 0)
            {
                // 0단계일 때는 스킵하거나, 필요하면 특정 로직
                return;
            }

            // 배열 범위를 벗어나지 않는지 확인
            if (shellIndex < Shells.Length)
            {
                // 이전 단계들까지 전부 켤지, 또는 이전 단계는 끌지 등은
                // 기획에 따라 수정 가능
                Shells[shellIndex].SetActive(true);
            }
        }
    }
}
