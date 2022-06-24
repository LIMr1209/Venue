using System.Collections.Generic;
using UnityEngine;
using System.IO;
using DefaultNamespace;
using Qiniu.Http;
using UnityEditor;

namespace Editor
{
    public class UploadAsset : MonoBehaviour
    {
        [MenuItem("Tools/上传ab包")]
        public static void UploadAb()
        {
            List<string> files = new List<string>();
            QiNiuHelp.ForeachFile(Path.Combine(Application.dataPath,Globle.AssetBundleDir), ref files); 
            foreach (string i  in files)
            {
                string[] splitName = i.Split('\\');
                string filename = splitName[splitName.Length-1];
                if (filename.StartsWith("scene"))
                {
                    continue;
                }

                // 上传文件名
                string replacePath = Application.dataPath.Replace("/", "\\") + "\\";
                string keySuffix = i.Replace(replacePath, "");
                string key = Path.Combine(Globle.QiNiuPrefix, Globle.AssetVision, keySuffix).Replace("\\", "/");
                // 加密
                byte[] fileBytes = Aes.FileToByte(i);
                byte[] encryptBytes = Aes.AESEncrypt(fileBytes, Globle.AesKey, Globle.AesIv);
                // HttpResult result = Upload(i, key);
                HttpResult result = QiNiuHelp.Upload(encryptBytes, key);
                if (result.Code != 200)
                {
                    Debug.Log("上传错误 文件: " + i + " 错误消息: "+result.Text);
                    return;
                }
            }

        }


        public static void OnUpLoadAB(string scenePrefix)
        {
            string path = "";
            QiNiuHelp.OnForeachFile(Path.Combine(Application.dataPath, Globle.AssetBundleDir), ref path);

            string[] splitName = path.Split('\\');
            string filename = splitName[splitName.Length - 1];

            // 上传文件名
            string replacePath = Application.dataPath.Replace("/", "\\") + "\\";

            string keySuffix = path.Replace(replacePath, "");
            string key = Path.Combine(scenePrefix, Globle.AssetVision, keySuffix).Replace("\\", "/");
            // 加密
            byte[] fileBytes = Aes.FileToByte(path);
            byte[] encryptBytes = Aes.AESEncrypt(fileBytes, Globle.AesKey, Globle.AesIv);
            // HttpResult result = Upload(i, key);
            HttpResult result = QiNiuHelp.Upload(encryptBytes, key);
            if (result.Code != 200)
            {
                Debug.Log("上传错误 文件: " + path + " 错误消息: " + result.Text);
                return;
            }

        }
























        [MenuItem("Tools/上传构建")]
        public static void UploadBuild()
        {
            string buildPath = @"C:\Users\thn\Desktop\VenueBuild";
            string suffixPath = @"C:\Users\thn\Desktop\";
            List<string> files = new List<string>();
            QiNiuHelp.ForeachFile(buildPath, ref files);
            foreach (string i in files)
            {
                // 上传文件名
                string keySuffix = i.Replace(suffixPath, "").Replace("\\", "/");
                string key = Path.Combine("unity", keySuffix).Replace("\\", "/");
                // 加密
                HttpResult result = QiNiuHelp.Upload(i, key, true);
                if (result.Code != 200)
                {
                    Debug.Log("上传错误 文件: " + i + " 错误消息: "+result.Text);
                    return;
                }
            }

            QiNiuHelp.RefreshDirs("unity/VenueBuild/");

        }

    }
}