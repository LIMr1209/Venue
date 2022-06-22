using UnityEngine;

namespace DefaultNamespace
{
    public class Globle : MonoBehaviour
    {
        // 全局变量
        public static string AssetHost = @"https://s3.taihuoniao.com"; // 资源文件 域名
        public static string ServiceHost = "http://render-dev.d3ingo.com"; // 后端业务 域名

        public static string QiNiuPrefix = "unity/venue"; // 七牛资源前缀
        public static string AssetVision = "2022062201"; // 资源版本
        public static string QiNiuBucket = "frstatic"; // 七牛Bucket
        public static string AssetPrefix = AssetHost + QiNiuPrefix + AssetVision; // 资源请求前缀
        public static string QiNiuAccessKey = "ERh7qjVSy0v42bQ0fftrFeKYZG39XbzRlaJO4NFy"; //七牛 AccessKey
        public static string QiNiuSecretKey = "r-NUrKsnRBEwTQxbLONVrK9tPuncXyHmcq4BkSc7"; //七牛 QiNiuSecretKey
        public static string AesIv = "1234567890123456"; // aes 解密 iv
        public static string AesKey = "12345678901234561234567890123456"; // aes 解密 key
        public static string tokenName = "dev_token";
        public static string AssetBundleDir = "AssetsBundles"; // assetBundle 一级目录

        // 后端url
        public static string userInfoRoute = "api/user/get_user"; // 用户详情 urlId  1
        public static string projectViewRoute = "api/user_project/view"; // 项目详情接口  urlId 2
        public static string worksListRoute = "api/room/works_list"; // 作品列表  urlId 3
        public static string roomViewRoute = "api/room/view"; // 房间详情  urlId 4
        public static string roomMemberListRoute = "api/room_member/list"; // 房间成员 urlId 5
        public static string sceneViewRoute = "api/scene_model/info";  // 场景详情 urlId 6

        public static string sceneId;
    }
}