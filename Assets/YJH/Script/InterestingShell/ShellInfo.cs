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
    [Header("UI")]
    [SerializeField]
    TMP_Text WordTMP;
    [SerializeField]
    TMP_Text WordDescTMP;



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

}
