using UnityEngine;
using TMPro;

public class ShellInfo : MonoBehaviour
{

    public enum CareerType
    {
        Realistic,    // 현실형
        Investigative, // 탐구형
        Artistic,     // 예술형
        Social,       // 사회형
        Enterprising, // 기업형
        Conventional  // 관습형
    }

    public bool IsCollected { get; set; }

    [Header("UI")]
    [SerializeField]
    TMP_Text WordTMP;
    [SerializeField]
    TMP_Text WordDescTMP;

    [Header("Comp")]
    [SerializeField]
    GameObject DecObj;
    private BoxCollider ManipulationBoxCollider;


    [Header("Data")]
    [SerializeField]
    private string wordText;
    [SerializeField]
    private string wordDesc;
    [SerializeField]
    private CareerType careerType;

    // --- Encapsulated Properties ---
    public string WordText
    {
        get => wordText;
        set => wordText = value;
    }

    public string WordDesc
    {
        get => wordDesc;
        set => wordDesc = value;
    }

    public CareerType Career
    {
        get => careerType;
        set => careerType = value;
    }

    public void DisplayInfo()
    {
        WordTMP.text = wordText;
        WordDescTMP.text = wordDesc;
    }


    // [ 초기 위치/회전/스케일 저장용 필드
    [SerializeField]
    private Vector3 initialPosition;
    [SerializeField]
    private Quaternion initialRotation = new Quaternion(0,0,0,1);

    private void Awake()
    {
        // 게임 시작 시점의 Transform 상태를 기억해둡니다.
       // initialPosition = transform.position;
        //initialRotation = transform.rotation;       
        ManipulationBoxCollider = GetComponent<BoxCollider>();
    }


    public void ResetShell()
    {
        if (ManipulationBoxCollider) ManipulationBoxCollider.enabled = false;
        else
        {
            ManipulationBoxCollider = GetComponent<BoxCollider>();
            ManipulationBoxCollider.enabled = false;
        }
        // Transform 되돌리기
        transform.localPosition = initialPosition;
        transform.localRotation = initialRotation;

        // IsCollected 리셋
        IsCollected = false;
        DecObj.SetActive(false);

        // 혹은 Shell의 collider를 잠시 비활성 -> 몇 프레임 뒤 활성
       // var coll = GetComponent<Collider>();
        //if (coll != null) coll.enabled = false;
        //Invoke(nameof(ReEnableCollider), 0.1f);
    }

    private void ReEnableCollider()
    {
        var coll = GetComponent<Collider>();
        if (coll != null) coll.enabled = true;
    }

}
