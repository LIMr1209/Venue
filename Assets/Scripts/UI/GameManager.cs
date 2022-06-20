using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using System.IO;
using UnityEngine.UI;
using DefaultNamespace;
using static DefaultNamespace.JsonData;

public class GameManager : MonoBehaviour
{
    public static GameManager instances;

    public bool isLoad;

    Transform root;

    Slider slider;

    public Dictionary<string, UIbase> UIdic = new Dictionary<string, UIbase>();

    public Dictionary<string, AssetBundle> depUIDic = new Dictionary<string, AssetBundle>();

    //public ViewResult<memberData> memberResult;

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
        //memberResult = OnMemberRequest();
        StartCoroutine(OnLoadScenePanel());
        
    }
    

    private void Update()
    {
       
        OnLoadingIndex();

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
        
    }


    public ViewResult<memberData> OnMemberRequest()
    {
        ViewResult<memberData> memberResult = null;
        // 获取房间成员
        Dictionary<string, string> memberRequest = new Dictionary<string, string>();
        memberRequest["id"] = Globle.roomId;
        memberRequest["token"] = Globle.token; // token 
        Request.instances.HttpSend(5, "get", memberRequest, (statusCode, error, body) =>
        {
            memberResult = JsonUtility.FromJson<ViewResult<memberData>>(body);
        });
        return memberResult;
    }

    public int OnGetMemberRequestNum()
    {
        return OnMemberRequest().data.invited_user.Length + OnMemberRequest().data.stranger.Length + 1;
    }


    public IEnumerator DownTexture(Image image, string url)
    {
        UnityWebRequest WebRequest = new UnityWebRequest(url);
        DownloadHandlerTexture Download = new DownloadHandlerTexture(true);
        WebRequest.downloadHandler = Download;
        yield return WebRequest.SendWebRequest();
        while (!WebRequest.isDone)
        {
            yield return null;
        }
        if (string.IsNullOrEmpty(WebRequest.error))
        {
            Texture2D rexture = Download.texture;
            image.sprite = GetSpriteByTexture(rexture);
        }
        else
        {
            //文件下载失败
            Debug.Log("文件下载失败");
        }
    }
    //将texture转成image的Sprite
    Sprite GetSpriteByTexture(Texture2D tex)
    {
        Sprite _sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        return _sprite;
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
        // UnityWebRequest deps = UnityWebRequestAssetBundle.GetAssetBundle(path + "/AssetsBundles");
        UnityWebRequest deps = UnityWebRequest.Get(path + "/AssetsBundles");
        yield return deps.SendWebRequest();
        if (!string.IsNullOrEmpty(deps.error))
        {
            Debug.LogError(deps.error);
            yield break;
        }
        byte[] abData = deps.downloadHandler.data;
        if (isLoad)
        {
            abData = Aes.AESDecrypt(abData, Globle.AesKey, Globle.AesIv);
        }
        depUIDic.Add("AssetBundleManifest", AssetBundle.LoadFromMemory(abData));

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
                // UnityWebRequest dep = UnityWebRequestAssetBundle.GetAssetBundle(depPath);
                UnityWebRequest dep = UnityWebRequest.Get(depPath);
                yield return dep.SendWebRequest();
                byte[] abData = dep.downloadHandler.data;
                if (isLoad)
                {
                    abData = Aes.AESDecrypt(abData, Globle.AesKey, Globle.AesIv);
                }

                AssetBundle andep = AssetBundle.LoadFromMemory(abData);
            }
        }
        // UnityWebRequest requestAB = UnityWebRequestAssetBundle.GetAssetBundle(path + "/uiprefabs/" + name + ".ab");
        UnityWebRequest requestAB = UnityWebRequest.Get(path + "/uiprefabs/" + name + ".ab");
        yield return requestAB.SendWebRequest();
        if (!string.IsNullOrEmpty(requestAB.error))
        {
            Debug.LogError(requestAB.error);
            yield break;
        }
        // AssetBundle AB = DownloadHandlerAssetBundle.GetContent(requestAB);
        byte[] requestABData = requestAB.downloadHandler.data;
        if (isLoad)
        {
            requestABData = Aes.AESDecrypt(requestABData, Globle.AesKey, Globle.AesIv);
        }

        AssetBundle AB = AssetBundle.LoadFromMemory(requestABData);
        if (AB != null)
        {
            GameObject obj = Instantiate(AB.LoadAsset<GameObject>(name));
            obj.transform.SetParent(parent);
            obj.transform.position = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            obj.transform.localPosition = point;
            Debug.Log(obj.name);
            if (name!= "scenepanel")
            {
                UIdic.Add(name, obj.GetComponent<UIbase>());
                obj.SetActive(false);
               
            }
        }
    }
    
    public Dictionary<string, AssetBundle> AssetBundelGameObjectDic = new Dictionary<string, AssetBundle>();

    public IEnumerator OnWebRequestLoadAssetBundleGameObject(string name, string parent = "")
    {
       Vector3 point = Vector3.zero;;
       Vector3 rotate = new Vector3(0, 0, 0);
       yield return StartCoroutine(OnWebRequestLoadAssetBundleGameObject(name, parent, point, rotate));
    }
    
    public IEnumerator OnWebRequestLoadAssetBundleGameObject(string name, string parent, GameObjectCallback callback)
    {
        Vector3 point = Vector3.zero;;
        Vector3 rotate = new Vector3(0, 0, 0);
        yield return StartCoroutine(OnWebRequestLoadAssetBundleGameObject(name, parent, point, rotate, callback));
    }

    public delegate void GameObjectCallback(GameObject obj);
    public IEnumerator OnWebRequestLoadAssetBundleGameObject(string name, string parent, Vector3 point, Vector3 rotate, GameObjectCallback callback=null)
    {
        AssetBundle AB = null;
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
        if(AssetBundelGameObjectDic.ContainsKey((name)))
        {
            AB = AssetBundelGameObjectDic[name];    
        }
        else
        {
            string abPath = Path.Combine(path, parent, name).Replace("\\", "/") + ".ab";
            // UnityWebRequest requestAB = UnityWebRequestAssetBundle.GetAssetBundle(abPath);
            UnityWebRequest requestAB = UnityWebRequest.Get(abPath);
            yield return requestAB.SendWebRequest();
            if (!string.IsNullOrEmpty(requestAB.error))
            {
                Debug.LogError(requestAB.error);
                yield break;
            }
            // AB = DownloadHandlerAssetBundle.GetContent(requestAB); 
            byte[] abData = requestAB.downloadHandler.data;
            if (isLoad)
            {
                abData = Aes.AESDecrypt(abData, Globle.AesKey, Globle.AesIv);
            }

            AB = AssetBundle.LoadFromMemory(abData);
            AssetBundelGameObjectDic.Add(name, AB);
        }
        if (AB != null)
        {
            GameObject obj = Instantiate(AB.LoadAsset<GameObject>(name), point, Quaternion.Euler(rotate));
            // obj.transform.localPosition = point;
            // obj.transform.localRotation = Quaternion.Euler(rotate);
            if (callback != null)
            {
                callback(obj);
                lodingindex = 1;
                slider.value = lodingindex;
                Debug.Log("加载完成时间： " + (Time.time - time));
                yield return new WaitForSeconds(0.5f);
                transform.Find("Mask").gameObject.SetActive(false);
            }
            
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
            // UnityWebRequest deps = UnityWebRequestAssetBundle.GetAssetBundle(path + "/AssetsBundles");
            UnityWebRequest deps = UnityWebRequest.Get(path + "/AssetsBundles");
            yield return deps.SendWebRequest();
            if (!string.IsNullOrEmpty(deps.error))
            {
                Debug.LogError(deps.error);
                yield break;
            }
            byte[] abData = deps.downloadHandler.data;
            if (isLoad)
            {
                abData = Aes.AESDecrypt(abData, Globle.AesKey, Globle.AesIv);
            }

            AssetBundleManifest= AssetBundle.LoadFromMemory(abData);
            // AssetBundleManifest = DownloadHandlerAssetBundle.GetContent(deps);
            depUIDic.Add("AssetBundleManifest", AssetBundleManifest);
        }
        AssetBundleManifest manifest = AssetBundleManifest.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        string[] depslist = manifest.GetAllAssetBundles();
        foreach (string _name in depslist)
        {
            if (_name.Contains("expression") && _name != "uiprefabs/expressionpanel.ab" && _name != "uiprefabs/expression1.ab")
            {
                string depPath = Path.Combine(path, _name);
                // UnityWebRequest dep = UnityWebRequestAssetBundle.GetAssetBundle(depPath);
                UnityWebRequest dep = UnityWebRequest.Get(depPath);
                yield return dep.SendWebRequest();
                if (!string.IsNullOrEmpty(dep.error))
                {
                    Debug.LogError(dep.error);
                    yield break;
                }
                byte[] abData = dep.downloadHandler.data;
                if (isLoad)
                {
                    abData = Aes.AESDecrypt(abData, Globle.AesKey, Globle.AesIv);
                }

                AssetBundle andep = AssetBundle.LoadFromMemory(abData);
                ExpressionList.Add(andep);
                ExpressionList.Add(andep);
            }
        }
    }

}
