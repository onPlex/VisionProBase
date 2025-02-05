using System.Collections.Generic;
using Novaflo.Login;
using UnityEngine;
using System;

namespace YJH
{
    public class SendResultData : MonoBehaviour
    {
        [SerializeField]
        string Game1KindData;
        [SerializeField]
        string Game2KindData;
        [SerializeField]
        string Game1TitleData;
        [SerializeField]
        string Game2TitleData;


        public void SendGameResult(String Game1ContDATA, int Game1ImgTypeDATA, int Game1StatusDATA,
        String Game2ContDATA, int Game2ImgTypeDATA, int Game2StatusDATA)
        {



            var game1 = new KochatGameData
            {
                Kind = Game1KindData,
                Title = Game1TitleData,
                Cont = Game1ContDATA,
                ImgType = Game1ImgTypeDATA,
                Status = Game1StatusDATA
            };

            var game2 = new KochatGameData
            {
                Kind = Game2KindData,
                Title = Game2TitleData,
                Cont = Game2ContDATA,
                ImgType = Game2ImgTypeDATA,
                Status = Game2StatusDATA
            };

            // 사용자 정보 가져오기
            var token = UserInfoManager.Instance.Token;
            var memberSeq = UserInfoManager.Instance.MemberSeq;

            List<KochatGameData> kochatGames = new List<KochatGameData>();
            kochatGames.Add(game1);
            kochatGames.Add(game2);


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

