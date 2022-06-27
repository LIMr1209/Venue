using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class AbInit : MonoBehaviour
    {
        public static AbInit instances;

        float time = 0;
        float lodingindex = 0;

        Slider slider;


        private void Awake()
        {
            instances = this;
#if !UNITY_EDITOR && UNITY_WEBGL
            enabled = false; // 默认不启动 前端发送场景url 后启动
#endif
        }


        private void Start()
        {
            slider = transform.Find("Mask/Slider").GetComponent<Slider>();
            // AssetBundle.UnloadAllAssetBundles(true);
        }


        private void Update()
        {
            OnLoadingIndex();
        }

        private void OnLoadingIndex()
        {
            if (Time.time - time > 0.1f)
            {
                if (lodingindex <= 1)
                {
                    slider.value = lodingindex;
                    lodingindex += 0.007f;

                    time = Time.time;
                }
            }
        }

        public void FinishSlider()
        {
            lodingindex = 1;
            slider.value = lodingindex;
            transform.Find("Mask").gameObject.SetActive(false);
        }

        public Dictionary<string, AssetBundle> AssetBundelGameObjectDic = new Dictionary<string, AssetBundle>();

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
                    Debug.LogError(requestAB.error);
                    yield break;
                }

                // AB = DownloadHandlerAssetBundle.GetContent(requestAB); 
                byte[] abData = requestAB.downloadHandler.data;
#if !UNITY_EDITOR && UNITY_WEBGL
                abData = Aes.AESDecrypt(abData, Globle.AesKey, Globle.AesIv);
#endif
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
                }

                AB.UnloadAsync(false);
            }
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
            AssetBundle AB = null;

             UnityWebRequest requestAB = UnityWebRequestAssetBundle.GetAssetBundle(url);
            //UnityWebRequest requestAB = UnityWebRequest.Get(url);
            yield return requestAB.SendWebRequest();
            if (!string.IsNullOrEmpty(requestAB.error))
            {
                    Debug.LogError(requestAB.error);
                yield break;
            }

             AB = DownloadHandlerAssetBundle.GetContent(requestAB);
            while (AB == null)
            {

            }
            //byte[] abData = requestAB.downloadHandler.data;
            //abData = Aes.AESDecrypt(abData, Globle.AesKey, Globle.AesIv);

            //AB = AssetBundle.LoadFromMemory(abData);

            if (AB != null)
            {
                GameObject obj = Instantiate(AB.LoadAsset<GameObject>(name), point, Quaternion.Euler(rotate));
                if (callback != null)
                {
                    callback(obj);
                }

                AB.UnloadAsync(false);
            }
        }

        public delegate void TextureCallback(Texture obj);

        public IEnumerator DownloadTexture(string url, TextureCallback callback = null)
        {
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
            yield return www.SendWebRequest();
            if (!string.IsNullOrEmpty(www.error))
            {
                Debug.LogError(www.error);
                yield break;
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
            StartCoroutine(DownloadTexture(url, (texture) =>
                {
                    material.mainTexture = texture;
                }
            ));
        }
    }
}