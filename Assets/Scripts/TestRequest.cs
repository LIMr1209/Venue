using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


namespace DefaultNamespace
{
    public class TestRequest : MonoBehaviour
    {
        private void Start()
        {
            StartCoroutine(GetText());  //协程 获取
            // StartCoroutine(Upload());  //协程 post
            // StartCoroutine(Put());  //协程 put
        }

        IEnumerator  GetText()
        {
            UnityWebRequest request = UnityWebRequest.Get("http://render-dev.d3ingo.com/api/element/list"); // 获取文本或者二进制数据
            // UnityWebRequest request = UnityWebRequestTexture.GetTexture("https://p4.taihuoniao.com/scene_rendering/model_scene/220524/628c4a19f5ccb1a5b5549a28"); // 获取纹理
            // UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle("https://www.my-server.com/myData.unity3d"); // 获取assetBundle
            yield return request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.error);
            }
            else
            {
                // 以文本形式显示结果
                string text = request.downloadHandler.text;
                Debug.Log(text);
                // JsonInfo jsonInfo = JsonMapper.ToObject<JsonInfo>(text);
 
                // 或者获取二进制数据形式的结果
                // byte[] results = request.downloadHandler.data;
                
                // 纹理
                // Texture myTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;
                // Texture myTexture = DownloadHandlerTexture.GetContent(request);
                // Debug.Log(myTexture);
                
                // AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(request);
                // Debug.Log(bundle);
            }

        }
        IEnumerator Upload()
        {
            // IMultipartFormSection[] formData = new IMultipartFormSection[] { };
            // formData.Append(new MultipartFormDataSection("field1=foo&field2=bar"));
            // formData.Append(new MultipartFormFileSection("my file data", "myfile.txt"));

            List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
            formData.Add(new MultipartFormDataSection("field1=foo&field2=bar"));
            formData.Add(new MultipartFormFileSection("my file data", "myfile.txt"));

            UnityWebRequest request = UnityWebRequest.Post("https://www.my-server.com/myform", formData);
            
            //旧版
            // WWWForm form = new WWWForm();
            // form.AddField("myField", "myData");
     
            // UnityWebRequest request = UnityWebRequest.Post("https://www.my-server.com/myform", form);
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
                Debug.Log(request.downloadHandler.text);
            }
        }
        IEnumerator Put() {
            byte[] myData = System.Text.Encoding.UTF8.GetBytes("This is some test data");
            UnityWebRequest request = UnityWebRequest.Put("https://www.my-server.com/upload", myData);
            yield return request.SendWebRequest();
 
            if (request.result != UnityWebRequest.Result.Success) {
                Debug.Log(request.error);
            }
            else {
                Debug.Log("Upload complete!");
            }
        }
    }
}