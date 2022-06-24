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
        public string sceneUrl = "https://s3.taihuoniao.com/unity/scene.ab";

        private void Awake()
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            WebGLInput.captureAllKeyboardInput = false;
            enabled = false;  // 默认不启动 前端发送场景url 后启动
#endif
        }

        private void Start()
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            StartCoroutine(
                AbInit.instances.OnWebRequestLoadAssetBundleGameObjectUrl("scene",sceneUrl, (obj) =>
                {
                    AddController controller = FindObjectOfType<AddController>();
                    controller.AddThird();
                    AbInit.instances.FinishSlider();
                })); 
#else
            StartCoroutine(
                AbInit.instances.OnWebRequestLoadAssetBundleGameObject(sceneModel, "", (obj) =>
                {
                    AddController controller = FindObjectOfType<AddController>();
                    controller.AddThird();
                    AbInit.instances.FinishSlider();
                })); 
#endif 
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            /*#if !UNITY_EDITOR && UNITY_WEBGL
            Debug.Log("游戏焦点: "+hasFocus);
            WebGLInput.captureAllKeyboardInput = hasFocus;
            #endif*/
        }


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.N))
            {
                string text =
                    "[{\"name\": \"art_1 (1)\", \"imageUrl\": \"https://p4.taihuoniao.com/art/220611/62a43b2e48d6d7e7e10b09e8-3.png\"}]";
                FindObjectOfType<JsSend>().JsReplaceArtImage(text);
            }
            if (Input.GetKeyDown(KeyCode.M))
            {
                FindObjectOfType<JsSend>().JsFocusArt("art_1 (1)");
            }
        }
    }
}