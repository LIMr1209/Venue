using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace DefaultNamespace.UI
{
    public class Tools : MonoBehaviour
    {
        public static AssetBundle GetAssetBundle(string filepath)
        {
#if UNITY_EDITOR
            string path = Path.Combine(Application.dataPath, Globle.AssetBundleDir, filepath);
            string uri="file:///"+path;
#else
            string url = Path.Combine(Globle.AssetPrefix, filepath).Replace("\\", "");
#endif
            UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(path); // 获取assetBundle
            request.SendWebRequest();
            while (!request.isDone)
            {
                
            }
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.error);
            }
            AssetBundle assetBundle = DownloadHandlerAssetBundle.GetContent(request);
            return assetBundle;
        }
    }
}