using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine.Networking;

namespace Editor
{
    public class Aes
    {
        /// <summary>
        /// AES加密，任意文件
        /// </summary>
        /// <param name="Data">被加密的明文</param>
        /// <param name="Key">密钥</param>
        /// <param name="Iv">密钥向量</param>
        /// <returns>密文</returns>
        public static byte[] AESEncrypt(byte[] Data, string Key, string Iv)
        {
            byte[] bKey = new byte[32]; //采用32位密码加密
            Array.Copy(Encoding.UTF8.GetBytes(Key.PadRight(bKey.Length)), bKey,
                bKey.Length); //如果用户输入的key不足32位，自动填充空格至32位
            byte[] bIv = new byte[16]; //密钥向量，为16位
            Array.Copy(Encoding.UTF8.GetBytes(Iv.PadRight(bIv.Length)), bIv, bIv.Length); //如果用户定义的Iv不足16位，自动填充空格至16位
            byte[] Cryptograph = null; //加密后的密文
            Rijndael Aes = Rijndael.Create();
            try
            {
                using (MemoryStream Memory = new MemoryStream())
                {
                    //把内存流对象包装成加密流对象
                    using (CryptoStream Encryptor =
                        new CryptoStream(Memory, Aes.CreateEncryptor(bKey, bIv), CryptoStreamMode.Write))
                    {
                        Encryptor.Write(Data, 0, Data.Length);
                        Encryptor.FlushFinalBlock();
                        Cryptograph = Memory.ToArray();
                    }
                }
            }
            catch
            {
                Cryptograph = null;
            }

            return Cryptograph;
        }

        /// <summary>
        /// AES解密，任意文件
        /// </summary>
        /// <param name="Data">被解密的密文</param>
        /// <param name="Key">密钥</param>
        /// <param name="Iv">密钥向量</param>
        /// <returns>明文</returns>
        public static byte[] AESDecrypt(byte[] Data, string Key, string Iv)
        {
            byte[] bKey = new byte[32];
            Array.Copy(Encoding.UTF8.GetBytes(Key.PadRight(bKey.Length)), bKey, bKey.Length);
            byte[] bIv = new byte[16];
            Array.Copy(Encoding.UTF8.GetBytes(Iv.PadRight(bIv.Length)), bIv, bIv.Length);
            byte[] original = null; //解密后的明文
            Rijndael Aes = Rijndael.Create();
            try
            {
                using (MemoryStream Memory = new MemoryStream(Data))
                {
                    //把内存流对象包装成加密对象
                    using (CryptoStream Decryptor =
                        new CryptoStream(Memory, Aes.CreateDecryptor(bKey, bIv), CryptoStreamMode.Read))
                    {
                        //明文存储区
                        using (MemoryStream originalMemory = new MemoryStream())
                        {
                            byte[] Buffer = new byte[1024];
                            int readBytes = 0;
                            while ((readBytes = Decryptor.Read(Buffer, 0, Buffer.Length)) > 0)
                            {
                                originalMemory.Write(Buffer, 0, readBytes);
                            }

                            original = originalMemory.ToArray();
                        }
                    }
                }
            }
            catch
            {
                original = null;
            }

            return original;
        }

        /// <summary>
        /// 将文件转换成byte[]数组
        /// </summary>
        /// <param name="fileUrl">文件路径文件名称</param>
        /// <returns>byte[]数组</returns>
        public static byte[] FileToByte(string fileUrl)
        {
            try
            {
                using (FileStream fs = new FileStream(fileUrl, FileMode.Open, FileAccess.Read))
                {
                    byte[] byteArray = new byte[fs.Length];
                    fs.Read(byteArray, 0, byteArray.Length);
                    return byteArray;
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 将byte[]数组保存成文件
        /// </summary>
        /// <param name="byteArray">byte[]数组</param>
        /// <param name="fileName">保存至硬盘的文件路径</param>
        /// <returns></returns>
        public static bool ByteToFile(byte[] byteArray, string fileName)
        {
            bool result = false;
            try
            {
                using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    fs.Write(byteArray, 0, byteArray.Length);
                    result = true;
                }
            }
            catch
            {
                result = false;
            }

            return result;
        }
        /// <summary>
        /// 将下载存为 bytes
        /// </summary>
        /// <param name="url">url链接</param>
        /// <returns>byte[]数组</returns>
        public static byte[] DownloadToBytes(string url)
        {
            try
            {
                UnityWebRequest request = UnityWebRequest.Get(url); // 获取文本或者二进制数据
                request.SendWebRequest();
                while (!request.isDone)
                {
                    
                }
                {
                    if (request.result != UnityWebRequest.Result.Success)
                    {
                        return null;
                    }

                    byte[] results = request.downloadHandler.data;
                    return results;
                }
            }
            catch
            {
                return null;
            }
        }
    }
}