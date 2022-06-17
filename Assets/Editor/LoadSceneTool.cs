using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.IO;
using System;
using UnityEditor;
using UnityEngine.Networking;
using System.Net;

public class LoadSceneTool : MonoBehaviour
{
   

    [MenuItem("Tools/��ӻ���㼶")]
    public static void OnLayerH()
    {
        //Ѱ��Hierarchy��������е�MeshRenderer
        var tArray = Resources.FindObjectsOfTypeAll(typeof(MeshRenderer));
        for (int i = 0; i < tArray.Length; i++)
        {
            MeshRenderer t = tArray[i] as MeshRenderer;
            if (t.gameObject.name.Contains("art"))
            {
                t.gameObject.layer = 6;
            }
            Undo.RecordObject(t, t.gameObject.name);
            EditorUtility.SetDirty(t);
        }
        Debug.Log("Succed");
    }


    [MenuItem("Tools/���س�����Դ")]
    public static void OnDownLoadScene()
    {
        string url = @"https://s3.taihuoniao.com/unity/scene.fbx";
        string progress = null;
        Debug.Log(url);
        Debug.Log("��ʼ����ģ�͡�");
        WWW w = new WWW(url);
        while (!w.isDone)
        {
            progress = (((int)(w.progress * 100)) % 100) + "%";
        }
        if (w.isDone)
        {
            byte[] model = w.bytes;
            int length = model.Length;
            //�ļ�����Ϣ  
            Stream sw;
            DirectoryInfo t = new DirectoryInfo(Application.dataPath + "/AssetsPackages/Textures");
            if (!t.Exists)
            {
                //������ļ��в������򴴽�  
                t.Create();
            }
            FileInfo j = new FileInfo(Application.dataPath + "/AssetsPackages/Textures/scene.fbx");
            if (!j.Exists)
            {
                //������ļ��������򴴽�  
                sw = j.Create();
            }
            else
            {
                //������ļ��������  
                sw = j.OpenWrite();
            }
            sw.Write(model, 0, length);
            //�ر���  
            sw.Close();
            //������  
            sw.Dispose();
            Debug.Log("�������");
            UnityEditor.AssetDatabase.Refresh();
        }
    }

  

}


