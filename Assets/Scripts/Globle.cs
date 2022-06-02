using UnityEngine;

namespace DefaultNamespace
{
    public class Globle : MonoBehaviour
    {
        // 全局变量
        public static string AssetHost = "https://s3.taihuoniao.com/venue"; // 资源文件 域名
        public static string ServiceHost = "http://render-dev.d3ingo.com/venue"; // 后端业务 域名

        public static string QiNiuPrefix = "unity/venue"; // 七牛资源前缀
        public static string AssetVision = "2022060101";  // 资源版本
        public static string QiNiuBucket = "frstatic"; // 七牛Bucket
        public static string AssetPrefix = AssetHost+QiNiuPrefix+AssetVision;  // 资源请求前缀
        public static string QiNiuAccessKey = "ERh7qjVSy0v42bQ0fftrFeKYZG39XbzRlaJO4NFy";  //七牛 AccessKey
        public static string QiNiuSecretKey = "r-NUrKsnRBEwTQxbLONVrK9tPuncXyHmcq4BkSc7"; //七牛 QiNiuSecretKey


        public static string AssetBundleDir = "AssetsBundle";  // assetBundle 一级目录
        public static string FirstAssetBundle = "firstcontroller.plugin"; // 第一人称 assetBundle 名称
        public static string ThirdAssetBundle = "thirdcontroller.plugin"; // 第三人称 assetBundle 名称
        
    }
}