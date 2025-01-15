using Novaflo.Common;
using UnityEngine;

namespace Novaflo.Login
{
    public class UserInfoManager : Singleton<UserInfoManager>
    {
        public int MemberSeq { get; private set; } // 사용자 고유 번호
        public string Token { get; private set; }  // 인증 토큰
        public string Name { get; private set; }   // 사용자 이름
        public string Tel { get; private set; }    // 사용자 전화번호

        /// <summary>
        /// 사용자 정보를 설정합니다.
        /// </summary>
        /// <param name="memberSeq">사용자 고유 번호</param>
        /// <param name="token">인증 토큰</param>
        /// <param name="name">사용자 이름</param>
        /// <param name="tel">사용자 전화번호</param>
        public void SetUserInfo(int memberSeq, string token, string name, string tel)
        {
            MemberSeq = memberSeq;
            Token = token;
            Name = name;
            Tel = tel;
        }

        /// <summary>
        /// 사용자 정보를 초기화합니다.
        /// </summary>
        public void ClearUserInfo()
        {
            MemberSeq = 0;
            Token = string.Empty;
            Name = string.Empty;
            Tel = string.Empty;
        }

        /// <summary>
        /// 사용자 정보를 디버그 로그에 출력합니다.
        /// </summary>
        public void LogUserInfo()
        {
            Debug.Log($"User Info:\nMemberSeq: {MemberSeq}\nToken: {Token}\nName: {Name}\nTel: {Tel}");
        }
    }
}