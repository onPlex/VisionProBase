using System.Collections.Generic;
using Novaflo.Login;
using UnityEngine;
using Novaflo.Demo;
using System;

namespace YJH
{
    public class SendResultData : MonoBehaviour
    {
        [SerializeField]
        string kindData;
         [SerializeField]
        string TitleData;
          

        public void SendGameResult(String ContDATA, int ImgTypeDATA, int StatusDATA)
        {
            // 예제 데이터 생성
            var game = new KochatGameData
            {
                Kind = kindData,
                Title = TitleData,
                Cont = ContDATA,
                ImgType = ImgTypeDATA,
                Status = StatusDATA
            };
            // 사용자 정보 가져오기
            var token = UserInfoManager.Instance.Token;
            var memberSeq = UserInfoManager.Instance.MemberSeq;

            List<KochatGameData> kochatGames = new List<KochatGameData>();
            kochatGames.Add(game);
            KochatAPI.SendStageData(token, memberSeq, kochatGames,
            (result) =>
            {
                Debug.Log("result : " + result);
            },
            (error) =>
            {
                Debug.Log("error : " + error);
            });
        }
    }
}

