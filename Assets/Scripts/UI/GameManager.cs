using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using System.IO;
using UnityEngine.UI;
using DefaultNamespace;

public class GameManager : MonoBehaviour
{
    public static GameManager instances;

    public bool isClick = false;

    public Dictionary<string, UIbase> UIdic = new Dictionary<string, UIbase>();

    public Dictionary<string, AssetBundle> depUIDic = new Dictionary<string, AssetBundle>();

    private GameManager()
    {
       
    }

    private void Awake()
    {
        instances = this;
    }

    private void Start()
    {
        AssetBundle.UnloadAllAssetBundles(true);
        StartCoroutine(OnLoadScenePanel());
    }

    public IEnumerator OnLoadScenePanel()
    {
        yield return StartCoroutine(OnWebRequestAssetBundle("scenepanel", new Vector3(0, 0, 0), transform.Find("Root").transform));
        //yield return transform.Find("Root/ScenePanel").transform != null;
        StartCoroutine(OnWebRequestAssetBundle("sharepanel", new Vector3(209, -290, 0), ScenePanel.instance.transform));
        yield return StartCoroutine(OnWebRequestAssetBundle("otherpanel", new Vector3(273, -148, 0), ScenePanel.instance.transform));
        Debug.Log("加载完成");
        transform.Find("Mask").gameObject.SetActive(false);
    }
    


    //内部加载AssetBundel
    public IEnumerator OnLoadUIPanel(string name, Vector3 point, Transform parent)
    {
        AssetBundle.UnloadAllAssetBundles(true);
        string path = Application.dataPath + "/AssetsBundles/";
        UnityWebRequest deps = UnityWebRequestAssetBundle.GetAssetBundle(path+ "AssetsBundles");
        yield return deps.SendWebRequest();
        if (!string.IsNullOrEmpty(deps.error))
        {
            Debug.LogError(deps.error);
            yield break;
        }
        AssetBundle abdeps = DownloadHandlerAssetBundle.GetContent(deps);
        AssetBundleManifest manifest = abdeps.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        string[] depslist = manifest.GetAllDependencies("uiprefabs/" + name + ".ab");
        foreach (string _name in depslist)
        {
            UnityWebRequest dep = UnityWebRequestAssetBundle.GetAssetBundle(path + _name);
            yield return dep.SendWebRequest();
            AssetBundle andep = DownloadHandlerAssetBundle.GetContent(dep);
        }
        UnityWebRequest requestAB = UnityWebRequestAssetBundle.GetAssetBundle(path + "uiprefabs/" + name + ".ab");
        yield return requestAB.SendWebRequest();
        if (!string.IsNullOrEmpty(requestAB.error))
        {
            Debug.LogError(requestAB.error);
            yield break;
        }
        AssetBundle AB = DownloadHandlerAssetBundle.GetContent(requestAB);
        if (AB != null)
        {
            GameObject obj = Instantiate(AB.LoadAsset<GameObject>(name));
            obj.transform.SetParent(parent);
            obj.transform.localPosition = point;
            obj.SetActive(true);
            UIdic.Add(name, obj.GetComponent<UIbase>());
        }
        
    }


    //外部加载AssetBundel
    public IEnumerator OnWebRequestAssetBundle(string name, Vector3 point, Transform parent)
    {
        
        string path = Path.Combine(Globle.AssetHost, Globle.QiNiuPrefix, Globle.AssetVision, Globle.AssetBundleDir);
        path = path.Replace("\\", "/");
        Debug.Log(path);
        AssetBundle AssetBundleManifest = null;
        if (depUIDic.ContainsKey("AssetBundleManifest"))
        {
            AssetBundleManifest = depUIDic["AssetBundleManifest"];
        }
        else
        {
            UnityWebRequest deps = UnityWebRequestAssetBundle.GetAssetBundle(path + "/AssetsBundles");
            yield return deps.SendWebRequest();
            if (!string.IsNullOrEmpty(deps.error))
            {
                Debug.LogError(deps.error);
                yield break;
            }
            AssetBundleManifest = DownloadHandlerAssetBundle.GetContent(deps);
            depUIDic.Add("AssetBundleManifest", AssetBundleManifest);
        }
        AssetBundleManifest manifest = AssetBundleManifest.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        string[] depslist = manifest.GetAllDependencies("uiprefabs/" + name + ".ab");
        foreach (string _name in depslist)
        {
            if (depUIDic.ContainsKey(_name))
            {

            }
            else
            {
                string depPath = Path.Combine(path, _name);
                UnityWebRequest dep = UnityWebRequestAssetBundle.GetAssetBundle(depPath);
                yield return dep.SendWebRequest();
                AssetBundle andep = DownloadHandlerAssetBundle.GetContent(dep);
            }
        }
        UnityWebRequest requestAB = UnityWebRequestAssetBundle.GetAssetBundle(path + "/uiprefabs/" + name + ".ab");
        yield return requestAB.SendWebRequest();
        if (!string.IsNullOrEmpty(requestAB.error))
        {
            Debug.LogError(requestAB.error);
            yield break;
        }
        AssetBundle AB = DownloadHandlerAssetBundle.GetContent(requestAB);
        if (AB != null)
        {
            GameObject obj = Instantiate(AB.LoadAsset<GameObject>(name));
            obj.transform.SetParent(parent);
            obj.transform.localPosition = point;
            if(name!= "scenepanel")
            {
                UIdic.Add(name, obj.GetComponent<UIbase>());
                obj.SetActive(false);
            }
        }
    }

}
