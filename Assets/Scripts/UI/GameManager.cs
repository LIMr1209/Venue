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

    public bool isLoad;

    Transform root;

    Slider slider;

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
        slider = transform.Find("Mask/Slider").GetComponent<Slider>();
        root = transform.Find("Root");
        AssetBundle.UnloadAllAssetBundles(true);

        StartCoroutine(OnLoadScenePanel());
    }
    float a;
    private void Update()
    {
       
        OnLoadingIndex();
        //Debug.Log(a);
    }

    float time = 0;
    float lodingindex = 0;
    private void OnLoadingIndex()
    {
        if(Time.time - time > 0.1f)
        {
            if (lodingindex <= 1)
            {

                slider.value = lodingindex;
                lodingindex += 0.007f;

                time = Time.time;
            }
        }
    }


    public IEnumerator OnLoadScenePanel()
    {
        time = Time.time;
        yield return StartCoroutine(OnWebRequestAssetBundleManifest());
        StartCoroutine(OnWebRequestAssetBundleUIPanel("otherpanel", new Vector3(273, -148, 0), root, isLoad));
        StartCoroutine(OnWebRequestAssetBundleUIPanel("sharepanel", new Vector3(209, -290, 0), root, isLoad));
        StartCoroutine(OnWebRequestAssetBundleUIPanel("expressionpanel", new Vector3(64, -284, 0), root, isLoad));
        StartCoroutine(OnWebRequestAssetBundleUIPanel("userpanel", new Vector3(652, 0, 0), root, isLoad));
        StartCoroutine(OnWebRequestAssetBundleUIPanel("listpanel", new Vector3(0, 40, 0), root, isLoad));
        StartCoroutine(OnLoadExpressionAssetBundel(isLoad));
        yield return StartCoroutine(OnWebRequestAssetBundleUIPanel("scenepanel", new Vector3(0, 0, 0), root, isLoad));
        lodingindex = 1;
        slider.value = lodingindex;
        Debug.Log("加载完成");
        yield return new WaitForSeconds(0.5f);
        transform.Find("Mask").gameObject.SetActive(false);
    }

    //加载AssetBundleManifest文件
    public IEnumerator OnWebRequestAssetBundleManifest()
    {
        string path = null;
        if (isLoad)
        {
            path = Path.Combine(Globle.AssetHost, Globle.QiNiuPrefix, Globle.AssetVision, Globle.AssetBundleDir);
            path = path.Replace("\\", "/");
        }
        else
        {
            path = Application.dataPath + "/AssetsBundles/";
        }
        UnityWebRequest deps = UnityWebRequestAssetBundle.GetAssetBundle(path + "/AssetsBundles");
        yield return deps.SendWebRequest();
        if (!string.IsNullOrEmpty(deps.error))
        {
            Debug.LogError(deps.error);
            yield break;
        }
        depUIDic.Add("AssetBundleManifest", DownloadHandlerAssetBundle.GetContent(deps));

    }

    //外部加载AssetBundel
    public IEnumerator OnWebRequestAssetBundleUIPanel(string name, Vector3 point, Transform parent,bool isLoad)
    {
        string path = null;
        if (isLoad)
        {
            path = Path.Combine(Globle.AssetHost, Globle.QiNiuPrefix, Globle.AssetVision, Globle.AssetBundleDir);
            path = path.Replace("\\", "/");
        }
        else
        {
            path = Application.dataPath + "/AssetsBundles/";
        }
        //StartCoroutine(OnWebRequestAssetBundleUIPaneldep(name, path));
        AssetBundle AssetBundleManifest = depUIDic["AssetBundleManifest"];
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
            obj.transform.position = Vector3.zero;
            obj.transform.localPosition = point;
            if(name!= "scenepanel")
            {
                UIdic.Add(name, obj.GetComponent<UIbase>());
                obj.SetActive(false);
            }
        }
    }

    

    public IEnumerator OnWebRequestLoadAssetBundleGameObject(string name,Vector3 point,bool isLoad)
    {
        string path = null;
        if (isLoad)
        {
            path = Path.Combine(Globle.AssetHost, Globle.QiNiuPrefix, Globle.AssetVision, Globle.AssetBundleDir);
            path = path.Replace("\\", "/");
        }
        else
        {
            path = Path.Combine(Application.dataPath, "AssetsBundles");
        }
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




    public  List<AssetBundle> ExpressionList = new List<AssetBundle>();
    public IEnumerator OnLoadExpressionAssetBundel(bool isLoad)
    {
        string path = null;
        if (isLoad)
        {
            path = Path.Combine(Globle.AssetHost, Globle.QiNiuPrefix, Globle.AssetVision, Globle.AssetBundleDir);
            path = path.Replace("\\", "/");
        }
        else
        {
            path = Application.dataPath + "/AssetsBundles/";
        }
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
        string[] depslist = manifest.GetAllAssetBundles();
        foreach (string _name in depslist)
        {
            if (_name.Contains("expression") && _name != "uiprefabs/expressionpanel.ab" && _name != "uiprefabs/expression1.ab")
            {
                string depPath = Path.Combine(path, _name);
                UnityWebRequest dep = UnityWebRequestAssetBundle.GetAssetBundle(depPath);
                yield return dep.SendWebRequest();
                if (!string.IsNullOrEmpty(dep.error))
                {
                    Debug.LogError(dep.error);
                    yield break;
                }
                AssetBundle andep = DownloadHandlerAssetBundle.GetContent(dep);
                ExpressionList.Add(andep);
                ExpressionList.Add(andep);
            }
        }
    }

}
