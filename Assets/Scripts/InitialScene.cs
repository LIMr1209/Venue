using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class InitialScene : MonoBehaviour
    {
        // 初始化 加载场景预制体
        // 在Unity3D中Project视窗中创建文件夹：Resources。
        // 将需要动态加载的文件放入其中，例如Texture，Sprite，prefab等等。
        // 在脚本中调用API接口Resources.Load()相关接口即可。
        // 此种方式只能访问Resources文件夹下的资源。
        public string sceneModel = "scene";

        private void Awake()
        {
            if (!Application.isEditor)
            {
                int sceneId = Tools.GetSceneId();
                Debug.Log("场景id: "+sceneId);
                string token = Tools.GetToken();
                Debug.Log("用户token: "+token);
                if (!string.IsNullOrEmpty(token))
                {
                    Dictionary<string, string> requestData = new Dictionary<string, string>();
                    requestData["token"] = token;
                    Request.instances.HttpSend(1, "get", requestData, (statusCode, error, body) =>
                    {
                        Debug.Log("statusCode: " + statusCode);
                        Debug.Log("error: " + error);
                        JsonData.UserResult userResult = new JsonData.UserResult();
                        userResult = JsonUtility.FromJson<JsonData.UserResult>(body);
                    });
                }
            }
        }

        
        private void Start()
        {
            StartCoroutine(
                GameManager.instances.OnWebRequestLoadAssetBundleGameObject(sceneModel,"", gameObject, "AddThird" ));
        }

        private void Update()
        {
            if (!Application.isEditor)
            {
                int width = Tools.GetWindowWidth();
                int height = Tools.GetWindowHeight();
                Tools.ResetCanvasSize(width, height);
                Screen.SetResolution(width, height, false);
            }

        }
    }
}