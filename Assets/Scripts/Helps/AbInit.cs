using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
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

        float time = 0;
        float lodingindex = 0;

        // Slider slider;


        private void Awake()
        {
            instances = this;
            // OnWebRequestAssetBundleManifest();
#if !UNITY_EDITOR && UNITY_WEBGL
            enabled = false; // 默认不启动 前端发送场景url 后启动
#endif
        }


        private void Start()
        {
            // slider = transform.Find("Mask/Slider").GetComponent<Slider>();
            // AssetBundle.UnloadAllAssetBundles(true);
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
                    // slider.value = lodingindex;
                    lodingindex += 0.007f;

                    time = Time.time;
                }
            }
        }

        public void FinishSlider()
        {
            lodingindex = 1;
            // slider.value = lodingindex;
            // transform.Find("Mask").gameObject.SetActive(false);
        }
        
        public IEnumerator OnWebRequestAssetBundleManifest()
        {
            if (manifest) yield break;
            string path = null;
#if !UNITY_EDITOR && UNITY_WEBGL
            path = Path.Combine(Globle.AssetHost, Globle.QiNiuPrefix, Globle.AssetVision, Globle.AssetBundleDir);
            path = path.Replace("\\", "/");
#else
            path = Path.Combine(Application.dataPath, "AssetsBundles");
#endif
            // UnityWebRequest deps = UnityWebRequestAssetBundle.GetAssetBundle(path + "/AssetsBundles");
            string depsUrl = Path.Combine(path, "AssetsBundles").Replace("\\", "/");
            UnityWebRequest deps = UnityWebRequest.Get(depsUrl);
            yield return deps.SendWebRequest();

            if (!string.IsNullOrEmpty(deps.error))
            {
                throw new Exception(deps.error);
            }
            byte[] abData = deps.downloadHandler.data;
#if !UNITY_EDITOR && UNITY_WEBGL
                abData = Aes.AESDecrypt(abData, Globle.AesKey, Globle.AesIv);
#endif
            assetBundleManifest = AssetBundle.LoadFromMemory(abData);
            manifest = assetBundleManifest.LoadAsset<AssetBundleManifest>("assetbundlemanifest");
        }

        public void ReloadManifest()
        {
            foreach (var i in AssetBundelLightMapDic)
            {
                i.Value.UnloadAsync(false);
            }   
            assetBundleManifest.UnloadAsync(false);
        }


        public IEnumerator OnWebRequestAssetBundleManifestScene(string url,string name)
        {
            if (sceneManifestList.Count != 0) yield break;
            string depsUrl = null;
            string data = null;
#if !UNITY_EDITOR && UNITY_WEBGL
            depsUrl= Path.Combine(url, name).Replace("\\", "/");
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








        public Dictionary<string, AssetBundle> AssetBundelDeps = new Dictionary<string, AssetBundle>();
        public Dictionary<string, AssetBundle> AssetBundelGameObjectDic = new Dictionary<string, AssetBundle>();
        public Dictionary<string, AssetBundle> AssetBundelLightMapDic = new Dictionary<string, AssetBundle>();

        public delegate void GameObjectCallback(GameObject obj);

        public IEnumerator OnWebRequestLoadAssetBundleGameObject(string name, string parent = "",
            GameObjectCallback callback = null)
        {
            Vector3 point = Vector3.zero;
            Vector3 rotate = new Vector3(0, 0, 0);
            yield return StartCoroutine(OnWebRequestLoadAssetBundleGameObject(name, parent, point, rotate, callback));
        }


        public IEnumerator OnWebRequestLoadAssetBundleGameObject(string name, string parent, Vector3 point,
            Vector3 rotate, GameObjectCallback callback = null)
        {
            yield return manifest != null;
            AssetBundle AB = null;
            string path = null;
#if !UNITY_EDITOR && UNITY_WEBGL
            path = Path.Combine(Globle.AssetHost, Globle.QiNiuPrefix, Globle.AssetVision, Globle.AssetBundleDir);
            path = path.Replace("\\", "/");
#else
            path = Path.Combine(Application.dataPath, "AssetsBundles");
#endif
            if (AssetBundelGameObjectDic.ContainsKey((name)))
            {
                AB = AssetBundelGameObjectDic[name];
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
                // UnityWebRequest requestAB = UnityWebRequestAssetBundle.GetAssetBundle(abPath);
                UnityWebRequest requestAB = UnityWebRequest.Get(abPath);
                yield return requestAB.SendWebRequest();
                if (!string.IsNullOrEmpty(requestAB.error))
                {
                    throw new Exception("请求资源包 "+name+" 错误 "+ requestAB.error+" "+abPath);
                }

                // AB = DownloadHandlerAssetBundle.GetContent(requestAB); 
                byte[] abData = requestAB.downloadHandler.data;
#if !UNITY_EDITOR && UNITY_WEBGL
                abData = Aes.AESDecrypt(abData, Globle.AesKey, Globle.AesIv);
#endif
                //abData = Aes.AESDecrypt(abData, Globle.AesKey, Globle.AesIv);
                AB = AssetBundle.LoadFromMemory(abData);
                AssetBundelGameObjectDic.Add(name, AB);
            }

            if (AB == null)
            {
                throw (new Exception("资源包"+name+"加载错误"));
            }

            GameObject obj = Instantiate(AB.LoadAsset<GameObject>(name), point, Quaternion.Euler(rotate));
            // obj.transform.localPosition = point;
            // obj.transform.localRotation = Quaternion.Euler(rotate);
            if (callback != null)
            {
                callback(obj);
            }

            AB.UnloadAsync(false);
        }

        public delegate void MaterialCallback(Material material);
        public IEnumerator OnWebRequestLoadAssetBundleMaterial(string name, string parent="", MaterialCallback callback = null)
        {
            AssetBundle AB = null;
            string path = null;
#if !UNITY_EDITOR && UNITY_WEBGL
            path = Path.Combine(Globle.AssetHost, Globle.QiNiuPrefix, Globle.AssetVision, Globle.AssetBundleDir);
            path = path.Replace("\\", "/");
#else
            path = Path.Combine(Application.dataPath, "AssetsBundles");
#endif
            if (AssetBundelGameObjectDic.ContainsKey((name)))
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
                    throw new Exception("请求资源包 "+name+" 错误 "+ requestAB.error);
                }

                // AB = DownloadHandlerAssetBundle.GetContent(requestAB); 
                byte[] abData = requestAB.downloadHandler.data;
#if !UNITY_EDITOR && UNITY_WEBGL
                abData = Aes.AESDecrypt(abData, Globle.AesKey, Globle.AesIv);
#endif
                AB = AssetBundle.LoadFromMemory(abData);
                AssetBundelGameObjectDic.Add(name, AB);
            }

            if (AB == null)
            {
                throw (new Exception("资源包"+name+"加载错误"));
            }

            Material material = AB.LoadAsset<Material>(name);
            if (callback != null)
            {
                callback(material);
            }

            AB.UnloadAsync(false);
        }




        public IEnumerator OnWebRequestLoadAssetBundleGameObjectUrl(string name, string url, bool isWeb,
            GameObjectCallback callback = null)
        {
            Vector3 point = Vector3.zero;
            Vector3 rotate = new Vector3(0, 0, 0);
            if (isWeb)
            {
                yield return StartCoroutine(OnWebRequestLoadAssetBundleGameObjectUrl(name, url, point, rotate, callback));
            }
            else
            {
                yield return StartCoroutine(OnWebRequestLoadAssetBundleGameObjectScene(name, url, point, rotate, callback));
            }
        }
        public IEnumerator OnWebRequestLoadAssetBundleGameObjectUrl(string name, string url, Vector3 point,
            Vector3 rotate, GameObjectCallback callback = null)
        {
            yield return StartCoroutine(OnLoadSceneLightmapAB());

            AssetBundle AB = null;
            // UnityWebRequest requestAB = UnityWebRequest.Get(url);
            UnityWebRequest requestAB = UnityWebRequestAssetBundle.GetAssetBundle(url);
            yield return requestAB.SendWebRequest();
            if (!string.IsNullOrEmpty(requestAB.error))
            {
                throw new Exception("请求资源包 "+name+" 错误 "+ requestAB.error);
            }
            
            AB = DownloadHandlerAssetBundle.GetContent(requestAB); 

            // byte[] abData = requestAB.downloadHandler.data;
            // abData = Aes.AESDecrypt(abData, Globle.AesKey, Globle.AesIv);

            // AB = AssetBundle.LoadFromMemory(abData);

            if (AB == null)
            {
                throw (new Exception("资源包"+name+"加载错误"));
            }

            GameObject obj = Instantiate(AB.LoadAsset<GameObject>(name), point, Quaternion.Euler(rotate));
            if (callback != null)
            {
                callback(obj);
            }

            AB.UnloadAsync(false);
        }


        public IEnumerator OnWebRequestLoadAssetBundleGameObjectScene(string name, string url, Vector3 point,
            Vector3 rotate, GameObjectCallback callback = null)
        {
            yield return StartCoroutine(OnLoadSceneLightmapAB());
            string path = Path.Combine(Application.dataPath, "AssetsBundles");
            string abPath = Path.Combine(path, name).Replace("\\", "/") + ".ab";
            UnityWebRequest requestAB = UnityWebRequest.Get(abPath);
            yield return requestAB.SendWebRequest();
            if (!string.IsNullOrEmpty(requestAB.error))
            {
                throw new Exception("请求资源包 " + name + " 错误 " + requestAB.error + " " + abPath);
            }
            byte[] abData = requestAB.downloadHandler.data;
            AssetBundle AB = AssetBundle.LoadFromMemory(abData);
            GameObject obj = Instantiate(AB.LoadAsset<GameObject>(name), point, Quaternion.Euler(rotate));
            if (callback != null)
            {
                callback(obj);
            }

            AB.UnloadAsync(false);
        }


        private IEnumerator OnLoadSceneLightmapAB()
        {
            yield return sceneManifestList.Count != 0;
            string[] deps = new string[sceneManifestList.Count];
            for (int i = 0; i < sceneManifestList.Count; i++)
            {
                deps[i] = sceneManifestList[i] + ".ab";
            }
            string deppath = null;
#if !UNITY_EDITOR && UNITY_WEBGL
            deppath = Path.Combine(Globle.AssetHost, Globle.QiNiuPrefix, Globle.AssetVision, Globle.AssetBundleDir);
            deppath = deppath.Replace("\\", "/");
#else
            deppath = Path.Combine(Application.dataPath, "AssetsBundles").Replace("\\", "/");
#endif
            foreach (var item in deps)
            {
                if (!AssetBundelLightMapDic.ContainsKey("lightmap"))
                {
                    string depPath = Path.Combine(deppath, item).Replace("\\", "/");
                    UnityWebRequest dep = UnityWebRequest.Get(depPath);
                    yield return dep.SendWebRequest();
                    if (!string.IsNullOrEmpty(dep.error))
                    {
                        throw new Exception(dep.error);
                    }
                    byte[] depBytes = dep.downloadHandler.data;
#if !UNITY_EDITOR && UNITY_WEBGL
                    depBytes = Aes.AESDecrypt(depBytes, Globle.AesKey, Globle.AesIv);
#endif
                    AssetBundle andep = AssetBundle.LoadFromMemory(depBytes);
                    if (!AssetBundelLightMapDic.ContainsKey(item))
                    {
                        AssetBundelLightMapDic.Add(item, andep);
                    }
                }
            }
        }


        public delegate void TextureCallback(Texture obj);

        public IEnumerator DownloadTexture(string url, TextureCallback callback = null)
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

        public void ReplaceMaterialContent(GameObject obj, string url, int nKind)
        {
            if (obj.TryGetComponent<VideoPlayer>(out VideoPlayer videoPlayer))
            {
                Destroy(videoPlayer);
            }
            if (nKind == 2)
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