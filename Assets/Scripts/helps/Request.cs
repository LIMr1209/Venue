using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace DefaultNamespace
{
    public class Request : MonoBehaviour

    {
        public static Request instances;
        
        private void Awake()
        {
            instances = this;
        }
        public delegate void ReqCallback(int statusCode, string error, string body);

        public static IEnumerator GetRequest(string url, string paramsStr, ReqCallback callback)
        {
            if (!string.IsNullOrEmpty(paramsStr))
            {
                url = string.Format("{0}?{1}", url, paramsStr);
            }
            UnityWebRequest request = UnityWebRequest.Get(url);
            yield return request.SendWebRequest();
            if (!string.IsNullOrEmpty(request.error))
            {
                Debug.LogError(request.error);
                yield break;
            }

            callback(Convert.ToInt32(request.responseCode), request.error, request.downloadHandler.text);
        }
        
        public static IEnumerator PostRequest(string url, WWWForm form, ReqCallback callback)
        {
            UnityWebRequest request = UnityWebRequest.Post(url, form);
            yield return request.SendWebRequest();
            if (!string.IsNullOrEmpty(request.error))
            {
                Debug.LogError(request.error);
                yield break;
            }

            callback(Convert.ToInt32(request.responseCode), request.error, request.downloadHandler.text);
        }

        public void HttpSend(int urlId, string method, Dictionary<string, string> requestData, ReqCallback callback)
        {
            WWWForm form = new WWWForm();
            string paramsStr = null;
            if (requestData == null || requestData.Count == 0)
                requestData = null;
            else
            {
                StringBuilder stringBuilder = new StringBuilder();
                bool isFirst = true;
                foreach (var item in requestData)
                {
                    if (isFirst)
                        isFirst = false;
                    else
                        stringBuilder.Append('&');
                    stringBuilder.Append(item.Key);
                    stringBuilder.Append('=');
                    stringBuilder.Append(item.Value);
                    form.AddField(item.Key, item.Value);
                }
                paramsStr = stringBuilder.ToString();
            }
            string url = null;
            switch (urlId)
            {
                case 1:
                {
                    url = Path.Combine(Globle.ServiceHost, Globle.userInfoRoute);
                    break;
                }
                case 2:
                {
                    break;
                }
                default:
                {
                    url = "";
                    break;
                }
            }

            if (string.IsNullOrEmpty(url))
            {
                return;
            }

            url = url.Replace("\\", "/");
            if (method == "get")
            {
                StartCoroutine(GetRequest(url, paramsStr, callback));
            }
            else if(method == "post")
            {
                StartCoroutine(PostRequest(url, form, callback));
            }
        }
    }
}