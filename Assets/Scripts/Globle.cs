using UnityEngine;

namespace DefaultNamespace
{
    public class Globle : MonoBehaviour
    {
        // 全局变量
        public static string AssetBundleDir = "AssetBundles";
        public static string FirstAssetBundle = "firstcontroller.plugin"; // 第一人称 assetBundle 名称
        public static string ThirdAssetBundle = "thirdcontroller.plugin"; // 第三人称 assetBundle 名称
        public static string AssetHost = "https://cdn1.d3ingo.com/venue"; // 资源文件 域名
        public static string ServiceHost = "http://render-dev.d3ingo.com/venue"; // 后端业务 域名
    }
}