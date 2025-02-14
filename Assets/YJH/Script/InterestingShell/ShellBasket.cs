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

       [Header("PopUpObject")]
        [SerializeField] private GameObject GuideTextObj;

        private void Start()
        {
            // 1) 이벤트 등록 (int 파라미터 받는 콜백)
            mainManager.OnPhaseChanged += UpdateShellsActivation;

            // 2) 씬 시작 시 현재 Phase에 맞춰 초기 활성화
            UpdateShellsActivation(mainManager.CurrentPhase);
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
            // 튜토리얼 Shell (예: ShellTutorial 태그)
            if (other.gameObject.CompareTag("ShellTutorial"))
            {
                Debug.Log("ShellTutorial Enter");
                TutorialPhase3.SetActive(false);
                TutorialPhase4.SetActive(true);
                TutorialShell.SetActive(false);
                SoundManager.Instance.PlayEffect("SE 2_b");
            }
            // 일반 Shell
            else if (other.gameObject.CompareTag("Shell"))
            {
                if(GuideTextObj.activeInHierarchy)GuideTextObj.SetActive(false);
                ShellInfo shell = other.GetComponent<ShellInfo>();
                if (shell != null && !shell.IsCollected)
                {
                    // 아직 수집 안 된 Shell만 처리
                    shell.IsCollected = true; // 다시는 트리거되지 않도록 플래그 설정

                    SoundManager.Instance.PlayEffect("SE 2_b");

                    int cIndex = (int)shell.Career;
                    mainManager.RegisterShellSelection(cIndex);

                    // 필요하다면, 다시 트리거 안 되게 collider 비활성화 or 오브젝트 Destroy
                    //Destroy(other.gameObject);
                }
            }
        }

        private void UpdateShellsActivation(int newPhase)
        {
           // 배열 전체 순회
            for (int i = 0; i < Shells.Length; i++)
            {
                // i < newPhase 이면 활성화, 아니면 비활성화
                bool shouldActive = (i < newPhase);

                if (Shells[i] != null)
                {
                    Shells[i].SetActive(shouldActive);
                }
            }
        }
    }
}
