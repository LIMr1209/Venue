using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Video;

// using UnityEngine.UI;

namespace DefaultNamespace
{
    public class AbInit : MonoBehaviour
    {
        public static AbInit instances;
        public AssetBundleManifest manifest;
        public AssetBundle assetBundleManifest;
        public List<string> sceneManifestList = new List<string>();
        public Dictionary<string, AssetBundle> AssetBundelDeps = new Dictionary<string, AssetBundle>();
        public Dictionary<string, AssetBundle> AssetBundelDic = new Dictionary<string, AssetBundle>();
        public Dictionary<string, AssetBundle> AssetBundelLightMapDic = new Dictionary<string, AssetBundle>();

        public delegate void Callback<T>(T obj);

        float time = 0;
        float lodingindex = 0;

        private void Awake()
        {
            instances = this;
#if !UNITY_EDITOR && UNITY_WEBGL
            enabled = false; // 默认不启动 前端发送场景url 后启动
#endif
        }

        private void Start()
        {
            StartCoroutine(OnWebRequestAssetBundleManifest());
        }


        private void Update()
        {
            OnLoadingIndex();
#if !UNITY_EDITOR && UNITY_WEBGL
            if(lodingindex<=1){
                Tools.sendProcess(lodingindex);
            }
#endif
        }

        private void OnLoadingIndex()
        {
            if (Time.time - time > 0.1f)
            {
                if (lodingindex <= 1)
                {
                    lodingindex += 0.007f;

                    time = Time.time;
                }
            }
        }

        public void FinishSlider()
        {
            lodingindex = 1;
        }

        // 获取ab包 AssetsBundles目录路径 
        private string GetAssetsBundlesPath()
        {
            string path = "";
#if !UNITY_EDITOR && UNITY_WEBGL
            path = Path.Combine(Globle.AssetHost, Globle.QiNiuPrefix, Globle.AssetVision, Globle.AssetBundleDir);
            path = path.Replace("\\", "/");
#else
            path = Path.Combine(Application.dataPath, "AssetsBundles");
#endif
            return path;
        }

        // 获取ab包 bytes 并解密
        private Byte[] GetAbBytes(Byte[] data, bool encryption = true)
        {
            if (!encryption)
            {
                return data;
            }
            else
            {
#if !UNITY_EDITOR && UNITY_WEBGL
                data = Aes.AESDecrypt(data, Globle.AesKey, Globle.AesIv);
#endif
                return data;
            }
        }

        // 根据url 请求ab 包
        IEnumerator GetAssetBundle(string url, Callback<AssetBundle> callback = null, bool needUnload = true, bool encryption=false)
        {
            AssetBundle AB;
            UnityWebRequest dep = UnityWebRequest.Get(url);
            yield return dep.SendWebRequest();
            if (!string.IsNullOrEmpty(dep.error))
            {
                throw new Exception(dep.error);
            }

            byte[] depBytes = GetAbBytes(dep.downloadHandler.data, encryption);
            AB = AssetBundle.LoadFromMemory(depBytes);
            if(AB == null) throw (new Exception("资源包加载错误"));
            if (callback != null) callback(AB);
            if (needUnload) AB.UnloadAsync(false);
        }
        

        // 加载项目 ab 包 
        IEnumerator GetAssetBundle(string name, string parent, Callback<AssetBundle> callback = null, bool needUnload = true)
        {
            yield return manifest != null;
            AssetBundle AB;
            string path = GetAssetsBundlesPath();
            if (AssetBundelDic.ContainsKey((name)))
            {
                AB = AssetBundelDic[name];
            }
            else
            {
//                string relativePath = Path.Combine(parent, name).Replace("\\", "/") + ".ab";
//                string[] depslist = manifest.GetAllDependencies(relativePath);
//                foreach (string _name in depslist)
//                {
//                    if (!AssetBundelDeps.ContainsKey(_name))
//                    {
//                        string depPath = Path.Combine(path, _name);
//                        // UnityWebRequest dep = UnityWebRequestAssetBundle.GetAssetBundle(depPath);
//                        UnityWebRequest dep = UnityWebRequest.Get(depPath);
//                        yield return dep.SendWebRequest();
//                        byte[] depBytes = dep.downloadHandler.data;
//#if !UNITY_EDITOR && UNITY_WEBGL
//                         depBytes = Aes.AESDecrypt(depBytes, Globle.AesKey, Globle.AesIv);
//#endif
//                        AssetBundle andep = AssetBundle.LoadFromMemory(depBytes);
//                    }
//                }
                string abPath = Path.Combine(path, parent, name).Replace("\\", "/") + ".ab";
                UnityWebRequest requestAB = UnityWebRequest.Get(abPath);
                yield return requestAB.SendWebRequest();
                if (!string.IsNullOrEmpty(requestAB.error))
                {
                    throw new Exception("请求资源包 " + name + " 错误 " + requestAB.error);
                }

                byte[] abData = GetAbBytes(requestAB.downloadHandler.data);
                AB = AssetBundle.LoadFromMemory(abData);
            }
            if (AB == null) throw (new Exception("资源包" + name + "加载错误"));
            if (callback != null) callback(AB);
            if (needUnload) AB.UnloadAsync(false);
            if(!needUnload) AssetBundelDic.Add(name, AB);
        }

