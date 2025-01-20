using UnityEngine;
using TMPro;

public class MainContentManager : MonoBehaviour
{
    [Header("Phases")]
    // 0~5까지의 Phase Parent 오브젝트
    [SerializeField] private GameObject[] phaseParents = new GameObject[6];

    // 현재 진행중인 Phase 인덱스(0~5)
    private int currentPhase = 0;



    // Career 선택 카운트 (R=0, I=1, A=2, S=3, E=4, C=5)
    private int[] careerSelectedCounts = new int[6];

    [Header("UI")]
    [SerializeField]
    TMP_Text TMP_PhaseText;


    [Header("Result")]
    [SerializeField] Jun.Dolphin dolphin;
    [SerializeField] GameObject Basket;
    [SerializeField] GameObject ShellCountUI;

    public ShellInfo.CareerType finalCareer;

    private void Start()
    {
        // 초기 셋업
        //InitializePhases();
    }

    /// <summary>
    /// 모든 PhaseParent를 비활성화 후, 현재 phaseParent만 활성화
    /// </summary>
    private void InitializePhases()
    {
        // 전체 PhaseParent 비활성화
        for (int i = 0; i < phaseParents.Length; i++)
        {
            if (phaseParents[i] != null)
                phaseParents[i].SetActive(false);
        }
        // 현재 Phase만 활성화
        ActivateCurrentPhase();
    }

    /// <summary>
    /// phaseParents[currentPhase]만 활성화
    /// </summary>
    private void ActivateCurrentPhase()
    {
        if (currentPhase < 0 || currentPhase >= phaseParents.Length)
        {
            Debug.LogWarning("Phase 범위 에러");
            return;
        }

        if (phaseParents[currentPhase] != null)
            phaseParents[currentPhase].SetActive(true);

        // currentPhase는 0~5이므로, 실제 표기는 1~6이 되도록 (currentPhase + 1)
        if (TMP_PhaseText != null)
        {
            TMP_PhaseText.text = $"{(currentPhase)}/{phaseParents.Length}";
        }
    }

    /// <summary>
    /// ShellBasket 등에서 Shell이 선택되었다고 알림받으면,
    /// 해당 Shell의 CareerType을 카운트하고 다음 Phase로 넘어감
    /// </summary>
    public void RegisterShellSelection(int careerIndex)
    {
        if (careerIndex < 0 || careerIndex >= 6)
        {
            Debug.LogWarning("Invalid careerIndex");
            return;
        }

        // 카운트 증가
        careerSelectedCounts[careerIndex]++;

        // 다음 Phase
        NextPhase();
    }

    /// <summary>
    /// Phase 1 증가 후, 끝났으면 결과 산출
    /// </summary>
    private void NextPhase()
    {
        // 현재 PhaseParent 비활성화
        if (phaseParents[currentPhase] != null)
            phaseParents[currentPhase].SetActive(false);

        currentPhase++;

        if (currentPhase < phaseParents.Length)
        {
            // 새 Phase 활성화
            ActivateCurrentPhase();
        }
        else
        {
            // 모든 Phase 끝났으면 결과 계산
            CalculateFinalCareer();

            // 결과 Phase로 이동
            GoToResultPhase();
        }
    }

    /// <summary>
    /// R,I,A,S,E,C 중 최다 선택된 항목 판별
    /// </summary>
    private void CalculateFinalCareer()
    {
        int maxIndex = 0;
        for (int i = 1; i < 6; i++)
        {
            if (careerSelectedCounts[i] > careerSelectedCounts[maxIndex])
                maxIndex = i;
        }
        finalCareer = (ShellInfo.CareerType)maxIndex;
        Debug.Log($"가장 많이 선택된 유형: {finalCareer}");
        // TODO: 결과 UI 표시, 씬 전환 등
    }

    private void GoToResultPhase()
    {
        // TODO: 결과 화면(Phase) 활성화, 씬 전환, UI 표시 등
        // ex) resultPhaseObject.SetActive(true);
        // ex) SceneManager.LoadScene("ResultScene");
        // ...

        Basket.SetActive(false);
        ShellCountUI.SetActive(false);
        dolphin.gameObject.SetActive(true);
        if (dolphin) dolphin.PlayAnimation();
    }
}
