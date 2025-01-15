using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Novaflo.Common
{
    public class HttpClient : Singleton<HttpClient>
    {
        /// <summary>
        /// Sends an HTTP request.
        /// </summary>
        /// <param name="url">The URL to request.</param>
        /// <param name="method">The HTTP method (GET, POST, PUT, DELETE).</param>
        /// <param name="headers">Optional headers as key-value pairs.</param>
        /// <param name="body">Optional body data as a JSON string.</param>
        /// <param name="onSuccess">Callback for a successful response.</param>
        /// <param name="onError">Callback for an error response.</param>
        public void SendRequest(string url, string method, Dictionary<string, string> headers, string body, Action<string> onSuccess, Action<string> onError)
        {
            StartCoroutine(SendRequestCoroutine(url, method, headers, body, onSuccess, onError));
        }

        private IEnumerator SendRequestCoroutine(string url, string method, Dictionary<string, string> headers, string body, Action<string> onSuccess, Action<string> onError)
        {
            Debug.Log("url : " + url);
            Debug.Log("method : " + method);
            Debug.Log("Request : " + body);
            
            using (UnityWebRequest request = new UnityWebRequest(url, method))
            {
                if (!string.IsNullOrEmpty(body))
                {
                    byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(body);
                    request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                }

                request.downloadHandler = new DownloadHandlerBuffer();

                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        request.SetRequestHeader(header.Key, header.Value);
                    }
                }

                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    onSuccess?.Invoke(request.downloadHandler.text);
                }
                else
                {
                    onError?.Invoke(request.error);
                }
            }
        }
    }
}