        // 加载 AssetBundles 总依赖
        public IEnumerator OnWebRequestAssetBundleManifest()
        {
            if (manifest) yield break;
            string path = GetAssetsBundlesPath();
            string depsUrl = Path.Combine(path, "AssetsBundles").Replace("\\", "/");
            UnityWebRequest deps = UnityWebRequest.Get(depsUrl);
            yield return deps.SendWebRequest();

            if (!string.IsNullOrEmpty(deps.error))
            {
                throw new Exception("加载总依赖错误:"+deps.error);
            }

            byte[] abData = GetAbBytes(deps.downloadHandler.data);
            assetBundleManifest = AssetBundle.LoadFromMemory(abData);
            manifest = assetBundleManifest.LoadAsset<AssetBundleManifest>("assetbundlemanifest");
        }

        // 重置场景时 需要卸载ab包
        public void ReloadManifest()
        {
            foreach (var i in AssetBundelLightMapDic)
            {
                i.Value.UnloadAsync(false);
            }

            assetBundleManifest.UnloadAsync(false);
        }


        public IEnumerator OnWebRequestLoadAssetBundleGameObject(string name, string parent = "",
            Callback<GameObject> callback = null, bool needUnload = true)
        {
            Vector3 point = Vector3.zero;
            Vector3 rotate = new Vector3(0, 0, 0);
            yield return StartCoroutine(OnWebRequestLoadAssetBundleGameObject(name, parent, point, rotate, needUnload, callback));
        }

        // 加载 游戏对象 ab包
        public IEnumerator OnWebRequestLoadAssetBundleGameObject(string name, string parent, Vector3 point,
            Vector3 rotate, bool needUnload = true, Callback<GameObject> callback = null)
        {
            
            AssetBundle AB = null;
            yield return StartCoroutine(GetAssetBundle(name, parent, (assetbundle) =>
            {
                AB = assetbundle;
                GameObject obj = Instantiate(AB.LoadAsset<GameObject>(name), point, Quaternion.Euler(rotate));
                if (callback != null) callback(obj);
            }, needUnload));
        }


        // 加载 材质ab 包
        public IEnumerator OnWebRequestLoadAssetBundleMaterial(string name, string parent = "",
            Callback<Material> callback = null)
        {
            AssetBundle AB = null;
            yield return StartCoroutine(GetAssetBundle(name, parent, (assetbundle) =>
            {
                AB = assetbundle;
                Material material = AB.LoadAsset<Material>(name);
                if (callback != null) callback(material);
            }));
        }
        
        // 加载贴图 ab 包

        public IEnumerator OnWebRequestLoadAssetBundleTexture(string name, string parent = "",
            Callback<Texture> callback = null)
        {
            AssetBundle AB = null;
            yield return StartCoroutine(GetAssetBundle(name, parent, (assetbundle) =>
            {
                AB = assetbundle;
                Texture texture = AB.LoadAsset<Texture>(name);
                if (callback != null) callback(texture);
            }));
        }
        
