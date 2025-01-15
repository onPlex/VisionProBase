using Novaflo.Login;
using SimpleJSON;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TrialCodeInput : MonoBehaviour
{
    public TMP_Text[] NumPadTexts; // 핀코드 숫자 UI 텍스트 배열
    public GameObject ErrorMessage; // 오류 메시지 UI
    public GameObject LoginFailed; // 로그인 실패 UI
    private int currentIndex = 0; // 현재 입력 위치를 추적하는 변수
    public bool isComplete;

    public ButtonState LoginButton;

    public UnityEvent LoginSuccessEvent;

    public UnityEvent StartGameEvent;

    // 버튼 클릭 시 호출될 메서드
    public void NumpadInput(string numberStr)
    {
        // 입력 가능한 자리 수 확인
        if (currentIndex < NumPadTexts.Length)
        {
            NumPadTexts[currentIndex].text = numberStr; // 현재 위치에 숫자 입력
            currentIndex++; // 위치 이동

           UpdateTextState();
        }
    }

    private void UpdateTextState()
    {
            // 모든 자리 입력 완료 시 처리
            if (currentIndex == NumPadTexts.Length)
            {
                isComplete = true;
                
                ErrorMessage.SetActive(false);
                LoginFailed.SetActive(false); 

                LoginButton.UpdateButtonState(true);
            }
            else
            {
                isComplete = false;
                LoginButton.UpdateButtonState(false);
            }
    }

    public void OnLogin()
    {
        if(isComplete)
        {
            string authCode = null;

            foreach(var np in NumPadTexts)
            {
                authCode += np.text;
            }

            Debug.Log("authCode : " + authCode);

            KochatAPI.Authenticate(authCode, 
            (result=>{
                  
                // JSON 파싱
                var jsonResponse = JSON.Parse(result);
                int resultCode = jsonResponse["resultCode"].AsInt;
                string token = jsonResponse["token"];
                int memberSeq = jsonResponse["memberSeq"].AsInt;
                string name = jsonResponse["name"];
                string tel = jsonResponse["tel"];

                if (resultCode == 200)
                {
                    Debug.Log("Login Success");
                    Debug.Log($"Token: {token}, MemberSeq: {memberSeq}, Name: {name}, Tel: {tel}");
                    
                    UserInfoManager.Instance.SetUserInfo(memberSeq, token, name, tel);

                    // 성공적인 로그인 처리 로직 추가
                    LoginSuccessEvent?.Invoke();
                }
                else
                {
                    Debug.Log("Login Failed: " + resultCode);
                    LoginFailed.SetActive(true); // 로그인 실패 UI 표시
                    ClearNumpad(); // 입력 초기화
                }
                
            }),
            (error)=>{
                Debug.Log("error : " + authCode);
                ClearNumpad();

                ErrorMessage.SetActive(true);
            });
        }
    }

    // 모든 입력 값 초기화
    public void ClearNumpad()
    {
        ClearUserInfo();
        foreach (var num in NumPadTexts)
        {
            num.text = "";
        }
        currentIndex = 0; // 입력 위치 초기화
        isComplete = false;
    }

    public void DeleteInput()
    {
        // 입력 중일 때만 실행
        if (currentIndex > 0)
        {
            currentIndex--; // 현재 인덱스를 한 칸 뒤로 이동
            NumPadTexts[currentIndex].text = ""; // 해당 인덱스의 입력 초기화
        }
        else
        {
            Debug.Log("No input to delete."); // 삭제할 입력이 없을 때
        }

        UpdateTextState();
    }

    public void ClearUserInfo()
    {
        UserInfoManager.Instance.ClearUserInfo();
    }

    public void StartGame()
    {
        StartGameEvent?.Invoke();
    }
}
