using UnityEngine;

public class PinInputController : MonoBehaviour
{
    [SerializeField] private PinInputManager pinInput;

    private void Start()
    {
        // PIN 입력 완료 이벤트 등록
        pinInput.onPinComplete.AddListener(OnPinComplete);

        // 초기 포커스 설정
        pinInput.FocusInput();
    }

    private void OnPinComplete(string pin)
    {
        Debug.Log($"입력된 PIN: {pin}");

        // PIN 검증
        if (ValidatePin(pin))
        {
            // 성공 처리
            Debug.Log("PIN 입력 성공!");
        }
        else
        {
            // 실패 처리
            Debug.Log("잘못된 PIN");
            pinInput.ResetDisplay();
        }
    }

    private bool ValidatePin(string pin)
    {
        // PIN 검증 로직 구현
        return pin == "1234"; // 예시
    }
}
