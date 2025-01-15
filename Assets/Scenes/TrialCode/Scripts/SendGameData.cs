using System.Collections.Generic;
using Novaflo.Login;
using SimpleJSON;
using UnityEngine;

namespace Novaflo.Demo
{
    public class SendGameData : MonoBehaviour
    {
        public void SendGameResult()
        {
            // 예제 데이터 생성
            var game1 = new KochatGameData
            {
                Kind = "초등 개인 01",
                Title = "나의 흥미 조개 찾기",
                Cont = "뚝딱진주 : 솔직하고 성실한 성격을 가지고 있고, 몸을 많이 움직이는 활동을 좋아해요. 새로운 아이디어를 생각하기보다는 기계나 도구를 다루는 일을 좋아해요. 나만의 텃밭 가꾸기, DIY 활동, 요리 활동을 추천해요.",
                ImgType = 1,
                Status = 1
            };

            var game2 = new KochatGameData
            {
                Kind = "중학 개인 02",
                Title = "직업 월드컵 게임",
                Cont = "노무사는 회사와 직원들 사이에서 발생할 수 있는 문제를 법적으로 해결해주는 전문가예요. 노동 관련 법률 지식을 바탕으로 회사와 직원 간의 갈등을 예방하고 해결해요. 노무사가 되려면 회사와 직원들 사이에서 조정하고 협상하는 능력이 필요해요. 법을 해석하고 적용할 수 있는 논리력과 법률지식, 의뢰인과의 효과적인 커뮤니케이션을 위한 소통능력도 중요하답니다.",
                ImgType = 4,
                Status = 2
            };

            // 사용자 정보 가져오기
            var token = UserInfoManager.Instance.Token;
            var memberSeq = UserInfoManager.Instance.MemberSeq;

            List<KochatGameData> kochatGames = new List<KochatGameData>();
            kochatGames.Add(game1);
            kochatGames.Add(game2);


            KochatAPI.SendStageData(token, memberSeq, kochatGames,
            (result)=>{
                Debug.Log("result : " + result);
            },
            (error)=>{
                Debug.Log("error : " + error);
            });
        }
    }
}