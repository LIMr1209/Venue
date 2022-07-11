using System.IO;
using UnityEngine;
using UnityEditor;
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
        public string sceneModel;

        [HideInInspector]
        public string sceneUrl;
        private float _deltaTime;
        private int _count;
        public float fps;

        private void Awake()
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            WebGLInput.captureAllKeyboardInput = false;
            enabled = false;  // 默认不启动 前端发送场景url 后启动
            Debug.Log("通知发送场景url");
            Tools.loadScene(); // 通知发送场景url
#else
            sceneModel = "scene";
            // sceneUrl = "https://cdn1.d3ingo.com/model_scene/220704/62c2646573844135b7385a6f/scene.ab";
            sceneUrl = "https://cdn1.d3ingo.com/model_scene/220630/62bda0ea5e8137d4fabd5c11/scene.ab";
#endif
        }

        private void Start()
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            StartCoroutine(
                AbInit.instances.OnWebRequestLoadAssetBundleGameObjectUrl("scene",sceneUrl, (obj) =>
                {
                    
                    AfterScene();
                    Tools.loadScene();
                })); 
#else

            StartCoroutine(
                AbInit.instances.OnWebRequestLoadAssetBundleGameObject(sceneModel, "", (obj) =>
                {
                    if (GameObject.Find("default_camera"))
                    {
                        GameObject.Find("default_camera").gameObject.SetActive(false);
                    }
                    //OnSetLightMap(obj);
                    Debug.Log(AbInit.instances.AssetBundelLightMapDic.Count);
                    AfterScene();
                })); 
#endif             
            
            StartCoroutine(
                AbInit.instances.OnWebRequestLoadAssetBundleMaterial("skybox_01", "", (material) =>
                {
                    Shader shader1 = Shader.Find("Skybox/Panoramic");
                    material.shader = shader1;
                    RenderSettings.skybox = material;
                    DynamicGI.UpdateEnvironment();
                })); 
        }

        public void AfterScene()
        {
            AddController controller = FindObjectOfType<AddController>();
            controller.enabled = true;
            Light[] lights = FindObjectsOfType<Light>();
            foreach (Light i in lights)
            {
                if (i.gameObject.name != "Directional Light")
                {
                    i.gameObject.SetActive(false);
                }
            }

            RunTimeBakeNavMesh runTimeBakeNavMesh = FindObjectOfType<RunTimeBakeNavMesh>();
            runTimeBakeNavMesh.BakeNav(); // 动态烘培导航区域
            AbInit.instances.FinishSlider();
        }

        public void OnSetLightMap(GameObject obj)
        {
            LightMap lightMap = obj.GetComponent<LightMap>();
            lightMap.OnCreatLightmapTexs(AbInit.instances.AssetBundelLightMapDic.Count);
            foreach (var item in AbInit.instances.AssetBundelLightMapDic)
            {
                if (item.Key.Contains("lightmap"))
                {
                    int BeginIndex = item.Key.IndexOf("/")+1;
                    int LastIndex = item.Key.IndexOf(".");
                    int len = LastIndex - BeginIndex;
                    string bundleName = item.Key.Substring(BeginIndex, len);
                    Debug.Log(bundleName);
                    Texture2D texture2D = item.Value.LoadAsset<Texture2D>(bundleName);
                    lightMap.OnAddLightmapTexs(texture2D);
                }
            }
            lightMap.i = 0;

        }


        private void Update()
        {
            _count++;
            _deltaTime += Time.deltaTime;

            if (_count % 60 == 0)
            {
                _count = 1;
                fps = 60f/_deltaTime;
                _deltaTime = 0;
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                FindObjectOfType<JsSend>().JsLockArt("showcase-013");
            }
            if (Input.GetKeyDown(KeyCode.Z))
            {
                FindObjectOfType<JsSend>().JsUnlockArt("showcase-013");
            }
            // 可以通过编辑>项目设置>质量找到质量级别列表。您可以添加、删除或编辑这些。
            // int qualityLevel = QualitySettings.GetQualityLevel();
            // QualitySettings.SetQualityLevel (5, true);
            // string[] names = QualitySettings.names;

        }
    }
}