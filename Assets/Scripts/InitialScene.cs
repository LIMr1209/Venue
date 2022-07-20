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
            Debug.Log("通知发送场景url");
            Tools.loadScene(); // 通知发送场景url
            enabled = false;  // 默认不启动 前端发送场景url 后启动
#else
            sceneModel = "scene";
            //sceneUrl = "https://cdn1.d3ingo.com/model_scene/220704/62c2646573844135b7385a6f/scene.ab";
            sceneUrl = "https://s3.taihuoniao.com/unity/scene.ab";
        
#endif
        }


        private void Start()
        {
            int BeginIndex = sceneUrl.IndexOf("/scene");
            string scenemanifestUrl = sceneUrl.Substring(0, BeginIndex);
            string sceneManifestName = sceneModel + ".ab.manifest";
            StartCoroutine(AbInit.instances.OnWebRequestAssetBundleManifestScene(scenemanifestUrl, sceneManifestName));


#if !UNITY_EDITOR && UNITY_WEBGL
            StartCoroutine(
                AbInit.instances.OnWebRequestLoadAssetBundleGameObjectUrl("scene", sceneUrl, true, (obj) =>
                 {
                    if (GameObject.Find("default_camera"))
                    {
                        GameObject.Find("default_camera").gameObject.SetActive(false);
                    }
                    OnSetLightMap(obj);
                    AfterScene();
                    Tools.loadScene();
                }));
#else

            StartCoroutine(
                AbInit.instances.OnWebRequestLoadAssetBundleGameObjectUrl(sceneModel, sceneUrl, false, (obj) =>
                 {
                     if (GameObject.Find("Camera"))
                     {
                         GameObject.Find("Camera").gameObject.SetActive(false);
                     }
                     OnSetLightMap(obj);
                     AfterScene();
                 }));
#endif



            StartCoroutine(
                AbInit.instances.OnWebRequestLoadAssetBundleMaterial("skybox_03", "", (material) =>
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
            if(controller) StartCoroutine(controller.AddThird());
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
            if (!lightMap) return;
            lightMap.OnCreatLightmapTexs(AbInit.instances.AssetBundelLightMapDic.Count);
            Debug.Log("OnSetLightMap"+AbInit.instances.AssetBundelLightMapDic.Count);
            foreach (var item in AbInit.instances.AssetBundelLightMapDic)
            {
                if (item.Key.Contains("lightmap"))
                {
                    int BeginIndex = item.Key.IndexOf("/")+1;
                    int LastIndex = item.Key.IndexOf(".");
                    int len = LastIndex - BeginIndex;
                    string bundleName = item.Key.Substring(BeginIndex, len);
                    Texture2D texture2D = item.Value.LoadAsset<Texture2D>(bundleName);
                    lightMap.OnAddLightmapTexs(texture2D);
                    Debug.Log(texture2D.name + 44);
                }
            }
            lightMap.i = 0;
            lightMap.OnLoadLightmap();
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

            if (Input.GetKeyDown("x"))
            {
                string text = "[{\"id\":0,\"imageUrl\":\"https://cdn1.d3ingo.com/scene_rendering/user_fodder/220715/62d1411a11197d1c8d8f500a.mp4\",\"name\":\"showcase-029\",\"location\":[0,0,0],\"scaleS\":1,\"rotateS\":0,\"isDel\":false,\"nKind\":3,\"kind\":2,\"cloneBase\":\"\"},{\"id\":1,\"imageUrl\":\"https://cdn1.d3ingo.com/scene_rendering/user_fodder/220715/62d1411a11197d1c8d8f500a.mp4\",\"name\":\"showcase-001\",\"location\":[0,0,0],\"scaleS\":1,\"rotateS\":0,\"isDel\":false,\"nKind\":3,\"kind\":2,\"cloneBase\":\"\"}]";
                FindObjectOfType<JsSend>().JsReplaceArtImage(text);
            }
            // 可以通过编辑>项目设置>质量找到质量级别列表。您可以添加、删除或编辑这些。
            // int qualityLevel = QualitySettings.GetQualityLevel();
            // QualitySettings.SetQualityLevel (5, true);
            // string[] names = QualitySettings.names;

        }
    }
}