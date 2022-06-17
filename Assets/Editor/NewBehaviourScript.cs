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
    [MenuItem("Tools/��ӳ���Ԥ����")]
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
                //Ŀ��Ŀ¼�������򴴽� 
                try
                {
                    Directory.CreateDirectory(destPath);
                }
                catch (Exception ex)
                {
                    throw new Exception("����Ŀ��Ŀ¼ʧ�ܣ�" + ex.Message);
                }
            }
            //���Դ�ļ��������ļ� 
            List<string> files = new List<string>(Directory.GetFiles(sourcePath));
            files.ForEach(c =>
            {
                string destFile = Path.Combine(new string[] { destPath, Path.GetFileName(c) });
                //����ģʽ 
                if (File.Exists(destFile))
                {
                    File.Delete(destFile);
                }
                File.Move(c, destFile);
            });
            //���Դ�ļ�������Ŀ¼�ļ� 
            List<string> folders = new List<string>(Directory.GetDirectories(sourcePath));

            folders.ForEach(c =>
            {
                string destDir = Path.Combine(new string[] { destPath, Path.GetFileName(c) });
                //Directory.Move����Ҫ��ͬһ����Ŀ¼���ƶ�����Ч�������ڲ�ͬ�����ƶ��� 
                //Directory.Move(c, destDir); 
                Debug.Log(111);
                //���õݹ�ķ���ʵ�� 
                MoveFolder(c, destDir);
            });
        }
        else
        {



        }
    }

    [MenuItem("Tools/��ӻ���㼶")]
    public static void OnLayerH()
    {
        //Ѱ��Hierarchy��������е�MeshRenderer
        var tArray = Resources.FindObjectsOfTypeAll(typeof(MeshRenderer));
        for (int i = 0; i < tArray.Length; i++)
        {
            MeshRenderer t = tArray[i] as MeshRenderer;
            //�������Ҫ�������������û��������룬unity�ǲ��������༭���иĶ��ģ���Ȼ�������ֱ���л������ı��ǲ�������
            //��  ��������������  ��������ĺ� �Լ�����ֶ��޸��³����������״̬ �ڱ���ͺ��� 
            
            Debug.Log(t.gameObject.name.Contains("Box") + "   " + t.name);
            if (t.gameObject.name.Contains("art"))
            {
                t.gameObject.layer = 6;
            }
            Undo.RecordObject(t, t.gameObject.name);
            //�൱������ˢ���� ��Ȼunity��ʾ���滹��֪���Լ��Ķ�����������  �����������ʾ֮ǰ�Ķ���
            EditorUtility.SetDirty(t);
        }
        Debug.Log("Succed");
    }


    [MenuItem("Tools/���س�����Դ")]
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
        //    ////fs.Write(�ֽ�����, ��ʼλ��, ���ݳ���);
        //    //fs.Write(results, 0, results.Length);
        //    //fs.Flush(); //�ļ�д��洢��Ӳ��
        //    //fs.Close(); //�ر��ļ�������
        //    //fs.Dispose(); //�����ļ�����
        //    File.WriteAllBytes(savePath, results);
        //}
        Download(Path, Application.dataPath + "/AssetsPackages/Textures");
    }

    public static void Download(string url, string localfile)
    {
        long startPosition = 0; // �ϴ����ص��ļ���ʼλ��
        FileStream writeStream; // д�뱾���ļ�������
        // �ж�Ҫ���ص��ļ����Ƿ����
        if (File.Exists(localfile))
        {
            writeStream = File.OpenWrite(localfile);             // �������Ҫ���ص��ļ�
            startPosition = writeStream.Length;                  // ��ȡ�Ѿ����صĳ���
            writeStream.Seek(startPosition, SeekOrigin.Current); // �����ļ�д��λ�ö�λ
        }
        else
        {
            writeStream = new FileStream(localfile, FileMode.Create);// �ļ������洴��һ���ļ�
            startPosition = 0;
        }
        try
        {
            HttpWebRequest myRequest = (HttpWebRequest)HttpWebRequest.Create(url);// ����������
            if (startPosition > 0)
            {
                myRequest.AddRange((int)startPosition);// ����Rangeֵ,�������writeStream.Seek������ͬ,��Ϊ�˶���Զ���ļ���ȡλ��
            }
            Stream readStream = myRequest.GetResponse().GetResponseStream();// �����������,��÷������Ļ�Ӧ������
            byte[] btArray = new byte[512];// ����һ���ֽ�����,������readStream��ȡ���ݺ���writeStreamд������
            int contentSize = readStream.Read(btArray, 0, btArray.Length);// ��Զ���ļ�����һ��
            while (contentSize > 0)// �����ȡ���ȴ������������
            {
                writeStream.Write(btArray, 0, contentSize);// д�뱾���ļ�
                contentSize = readStream.Read(btArray, 0, btArray.Length);// ������Զ���ļ���ȡ
            }
            //�ر���
            writeStream.Close();
            readStream.Close();     //����true���سɹ�
        }
        catch (Exception e)
        {
            writeStream.Close();     //����false����ʧ��
        }

    }

}


