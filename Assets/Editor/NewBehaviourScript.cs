using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.IO;
using System;
using UnityEditor;
using UnityEngine.Networking;
using System.Net;

public class NewBehaviourScript : MonoBehaviour
{
    [MenuItem("Tools/添加场景预设体")]
    public static void OnAddFBXModelTOModels()
    {
        string DestPath = Application.dataPath + "/AssetsPackages/Models";
        MoveFolder("E:/UnityPart/Work/AAA", DestPath);
    }
    public static void MoveFolder(string sourcePath, string destPath)
    {
        if (Directory.Exists(sourcePath))
        {
            if (!Directory.Exists(destPath))
            {
                //目标目录不存在则创建 
                try
                {
                    Directory.CreateDirectory(destPath);
                }
                catch (Exception ex)
                {
                    throw new Exception("创建目标目录失败：" + ex.Message);
                }
            }
            //获得源文件下所有文件 
            List<string> files = new List<string>(Directory.GetFiles(sourcePath));
            files.ForEach(c =>
            {
                string destFile = Path.Combine(new string[] { destPath, Path.GetFileName(c) });
                //覆盖模式 
                if (File.Exists(destFile))
                {
                    File.Delete(destFile);
                }
                File.Move(c, destFile);
            });
            //获得源文件下所有目录文件 
            List<string> folders = new List<string>(Directory.GetDirectories(sourcePath));

            folders.ForEach(c =>
            {
                string destDir = Path.Combine(new string[] { destPath, Path.GetFileName(c) });
                //Directory.Move必须要在同一个根目录下移动才有效，不能在不同卷中移动。 
                //Directory.Move(c, destDir); 
                Debug.Log(111);
                //采用递归的方法实现 
                MoveFolder(c, destDir);
            });
        }
        else
        {



        }
    }

    [MenuItem("Tools/添加画板层级")]
    public static void OnLayerH()
    {
        //寻找Hierarchy面板下所有的MeshRenderer
        var tArray = Resources.FindObjectsOfTypeAll(typeof(MeshRenderer));
        for (int i = 0; i < tArray.Length; i++)
        {
            MeshRenderer t = tArray[i] as MeshRenderer;
            //这个很重要，博主发现如果没有这个代码，unity是不会察觉到编辑器有改动的，自然设置完后直接切换场景改变是不被保存
            //的  如果不加这个代码  在做完更改后 自己随便手动修改下场景里物体的状态 在保存就好了 
            
            Debug.Log(t.gameObject.name.Contains("Box") + "   " + t.name);
            if (t.gameObject.name.Contains("art"))
            {
                t.gameObject.layer = 6;
            }
            Undo.RecordObject(t, t.gameObject.name);
            //相当于让他刷新下 不然unity显示界面还不知道自己的东西被换掉了  还会呆呆的显示之前的东西
            EditorUtility.SetDirty(t);
        }
        Debug.Log("Succed");
    }


    [MenuItem("Tools/下载场景资源")]
    public static void OnDownLoadScene()
    {
        string Path = @"https://s3.taihuoniao.com/unity/scene.fbx";
        //UnityWebRequest requestAB = UnityWebRequest.Get(Path);
        //while(!requestAB.isDone)
        //{
        //    Debug.Log(requestAB.downloadHandler.data);
        //}
        //if (requestAB.isDone)
        //{
        //    byte[] results = requestAB.downloadHandler.data;
        //    string savePath = Application.dataPath + "/AssetsPackages/Textures";

        //    if (!Directory.Exists(savePath))
        //    {
        //        Directory.CreateDirectory(savePath);
        //    }
        //    //FileInfo fileInfo = new FileInfo(savePath + "/" + "scene");
        //    //FileStream fs = fileInfo.Create();
        //    ////fs.Write(字节数组, 开始位置, 数据长度);
        //    //fs.Write(results, 0, results.Length);
        //    //fs.Flush(); //文件写入存储到硬盘
        //    //fs.Close(); //关闭文件流对象
        //    //fs.Dispose(); //销毁文件对象
        //    File.WriteAllBytes(savePath, results);
        //}
        Download(Path, Application.dataPath + "/AssetsPackages/Textures");
    }

    public static void Download(string url, string localfile)
    {
        long startPosition = 0; // 上次下载的文件起始位置
        FileStream writeStream; // 写入本地文件流对象
        // 判断要下载的文件夹是否存在
        if (File.Exists(localfile))
        {
            writeStream = File.OpenWrite(localfile);             // 存在则打开要下载的文件
            startPosition = writeStream.Length;                  // 获取已经下载的长度
            writeStream.Seek(startPosition, SeekOrigin.Current); // 本地文件写入位置定位
        }
        else
        {
            writeStream = new FileStream(localfile, FileMode.Create);// 文件不保存创建一个文件
            startPosition = 0;
        }
        try
        {
            HttpWebRequest myRequest = (HttpWebRequest)HttpWebRequest.Create(url);// 打开网络连接
            if (startPosition > 0)
            {
                myRequest.AddRange((int)startPosition);// 设置Range值,与上面的writeStream.Seek用意相同,是为了定义远程文件读取位置
            }
            Stream readStream = myRequest.GetResponse().GetResponseStream();// 向服务器请求,获得服务器的回应数据流
            byte[] btArray = new byte[512];// 定义一个字节数据,用来向readStream读取内容和向writeStream写入内容
            int contentSize = readStream.Read(btArray, 0, btArray.Length);// 向远程文件读第一次
            while (contentSize > 0)// 如果读取长度大于零则继续读
            {
                writeStream.Write(btArray, 0, contentSize);// 写入本地文件
                contentSize = readStream.Read(btArray, 0, btArray.Length);// 继续向远程文件读取
            }
            //关闭流
            writeStream.Close();
            readStream.Close();     //返回true下载成功
        }
        catch (Exception e)
        {
            writeStream.Close();     //返回false下载失败
        }

    }

}


