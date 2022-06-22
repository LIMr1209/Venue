using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.IO;
using System;
using UnityEditor;
using UnityEngine.Networking;
using System.Net;
using Editor;

public class LoadSceneTool : MonoBehaviour
{
   

    [MenuItem("Tools/添加画板层级")]
    public static void OnLayerH()
    {
        //寻找Hierarchy面板下所有的MeshRenderer
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


    [MenuItem("Tools/下载场景资源")]
    public static void OnDownLoadScene()
    {
        string url = @"https://s3.taihuoniao.com/unity/scene.fbx";
        string progress = null;
        Debug.Log(url);
        Debug.Log("开始下载模型。");
        WWW w = new WWW(url);
        while (!w.isDone)
        {
            progress = (((int)(w.progress * 100)) % 100) + "%";
        }
        if (w.isDone)
        {
            byte[] model = w.bytes;
            int length = model.Length;
            //文件流信息  
            Stream sw;
            DirectoryInfo t = new DirectoryInfo(Application.dataPath + "/Resources");
            if (!t.Exists)
            {
                //如果此文件夹不存在则创建  
                t.Create();
            }
            FileInfo j = new FileInfo(Application.dataPath + "/Resources/scene.fbx");
            if (!j.Exists)
            {
                //如果此文件不存在则创建  
                sw = j.Create();
            }
            else
            {
                //如果此文件存在则打开  
                sw = j.OpenWrite();
            }
            sw.Write(model, 0, length);
            //关闭流  
            sw.Close();
            //销毁流  
            sw.Dispose();
            Debug.Log("下载完成");
            UnityEditor.AssetDatabase.Refresh();
            OnAddSceneModel();
        }
    }


    [MenuItem("Tools/添加场景")]
    public static void OnAddSceneModel()
    {
        GameObject obj = Resources.Load("scene") as GameObject;
        Instantiate(obj);
        AddMeshCollider.Add();
        OnLayerH();
        string targetPath = Application.dataPath + "/AssetsPackages/prefabs/OtherPrefabs/" + obj.name;
        GameObject modelGame = PrefabUtility.InstantiatePrefab(obj) as GameObject;
        PrefabUtility.SaveAsPrefabAsset(modelGame, targetPath+".prefab");
        OnDestoryObj();
        OnSetAssetBundelName();
    }

    private static void OnSetAssetBundelName()
    {
        Debug.Log(Directory.Exists(Application.dataPath + "/AssetsPackages/prefabs/OtherPrefabs/"));
        if (Directory.Exists(Application.dataPath + "/AssetsPackages/prefabs/OtherPrefabs/"))
        {
            DirectoryInfo direction = new DirectoryInfo(Application.dataPath + "/AssetsPackages/prefabs/OtherPrefabs/");
            FileInfo[] files = direction.GetFiles("*");
            for (int i = 0; i < files.Length; i++)
            {
                //去除Unity内部.meta文件
                if (files[i].Name.EndsWith(".meta"))
                    continue;
                string path = "Assets/AssetsPackages/prefabs/OtherPrefabs/" + files[i].Name;
                GameObject prefab = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;
                string a_path = AssetDatabase.GetAssetPath(prefab);
                Debug.Log(a_path);
                AssetImporter asset = AssetImporter.GetAtPath(a_path);
                asset.assetBundleName = "scene";
                asset.assetBundleVariant = "ab";
                asset.SaveAndReimport();
            }
        }
        CreateAssetBundles.BuildAllAssetBundlesLocal();
        UploadAsset.Upload();
    }

    public static void OnDestoryObj()
    {
        GameObject[] tArray = (GameObject[])Resources.FindObjectsOfTypeAll(typeof(GameObject));
        for (int i = 0; i < tArray.Length; i++)
        {
            if (tArray[i].name.Contains("scene"))
            {
                DestroyImmediate(tArray[i]);
            }
        }
    }
  

}


