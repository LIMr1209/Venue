using System.Collections.Generic;
using System.IO;
using DefaultNamespace;
using Qiniu.CDN;
using Qiniu.Http;
using Qiniu.Storage;
using Qiniu.Util;
using UnityEngine;
using FileInfo = System.IO.FileInfo;

namespace Editor
{
    public class QiNiuHelp
    {
        public static void ForeachFile(string filePathByForeach, ref List<string> files)

        {
            DirectoryInfo theFolder = new DirectoryInfo(filePathByForeach);
            DirectoryInfo[] dirInfo = theFolder.GetDirectories(); //获取所在目录的文件夹
            FileInfo[] file = theFolder.GetFiles(); //获取所在目录的文件

            foreach (FileInfo fileItem in file) //遍历文件
            {
                string result = Path.Combine(fileItem.DirectoryName, fileItem.Name);
                files.Add(result);
            }

            //遍历文件夹
            foreach (DirectoryInfo NextFolder in dirInfo)
            {
                ForeachFile(NextFolder.FullName, ref files);
            }
        }

        public static Config GetConfig()
        {
            Config config = new Config();
            // 设置上传区域
            config.Zone = Zone.ZONE_CN_East;
            // 设置 http 或者 https 上传
            config.UseHttps = true;
            config.UseCdnDomains = true;
            config.ChunkSize = ChunkUnit.U512K;
            return config;
        }

        public static string GetToken(bool overwrite = false, string key = "")
        {
            Mac mac = new Mac(Globle.QiNiuAccessKey, Globle.QiNiuSecretKey);
            // 存储空间名
            string Bucket = Globle.QiNiuBucket;
            // 设置上传策略
            PutPolicy putPolicy = new PutPolicy();
            // 设置要上传的目标空间
            if (overwrite)
            {
                putPolicy.Scope = Bucket + ":" + key;
            }
            else
            {
                putPolicy.Scope = Bucket;
            }

            // 上传策略的过期时间(单位:秒)
            putPolicy.SetExpires(3600);
            // 生成上传token
            string token = Auth.CreateUploadToken(mac, putPolicy.ToJsonString());
            Debug.Log(token);
            return token;
        }

        public static HttpResult Upload(byte[] fileBytes, string key, bool overwrite = false)
        {
            string token = GetToken(overwrite, key);
            Config config = GetConfig();
            FormUploader target = new FormUploader(config);
            HttpResult result = target.UploadData(fileBytes, key, token, null);
            return result;
        }

        public static HttpResult Upload(string localPath, string key, bool overwrite = false)
        {
            string token = GetToken(overwrite, key);
            Config config = GetConfig();
            FormUploader target = new FormUploader(config);
            HttpResult result = target.UploadFile(localPath, key, token, null);
            return result;
        }

        // 刷新目录
        public static void RefreshDirs(string dir)
        {
            Mac mac = new Mac(Globle.QiNiuAccessKey, Globle.QiNiuSecretKey);
            CdnManager manager = new CdnManager(mac);
            if (string.IsNullOrEmpty(dir))
            {
                Debug.Log("目录不能为空");
            }

            string[] dirs = new[]
            {
                Path.Combine(Globle.AssetHost, dir).Replace("\\","/")
            };
            RefreshResult ret = manager.RefreshDirs(dirs);
        }
    }
}