using System;
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
        [HideInInspector]
        public string sceneModel = "scene";
        
        [HideInInspector]
        public string sceneUrl = "https://cdn1.d3ingo.com/model_scene/220629/62b96fe7baae4131bab41cd1/scene.ab";

        private void Awake()
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            WebGLInput.captureAllKeyboardInput = false;
            enabled = false;  // 默认不启动 前端发送场景url 后启动
            Debug.Log("通知发送场景url");
            Tools.loadScene(); // 通知发送场景url
#endif
        }

        private void Start()
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            StartCoroutine(
                AbInit.instances.OnWebRequestLoadAssetBundleGameObjectUrl("scene",sceneUrl, (obj) =>
                {
                    AddController controller = FindObjectOfType<AddController>();
                    if (controller) controller.AddThird();
                    OpusShow opusShow = FindObjectOfType<OpusShow>();
                    if (opusShow) opusShow.enabled = true;
                    AbInit.instances.FinishSlider();
                    Light[] lights = FindObjectsOfType<Light>();
                    foreach (Light i in lights)
                    {
                        if (i.gameObject.name != "Directional Light")
                        {
                            i.gameObject.SetActive(false);
                        }
                    }
                    Tools.loadScene();
                })); 
#else
            StartCoroutine(
                AbInit.instances.OnWebRequestLoadAssetBundleGameObjectUrl(sceneModel, sceneUrl, (obj) =>
                {
                    AddController controller = FindObjectOfType<AddController>();
                    if (controller) controller.AddThird();
                    OpusShow opusShow = FindObjectOfType<OpusShow>();
                    if (opusShow) opusShow.enabled = true;
                    AbInit.instances.FinishSlider();
                    
                    Light[] lights = FindObjectsOfType<Light>();
                    foreach (Light i in lights)
                    {
                        if (i.gameObject.name != "Directional Light")
                        {
                            i.gameObject.SetActive(false);
                        }
                    }
                })); 
#endif             
                // StartCoroutine(AbInit.instances.DownloadTexture("https://s3.taihuoniao.com/unity/photo_studio_01_1k.hdr", (texture) =>
                // {
                //     Material material = new Material("hdri");
                //     Shader shader1 = Shader.Find("Skybox/Panoramic");
                //     material.shader = shader1;
                //     material.mainTexture = texture;
                //     RenderSettings.skybox = material;
                //     DynamicGI.UpdateEnvironment();
                // }
                // ));
            
            StartCoroutine(
                AbInit.instances.OnWebRequestLoadAssetBundleMaterial("skybox_01", "", (material) =>
                {
                    Shader shader1 = Shader.Find("Skybox/Panoramic");
                    material.shader = shader1;
                    RenderSettings.skybox = material;
                    DynamicGI.UpdateEnvironment();
                })); 
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
            // 可以通过编辑>项目设置>质量找到质量级别列表。您可以添加、删除或编辑这些。
            // int qualityLevel = QualitySettings.GetQualityLevel();
            // QualitySettings.SetQualityLevel (5, true);
            // string[] names = QualitySettings.names;
            // if (Input.GetKeyDown(KeyCode.N))
            // {
            //     string text =
            //         "[{\"name\": \"paintings-022\", \"id\":\"398\", \"imageUrl\": \"https://cdn1.d3ingo.com/scene_rendering/user_fodder/220517/628342319b25fefdacc58282.jpg\"}]";
            //     FindObjectOfType<JsSend>().JsReplaceArtImage(text);
            // }
            // if (Input.GetKeyDown(KeyCode.M))
            // {
            //     Tools.showFocusWindow(Convert.ToString(398));
            // }
            // if (Input.GetKeyDown(KeyCode.B))
            // {
            //     Tools.showFocusWindow("test");
            // }
        }
    }
}