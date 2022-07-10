using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
// using UnityEngine.UI;

namespace DefaultNamespace
{
    public class AbInit : MonoBehaviour
    {
        public static AbInit instances;
        public AssetBundleManifest manifest;

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
        
        public void OnWebRequestAssetBundleManifest()
        {
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
            deps.SendWebRequest();
            while (!deps.isDone)
            {
                
            }
            if (!string.IsNullOrEmpty(deps.error))
            {
                throw new Exception(deps.error);
            }
            byte[] abData = deps.downloadHandler.data;
#if !UNITY_EDITOR && UNITY_WEBGL
                abData = Aes.AESDecrypt(abData, Globle.AesKey, Globle.AesIv);
#endif
            AssetBundle assetBundleManifest = AssetBundle.LoadFromMemory(abData);
            manifest = assetBundleManifest.LoadAsset<AssetBundleManifest>("assetbundlemanifest");
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


            if (manifest == null)
            {
                OnWebRequestAssetBundleManifest();
            }
            string[] deps = manifest.GetAllDependencies(name + ".ab");
            string deppath = Path.Combine(Globle.AssetHost, Globle.QiNiuPrefix, Globle.AssetVision, Globle.AssetBundleDir);
            deppath = deppath.Replace("\\", "/");
            foreach (var item in deps)
            {
                if (!AssetBundelLightMapDic.ContainsKey("lightmap"))
                {
                    string depPath = Path.Combine(deppath, item).Replace("\\", "/");
                    //UnityWebRequest dep = UnityWebRequestAssetBundle.GetAssetBundle(depPath);
                    UnityWebRequest dep = UnityWebRequest.Get(depPath);
                    yield return dep.SendWebRequest();
                    byte[] depBytes = dep.downloadHandler.data;

                    depBytes = Aes.AESDecrypt(depBytes, Globle.AesKey, Globle.AesIv);

                    AssetBundle andep = AssetBundle.LoadFromMemory(depBytes);
                    AssetBundelLightMapDic.Add(item, andep);
                }

            }





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
                    throw new Exception(requestAB.error);
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
                throw (new Exception("unity AB包资源加载错误"));
            }

            GameObject obj = Instantiate(AB.LoadAsset<GameObject>(name), point, Quaternion.Euler(rotate));
            // obj.transform.localPosition = point;
            // obj.transform.localRotation = Quaternion.Euler(rotate);
            if (callback != null)
            {
                callback(obj);
            }

            // AB.UnloadAsync(false);
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
                    throw new Exception(requestAB.error);
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
                throw (new Exception("unity AB包资源加载错误"));
            }

            Material material = AB.LoadAsset<Material>(name);
            if (callback != null)
            {
                callback(material);
            }

            AB.UnloadAsync(false);
        }

        public IEnumerator OnWebRequestLoadAssetBundleGameObjectUrl(string name, string url,
            GameObjectCallback callback = null)
        {
            Vector3 point = Vector3.zero;
            ;
            Vector3 rotate = new Vector3(0, 0, 0);
            yield return StartCoroutine(OnWebRequestLoadAssetBundleGameObjectUrl(name, url, point, rotate, callback));
        }

        public IEnumerator OnWebRequestLoadAssetBundleGameObjectUrl(string name, string url, Vector3 point,
            Vector3 rotate, GameObjectCallback callback = null)
        {
            if (manifest == null)
            {
                OnWebRequestAssetBundleManifest();
            }
            string[] deps = manifest.GetAllDependencies(name + ".ab");
            string deppath = Path.Combine(Globle.AssetHost, Globle.QiNiuPrefix, Globle.AssetVision, Globle.AssetBundleDir);
            deppath = deppath.Replace("\\", "/");
            foreach (var item in deps)
            {
                if (!AssetBundelLightMapDic.ContainsKey("lightmap"))
                {
                    string depPath = Path.Combine(deppath, item).Replace("\\", "/");
                    //UnityWebRequest dep = UnityWebRequestAssetBundle.GetAssetBundle(depPath);
                    UnityWebRequest dep = UnityWebRequest.Get(depPath);
                    dep.SendWebRequest();
                    while (!dep.isDone)
                    {

                    }
                    if (!string.IsNullOrEmpty(dep.error))
                    {
                        throw new Exception(dep.error);
                    }
                    //yield return dep.SendWebRequest();
                    byte[] depBytes = dep.downloadHandler.data;

                    depBytes = Aes.AESDecrypt(depBytes, Globle.AesKey, Globle.AesIv);

                    AssetBundle andep = AssetBundle.LoadFromMemory(depBytes);
                    AssetBundelLightMapDic.Add(item, andep);
                }
               
            }


            AssetBundle AB = null;

            // UnityWebRequest requestAB = UnityWebRequest.Get(url);
            UnityWebRequest requestAB = UnityWebRequestAssetBundle.GetAssetBundle(url);
            yield return requestAB.SendWebRequest();
            if (!string.IsNullOrEmpty(requestAB.error))
            {
                throw new Exception(requestAB.error);
            }
            
            AB = DownloadHandlerAssetBundle.GetContent(requestAB); 

            // byte[] abData = requestAB.downloadHandler.data;
            // abData = Aes.AESDecrypt(abData, Globle.AesKey, Globle.AesIv);

            // AB = AssetBundle.LoadFromMemory(abData);

            if (AB == null)
            {
                throw (new Exception("场景AB包加载错误"));
            }

            GameObject obj = Instantiate(AB.LoadAsset<GameObject>(name), point, Quaternion.Euler(rotate));
            if (callback != null)
            {
                callback(obj);
            }

            AB.UnloadAsync(false);
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
                    throw new Exception("下载贴图失败");
                }
                throw new Exception(www.error);
            }

            Texture texture = DownloadHandlerTexture.GetContent(www);
            if (callback != null)
            {
                callback(texture);
            }
        }

        public void ReplaceMaterialImage(GameObject obj, string url)
        {
            Material material = obj.GetComponent<MeshRenderer>().material;
            StartCoroutine(DownloadTexture(url, (texture) => { material.mainTexture = texture; }
            ));
        }
    }
}