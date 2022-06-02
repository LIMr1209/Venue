using System.Collections.Generic;
using UnityEngine;
using System.IO;
using DefaultNamespace;
using Qiniu.Http;
using Qiniu.Storage;
using UnityEditor;
using Qiniu.Util;
using FileInfo = System.IO.FileInfo;

namespace Editor
{
    public class UploadAsset : MonoBehaviour
    {
        [MenuItem("Tools/上传七牛")]
        public static void Upload()
        {

            UploadApplication();
            // EditorCoroutine.start(UploadApplication());

        }

        public static void  UploadApplication()
        {
            List<string> files = new List<string>();
            ForeachFile(Path.Combine(Application.dataPath,Globle.AssetBundleDir), ref files); 
            foreach (string i  in files)
            {
                HttpResult result = UploadFile(i);
                if (result.Code != 200)
                {
                    Debug.Log("上传错误 文件: " + i + " 错误消息: "+result.Text);
                    return;
                }
            }
        }

        public static void ForeachFile(string filePathByForeach,ref List<string> files)

        {
            DirectoryInfo theFolder = new DirectoryInfo(filePathByForeach);
            DirectoryInfo[] dirInfo = theFolder.GetDirectories();//获取所在目录的文件夹
            FileInfo[] file=  theFolder.GetFiles();//获取所在目录的文件
     
            foreach (FileInfo fileItem in file) //遍历文件
            {
                string result = Path.Combine(fileItem.DirectoryName, fileItem.Name);
                files.Add(result);
            }
            //遍历文件夹
            foreach (DirectoryInfo NextFolder in dirInfo)
            {
                ForeachFile(NextFolder.FullName, ref  files);
            }
        }

        public static bool FilterFile(FileInfo fileItem)
        {
            if(fileItem.Name.EndsWith(".meta"))
            {
                return true;
            }
            return false;
        }

        public static HttpResult UploadFile(string LocalPath)
        {
            Mac mac = new Mac(Globle.QiNiuAccessKey, Globle.QiNiuSecretKey);
            // 上传文件名
            string replacePath = Application.dataPath.Replace("/", "\\") + "\\";
            string keySuffix = LocalPath.Replace(replacePath, "");
            string key = Path.Combine(Globle.QiNiuPrefix, Globle.AssetVision, keySuffix).Replace("\\", "/");
            // 存储空间名
            string Bucket = Globle.QiNiuBucket;
            // 设置上传策略
            PutPolicy putPolicy = new PutPolicy();
            // 设置要上传的目标空间
            putPolicy.Scope = Bucket;
            // 上传策略的过期时间(单位:秒)
            putPolicy.SetExpires(3600);
            // 生成上传token
            string token = Auth.CreateUploadToken(mac, putPolicy.ToJsonString());
            Config config = new Config();
            // 设置上传区域
            config.Zone = Zone.ZONE_CN_East;
            // 设置 http 或者 https 上传
            config.UseHttps = true;
            config.UseCdnDomains = true;
            config.ChunkSize = ChunkUnit.U512K;
            // 表单上传
            FormUploader target = new FormUploader(config);
            HttpResult result = target.UploadFile(LocalPath, key, token, null);
            return result;
        }
    }
}