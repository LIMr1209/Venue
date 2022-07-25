using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace DefaultNamespace
{
    public class Globle : MonoBehaviour
    {
        // public static Globle instances;
        // 全局变量
        public static string AssetHost = "https://s3.taihuoniao.com"; // 资源文件 域名 frstatic bucket空间
        public static string FrfileHost = "https://cdn1.d3ingo.com"; //  资源文件 frfile bucket 域名
        public static string SceneBucket = "frfile"; // 场景ab包 上传空间名
        public static string AbBucket = "frstatic"; // 项目ab包 上传空间名
        public static string ServiceHost = "http://render-dev.d3ingo.com"; // 后端业务 域名

        public static string QiNiuPrefix = "unity/venue/pro"; // 七牛资源前缀

        public static string AssetVision = "2022072501"; // 资源版本

        public static string QiNiuAccessKey = "ERh7qjVSy0v42bQ0fftrFeKYZG39XbzRlaJO4NFy"; //七牛 AccessKey
        public static string QiNiuSecretKey = "r-NUrKsnRBEwTQxbLONVrK9tPuncXyHmcq4BkSc7"; //七牛 QiNiuSecretKey
        public static string AesIv = "1234567890123456"; // aes 解密 iv
        public static string AesKey = "12345678901234561234567890123456"; // aes 解密 key
        public static string AssetBundleDir = "AssetsBundles"; // assetBundle 一级目录

        // 后端url
        public static string userInfoRoute = "api/user/get_user"; // 用户详情 urlId  1
        public static string projectViewRoute = "api/user_project/view"; // 项目详情接口  urlId 2
        public static string worksListRoute = "api/room/works_list"; // 作品列表  urlId 3
        public static string roomViewRoute = "api/room/view"; // 房间详情  urlId 4
        public static string roomMemberListRoute = "api/room_member/list"; // 房间成员 urlId 5
        public static string sceneViewRoute = "api/scene_model/info";  // 场景详情 urlId 6
        
        // scene 图层
        public static string focusArtLayer = "art"; // 聚焦层画
        public static string lockArtlayer = "artLock"; // 锁定层画
        public static string groundLayer = "ground"; // 寻路地板层
        public static string wallLayer = "wall"; // 寻路地板层
        public static string playerLayer = "player"; // 玩家

        public static Dictionary<string, Dictionary<string, string>> dic =
            new Dictionary<string, Dictionary<string, string>>();

        // private void Awake()
        // {
        //     instances = this;
        //     StartCoroutine(ReadConfig());
        // }

        public IEnumerator ReadConfig()
        {
            
            string[] lines = null;
#if !UNITY_EDITOR && UNITY_WEBGL
            UnityWebRequest request = UnityWebRequest.Get("https://s3.taihuoniao.com/unity/venue/pro/VenueBuild/streamingAssetsPath/config.txt"); //URL 是需要获取的网址地址
            yield return request.SendWebRequest();
            string data = request.downloadHandler.text;
            lines = data.Split(Environment.NewLine.ToCharArray());
#else
            string configPath = Path.Combine(Application.streamingAssetsPath, "config.txt").Replace("\\","/");
            lines = File.ReadAllLines(configPath);
#endif            
            LoadConfig(lines);
            yield return null;
        }
        
        static void LoadConfig(string[] lines)
        {
            BuildDic(lines);
            QiNiuPrefix = dic["Asset"]["QiNiuPrefix"];
            AssetVision = dic["Asset"]["Vision"];
        }
        
        static void BuildDic(string[] lines)
        {
            string mainKey = null;
            string subKey = null;
            string subValue = null;
            foreach (var item in lines)
            {
                string line = null;
                line = item.Trim();
                if (!string.IsNullOrEmpty(line))
                {
                    if (line.StartsWith("["))
                    {
                        mainKey = line.Substring(1, line.IndexOf("]") - 1);
                        dic.Add(mainKey, new Dictionary<string, string>());
                    }
                    else
                    {
                        var configValue = line.Split('=', StringSplitOptions.RemoveEmptyEntries);
                        subKey = configValue[0].Trim();
                        subValue = configValue[1].Trim();
                        subValue = subValue.StartsWith(@"\") ? subValue.Substring(1) : subValue;
                        dic[mainKey].Add(subKey, subValue);
                    }
                }
            }
        }
    } 
}