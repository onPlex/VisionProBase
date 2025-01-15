using System;
using System.Collections.Generic;
using Novaflo.Common;
using SimpleJSON;
using UnityEngine;

namespace Novaflo.Login
{
    public static class KochatAPI
    {
        private const string BaseUrl = "https://api.kochatgpt.com:8004";


        /// <summary>
        /// Authenticate with the API to retrieve a token.
        /// </summary>
        /// <param name="authCode">The 4-digit authentication code.</param>
        /// <param name="onSuccess">Callback for a successful response.</param>
        /// <param name="onError">Callback for an error response.</param>
        public static void Authenticate(string authCode, Action<string> onSuccess, Action<string> onError)
        {
            string url = $"{BaseUrl}/data/auth";
            string method = "POST";
            var headers = new Dictionary<string, string> { 
                { "Content-Type", "application/json" },
                { "charset", "UTF-8" }
            };

            var payload = new JSONObject
            {
                ["authCode"] = authCode
            };

            string body = payload.ToString();

            HttpClient.Instance.SendRequest(url, method, headers, body, onSuccess, onError);
        }

        public static void SendStageData(string token, int memberSeq, List<KochatGameData> gameDataList, Action<string> onSuccess, Action<string> onError)
        {
            string url = $"{BaseUrl}/data/stage1";
            string method = "POST";
            var headers = new Dictionary<string, string>
            {
                { "Content-Type", "application/json" },
                { "d-token", token }
            };

    
            // 데이터 전송 형식 변경
            var payload = new JSONObject
            {
                ["memberSeq"] = memberSeq
            };

            int index = 1;
            foreach(var game in gameDataList)
            {
                var json = new JSONObject
                {
                    ["kind"] = game.Kind,
                    ["title"] = game.Title,
                    ["cont"] = game.Cont,
                    ["imgType"] = game.ImgType,
                    ["status"] = game.Status
                };

                var key = "game"+index;
                payload.Add(key, json);
                index++;
            }

            string body = payload.ToString();
            
            HttpClient.Instance.SendRequest(url, method, headers, body, onSuccess, onError);
        }
    }
}