        public IEnumerator OnWebRequestAssetBundleManifestScene(string url, string name)
        {
            if (sceneManifestList.Count != 0) yield break;
            string depsUrl = null;
            string data = null;
#if !UNITY_EDITOR && UNITY_WEBGL
            depsUrl = Path.Combine(url, name).Replace("\\", "/");
#else
            depsUrl = Path.Combine(Application.dataPath, "AssetsBundles").Replace("\\", "/");
            depsUrl = Path.Combine(depsUrl, name).Replace("\\", "/");
#endif
            UnityWebRequest webR = UnityWebRequest.Get(depsUrl); //URL 是需要获取的网址地址
            yield return webR.SendWebRequest();
            data = webR.downloadHandler.text;
            string[] str = data.Split(':');
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i].Contains("/Work/venue/Assets/AssetsBundles/lightmap/lightmap"))
                {
                    string s = str[i];
                    int BeginIndex = s.IndexOf("lightmap");
                    int LastIndex = s.IndexOf(".");
                    int len = LastIndex - BeginIndex;
                    string bundleName = s.Substring(BeginIndex, len);
                    sceneManifestList.Add(bundleName);
                }
            }
        }

        public IEnumerator OnWebRequestLoadAssetBundleGameObjectUrl(string name, string url, bool isWeb,
            Callback<GameObject> callback = null)
        {
            if (isWeb)
            {
                yield return StartCoroutine(
                    OnWebRequestLoadAssetBundleGameObjectUrl(name, url, callback));
            }
            else
            {
                yield return StartCoroutine(
                    OnWebRequestLoadAssetBundleGameObjectScene(name, callback));
            }
        }

        public IEnumerator OnWebRequestLoadAssetBundleGameObjectUrl(string name, string url, Callback<GameObject> callback = null)
        {
            if (name == "scene")
            {
                yield return StartCoroutine(OnLoadSceneLightmapAB());
            }
            yield return StartCoroutine(GetAssetBundle(url, (assetbundle) =>
            {
                GameObject obj = Instantiate(assetbundle.LoadAsset<GameObject>(name));
                if (callback != null) callback(obj);
            }));
        }


        public IEnumerator OnWebRequestLoadAssetBundleGameObjectScene(string name, Callback<GameObject> callback = null)
        {
            if (name == "scene")
            {
                yield return StartCoroutine(OnLoadSceneLightmapAB());
            }
            yield return StartCoroutine(GetAssetBundle(name, "", (assetbundle) =>
            {
                GameObject obj = Instantiate(assetbundle.LoadAsset<GameObject>(name));
                if (callback != null) callback(obj);
            }));
            
        }

        private IEnumerator OnLoadSceneLightmapAB()
        {
            while (sceneManifestList.Count == 0)
            {
                yield return null;
            }

            string[] deps = new string[sceneManifestList.Count];
            for (int i = 0; i < sceneManifestList.Count; i++)
            {
                deps[i] = sceneManifestList[i] + ".ab";
            }

            string deppath = GetAssetsBundlesPath();
            foreach (var item in deps)
            {
                if (!AssetBundelLightMapDic.ContainsKey(item))
                {
                    string depPath = Path.Combine(deppath, item).Replace("\\", "/");
                    StartCoroutine(GetAssetBundle(depPath, (assetbundle) =>
                    {
                        if (!AssetBundelLightMapDic.ContainsKey(item))
                        {
                            AssetBundelLightMapDic.Add(item, assetbundle);
                        }
                    },false, true));
                }
            }
        }

        public IEnumerator DownloadTexture(string url, Callback<Texture> callback = null)
        {
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
            yield return www.SendWebRequest();
            if (!string.IsNullOrEmpty(www.error))
            {
                if (www.error == "Data Processing Error, see Download Handler error")
                {
                    throw new Exception("贴图加载错误");
                }

                throw new Exception(www.error);
            }

            Texture texture = DownloadHandlerTexture.GetContent(www);
            if (callback != null)
            {
                callback(texture);
            }
        }

        public void ReplaceMaterialContent(GameObject obj, string url, int nKind, bool init)
        {
            if (!init)
            {
                if (obj.TryGetComponent<VideoPlayer>(out VideoPlayer videoPlayer))
                {
                    Destroy(videoPlayer);
                }
            }

            if (nKind == 2 || nKind == 1)
            {
                Material material = obj.GetComponent<MeshRenderer>().material;
                Material cloneMaterial = Instantiate(material);
                obj.GetComponent<MeshRenderer>().material = cloneMaterial;
                StartCoroutine(DownloadTexture(url, (texture) =>
                    {
                        cloneMaterial.mainTexture = texture;
                        Vector2 contentSize = new Vector2(texture.width, texture.height);
                        SizeAdaptation.SetSize(obj, contentSize);
                    }
                ));
            }
            else if (nKind == 3)
            {
                StartCoroutine(PlayerVideoHelp.AddVideoComponent(obj, url));
            }
        }
    }
}