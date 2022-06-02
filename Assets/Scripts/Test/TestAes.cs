using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class TestAes : MonoBehaviour
    {
        
        // 测试gltf 解密
        private void Start()
        {
            // string path = Path.Combine(Application.dataPath, "edit_encrypt.gltf");
            // Byte[] originBytes = Aes.FileToByte(path);
            string url = "https://cdn1.d3ingo.com/model_scene/220601/6296f34e5a262d9c830ecb6b/edit_encrypt.gltf";
            Byte[] originBytes = Aes.DownloadToBytes(url);
            Byte[] newBytes = Aes.AESDecrypt(originBytes, Globle.AesKey, Globle.AesIv);
            
            
        }
    }
}