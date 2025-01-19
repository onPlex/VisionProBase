using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class PinInputManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_InputField hiddenInputField;  // 실제 입력을 받는 숨겨진 InputField
    [SerializeField] private TMP_Text[] displayTexts;          // 4개의 숫자 표시 텍스트
    [SerializeField] private Image[] digitBackgrounds;         // 각 숫자칸의 배경

    [Header("Visual Settings")]
    [SerializeField] private bool usePasswordMask = true;      // 비밀번호 마스킹 사용 여부
    [SerializeField] private Color normalColor = Color.white;  // 빈 칸 색상
    [SerializeField] private Color activeColor = new Color(0.9f, 0.9f, 0.9f);  // 입력된 칸 색상

    [Header("Events")]
    public UnityEvent<string> onPinComplete;  // PIN 입력 완료시 이벤트
    public UnityEvent onPinReset;            // PIN 리셋시 이벤트

    private const int PIN_LENGTH = 4;
    private string currentPin = "";

    private void Start()
    {
        SetupInputField();
        ResetDisplay();
    }

    private void SetupInputField()
    {
        if (hiddenInputField != null)
        {
            hiddenInputField.contentType = TMP_InputField.ContentType.IntegerNumber;
            hiddenInputField.characterLimit = PIN_LENGTH;
            hiddenInputField.onValueChanged.AddListener(OnInputValueChanged);
        }
        else
        {
            Debug.LogError("Hidden InputField is not assigned!");
        }
    }

    private void OnInputValueChanged(string value)
    {
        // 숫자만 허용
        string filteredValue = System.Text.RegularExpressions.Regex.Replace(value, "[^0-9]", "");

        if (filteredValue != value)
        {
            hiddenInputField.text = filteredValue;
            return;
        }

        currentPin = filteredValue;
        UpdateDisplay();

        // PIN 입력이 완료되면 이벤트 발생
        if (currentPin.Length == PIN_LENGTH)
        {
            onPinComplete?.Invoke(currentPin);
        }
    }

    private void UpdateDisplay()
    {
        for (int i = 0; i < displayTexts.Length; i++)
        {
            // 텍스트 업데이트
            if (i < currentPin.Length)
            {
                displayTexts[i].text = usePasswordMask ? "*" : currentPin[i].ToString();
                // displayTexts[i].text = currentPin[i].ToString();
                digitBackgrounds[i].color = activeColor;
            }
            else
            {
                displayTexts[i].text = "";
                digitBackgrounds[i].color = normalColor;
            }
        }
    }

    public void ResetDisplay()
    {
        currentPin = "";
        hiddenInputField.text = "";
        UpdateDisplay();
        onPinReset?.Invoke();
    }

    // PIN 입력 필드에 포커스
    public void FocusInput()
    {
        hiddenInputField.ActivateInputField();
    }

    // 현재 PIN 값 반환
    public string GetCurrentPin()
    {
        return currentPin;
    }
}
