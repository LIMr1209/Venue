using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using Editor;
using static DefaultNamespace.JsonData;
using DefaultNamespace;

public class LoadSceneTool : MonoBehaviour
{
    [MenuItem("Tools/��ӻ���")]
    public static void OnLayerH()
    {
        //Ѱ��Hierarchy��������е�MeshRenderer
        var tArray = Resources.FindObjectsOfTypeAll(typeof(MeshRenderer));
        for (int i = 0; i < tArray.Length; i++)
        {
            MeshRenderer t = tArray[i] as MeshRenderer;
            if (t.gameObject.name.Contains("paintings"))
            {
                t.gameObject.layer = LayerMask.NameToLayer(Globle.focusArtLayer);
            }
            Undo.RecordObject(t, t.gameObject.name);
            EditorUtility.SetDirty(t);
        }
    }

    [MenuItem("Tools/���س�����Դ")]
    public static void OnGetSceneUrl()
    {
        ViewResult<sceneData> memberResult = null;
        Dictionary<string, string> memberRequest = new Dictionary<string, string>();
        memberRequest["id"] = "101";
        RequestEditor.HttpSend(6, "get", memberRequest, (statusCode, error, body) =>
        {
            memberResult = JsonUtility.FromJson<ViewResult<sceneData>>(body);
            OnDownLoadSceneTesture(memberResult.data.qiniu_path);
            OnDownLoadScene(memberResult.data.fbx_file_url,memberResult.data.qiniu_path);
            Debug.Log("data.fbx_file_url : " + memberResult.data.fbx_file_url);
            Debug.Log("data.qiniu_path : " + memberResult.data.qiniu_path);
        });
    }


    //[MenuItem("Tools/���س�����Դ")]
    public static void OnDownLoadScene(string url,string buildurl)
    {
        //string url = @"https://s3.taihuoniao.com/unity/scene.fbx";
        string progress = null;
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
            DirectoryInfo t = new DirectoryInfo(Application.dataPath + "/Resources");
            if (!t.Exists)
            {
                //������ļ��в������򴴽�  
                t.Create();
            }
            FileInfo j = new FileInfo(Application.dataPath + "/Resources/scene.fbx");
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
            OnAddSceneModel(buildurl);
        }
    }
    public static void OnDownLoadSceneTesture(string url)
    {
        List<string> files = QiNiuHelp.ListFiles(Path.Combine(url, "textures").Replace("\\", "/"),Globle.SceneBucket);
        foreach (string i in files)
        {
            string path = Path.Combine(Globle.FrfileHost, i).Replace("\\", "/");
            Debug.Log("OnDownLoadSceneTesture  Path ; " + path);
            WWW w = new WWW(path);
            while (!w.isDone)
            {

            }
            if (w.isDone)
            {
                byte[] model = w.bytes;
                int length = model.Length;
                Stream sw;
                DirectoryInfo t = new DirectoryInfo(Application.dataPath + "/AssetsPackages/OtherPrefabs/textures");
                if (!t.Exists)
                {
                    t.Create();
                }
                string[] aa = i.Split("/");
                FileInfo j = new FileInfo(Application.dataPath + "/AssetsPackages/OtherPrefabs/textures/" + aa[aa.Length-1]);
                if (!j.Exists)
                {
                    sw = j.Create();
                }
                else
                {
                    sw = j.OpenWrite();
                }
                sw.Write(model, 0, length);
                sw.Close();
                sw.Dispose();
                UnityEditor.AssetDatabase.Refresh();
            }
        }
       
    }


    public static void OnAddSceneModel(string buildurl)
    {
        GameObject scene = new GameObject("scene");
        GameObject obj = Resources.Load("scene") as GameObject;
        obj=Instantiate(obj);
        obj.transform.parent = scene.transform;
        obj.transform.localEulerAngles = new Vector3(0, 0, 0);
        OnLayerH();
        AddMeshCollider.Add();
        string targetPath ="Assets/AssetsPackages/OtherPrefabs/" + scene.name+".prefab";
        PrefabUtility.SaveAsPrefabAsset(scene, targetPath);
        OnDestoryObj();
        //OnSetAssetBundelName(buildurl);
    }

    private static void OnSetAssetBundelName(string buildurl)
    {
        if (Directory.Exists(Application.dataPath + "/AssetsPackages/OtherPrefabs/"))
        {
            DirectoryInfo direction = new DirectoryInfo(Application.dataPath + "/AssetsPackages/OtherPrefabs/");
            FileInfo[] files = direction.GetFiles("*");
            for (int i = 0; i < files.Length; i++)
            {
                //ȥ��Unity�ڲ�.meta�ļ�
                if (files[i].Name.EndsWith(".meta"))
                    continue;
                string path = "Assets/AssetsPackages/OtherPrefabs/" + files[i].Name;
                GameObject prefab = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;
                prefab.transform.rotation = Quaternion.Lerp(prefab.transform.rotation, Quaternion.Euler(-90, 180, 0), 0.01f);
                string a_path = AssetDatabase.GetAssetPath(prefab);
                AssetImporter asset = AssetImporter.GetAtPath(a_path);
                asset.assetBundleName = "scene";
                asset.assetBundleVariant = "ab";
                asset.SaveAndReimport();
            }
        }
        CreateAssetBundles.BuildAllAssetBundlesLocal();
        // UploadAsset.OnUpLoadAB(buildurl);
        // DeleteAllFile(Application.dataPath + "/AssetsPackages/OtherPrefabs",false);
        // DeleteAllFile(Application.dataPath + "/AssetsPackages/OtherPrefabs/textures",false);
        // DeleteAllFile(Application.dataPath + "/Resources",true);
        UnityEditor.AssetDatabase.Refresh();
    }

    public static void DeleteAllFile(string deletePath,bool isscene)
    {
        //��ȡָ��·�������������Դ�ļ�  Ȼ�����ɾ��
        if (Directory.Exists(deletePath))
        {
            DirectoryInfo direction = new DirectoryInfo(deletePath);
            FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);

            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].Name.EndsWith(".meta"))
                {
                    continue;
                }
                if (isscene)
                {
                    if (files[i].Name.Contains("scene"))
                    {
                        string FilePath = deletePath + "/" + files[i].Name;
                        File.Delete(FilePath);
                    }
                }
                else
                {
                    string FilePath = deletePath + "/" + files[i].Name;
                    File.Delete(FilePath);
                }
                
                
            }
        }
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


