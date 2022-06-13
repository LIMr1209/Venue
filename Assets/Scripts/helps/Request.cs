using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace DefaultNamespace
{
    public class Request
    {

        private static UnityWebRequest HttpGet(string url)
        {
            UnityWebRequest request = UnityWebRequest.Get(url); // 获取文本或者二进制数据
            request.SendWebRequest();
            while (!request.isDone)
            {
                
            }
            return request;
        }


        public static JsonData.UserResult GetUserInfo()
        {
            string url = Path.Combine(Globle.ServiceHost, Globle.userInfoRoute) +
                         "?token=sHVfLmgZvKMz1gDJuBQnhEcHgj7GopxXlOFcvy41Fng4RNnTecsaMqVLfCMU";
            UnityWebRequest request = HttpGet(url);
            string text = request.downloadHandler.text;
            JsonData.UserResult result = new JsonData.UserResult();
            result = JsonUtility.FromJson<JsonData.UserResult>(text); // ToJson
            return result;
        }
    }
}