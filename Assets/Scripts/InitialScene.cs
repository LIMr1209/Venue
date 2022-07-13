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
            Debug.Log("通知发送场景url");
            Tools.loadScene(); // 通知发送场景url
            enabled = false;  // 默认不启动 前端发送场景url 后启动
#else
            sceneModel = "scene";
            sceneUrl = "https://cdn1.d3ingo.com/model_scene/220704/62c2646573844135b7385a6f/scene.ab";
#endif
        }


        private void Start()
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            StartCoroutine(
                AbInit.instances.OnWebRequestLoadAssetBundleGameObjectUrl("scene", sceneUrl, (obj) =>
                 {
                    Debug.Log(FindObjectOfType<AbInit>() + "3");
                    if (GameObject.Find("default_camera"))
                    {
                        GameObject.Find("default_camera").gameObject.SetActive(false);
                    }
                    Debug.Log(FindObjectOfType<AbInit>() + "8");
                    OnSetLightMap(obj);
                    AfterScene();
                    Debug.Log(FindObjectOfType<AbInit>() + "9");
                    Tools.loadScene();
                }));
#else
            int BeginIndex = sceneUrl.IndexOf("/scene");
            string scenemanifestUrl = sceneUrl.Substring(0, BeginIndex);
            string sceneManifestName = sceneModel + ".ab.manifest";
            StartCoroutine(AbInit.instances.OnWebRequestAssetBundleManifestScene(scenemanifestUrl, sceneManifestName));


            StartCoroutine(
                AbInit.instances.OnWebRequestLoadAssetBundleGameObject(sceneModel, "", (obj) =>
                {
                    if (GameObject.Find("default_camera"))
                    {
                        GameObject.Find("default_camera").gameObject.SetActive(false);
                    }
                    OnSetLightMap(obj);
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
            if(controller) controller.AddThird();
            OpusShow opusShow = FindObjectOfType<OpusShow>();
            if (opusShow) opusShow.enabled = true;


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
            // 可以通过编辑>项目设置>质量找到质量级别列表。您可以添加、删除或编辑这些。
            // int qualityLevel = QualitySettings.GetQualityLevel();
            // QualitySettings.SetQualityLevel (5, true);
            // string[] names = QualitySettings.names;

        }
    }
}