using Novaflo.Login;
using TMPro;
using UnityEngine;

public class UpdateResultView : MonoBehaviour
{
    public TMP_Text userInfo;

    public void OnEnable()
    {
        // 사용자 정보 가져오기
        var userName = UserInfoManager.Instance.Name;
        var tel = UserInfoManager.Instance.Tel;

        // 사용자 정보를 포맷에 맞게 설정
        userInfo.text = string.Format("이름 : {0}\n전화번호 : {1}", userName, tel);
    }
}