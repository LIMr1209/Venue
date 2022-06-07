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
    Transform root;

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
        root = transform.Find("Root");
        AssetBundle.UnloadAllAssetBundles(true);

        StartCoroutine(OnLoadScenePanel());
    }


    public IEnumerator OnLoadScenePanel()
    {
        StartCoroutine(OnWebRequestAssetBundleUIPanel("otherpanel", new Vector3(273, -148, 0), root));
        yield return new WaitForSeconds(0.05f);
        StartCoroutine(OnWebRequestAssetBundleUIPanel("sharepanel", new Vector3(209, -290, 0), root));
        yield return new WaitForSeconds(0.05f);
        yield return StartCoroutine(OnWebRequestAssetBundleUIPanel("scenepanel", new Vector3(0, 0, 0), root));
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
    public IEnumerator OnWebRequestAssetBundleUIPanel(string name, Vector3 point, Transform parent)
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


    public IEnumerator OnWebRequestLoadAssetBundleGameObject(string name,Vector3 point)
    {
        string path = Path.Combine(Globle.AssetHost, Globle.QiNiuPrefix, Globle.AssetVision, Globle.AssetBundleDir);
        path = path.Replace("\\", "/");
        Debug.Log(path);
        UnityWebRequest requestAB = UnityWebRequestAssetBundle.GetAssetBundle(Path.Combine(path, name) + ".ab");
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
            obj.transform.localPosition = point;
        }
    }

}
