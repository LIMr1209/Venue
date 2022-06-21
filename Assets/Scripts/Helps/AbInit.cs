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

        public bool isLoad;
        
        float time = 0;
        float lodingindex = 0;
        
        Transform root;
        
        Slider slider;


        private void Awake()
        {
            instances = this;
        }


        private void Start()
        {
            slider = transform.Find("Mask/Slider").GetComponent<Slider>();
            root = transform.Find("Root");
            AssetBundle.UnloadAllAssetBundles(true);
        }


        private void Update()
        {
            OnLoadingIndex();
        }
        
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

        public Dictionary<string, AssetBundle> AssetBundelGameObjectDic = new Dictionary<string, AssetBundle>();

        public IEnumerator OnWebRequestLoadAssetBundleGameObject(string name, string parent = "")
        {
            Vector3 point = Vector3.zero;
            ;
            Vector3 rotate = new Vector3(0, 0, 0);
            yield return StartCoroutine(OnWebRequestLoadAssetBundleGameObject(name, parent, point, rotate));
        }

        public IEnumerator OnWebRequestLoadAssetBundleGameObject(string name, string parent,
            GameObjectCallback callback)
        {
            Vector3 point = Vector3.zero;
            ;
            Vector3 rotate = new Vector3(0, 0, 0);
            yield return StartCoroutine(OnWebRequestLoadAssetBundleGameObject(name, parent, point, rotate, callback));
        }

        public delegate void GameObjectCallback(GameObject obj);

        public IEnumerator OnWebRequestLoadAssetBundleGameObject(string name, string parent, Vector3 point,
            Vector3 rotate, GameObjectCallback callback = null)
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
    }
}