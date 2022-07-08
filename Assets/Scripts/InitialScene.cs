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
        public string sceneModel = "scene";

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
            sceneUrl = "https://cdn1.d3ingo.com/model_scene/220704/62c2646573844135b7385a6f/scene.ab";
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
                    OnSetLightMap(obj);
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
            if (controller) controller.AddThird();
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
            runTimeBakeNavMesh.BakeNav();
            AbInit.instances.FinishSlider();
        }

        public void OnSetLightMap(GameObject obj)
        {
            LightMap lightMap = obj.GetComponent<LightMap>();
            lightMap.OnCreatLightmapTexs(AbInit.instances.AssetBundelLightMapDic.Count);
            foreach (var item in AbInit.instances.AssetBundelLightMapDic)
            {
                if (item.Key.Contains("Lightmap"))
                {
                    Texture2D texture2D = item.Value.LoadAsset<Texture2D>(item.Key);
                    lightMap.OnAddLightmapTexs(texture2D);
                }
            }
            lightMap.i = 0;
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
            _count++;
            _deltaTime += Time.deltaTime;

            if (_count % 60 == 0)
            {
                _count = 1;
                fps = 60f/_deltaTime;
                _deltaTime = 0;
            }
            // if (Input.GetKeyDown(KeyCode.C))
            // {
            //     var tArray = Resources.FindObjectsOfTypeAll(typeof(MeshRenderer ));
            //     for (int i = 0; i < tArray.Length; i++)
            //     {
            //         MeshRenderer t = tArray[i] as MeshRenderer;
            //         //这个很重要，博主发现如果没有这个代码，unity是不会察觉到编辑器有改动的，自然设置完后直接切换场景改变是不被保存
            //         //的  如果不加这个代码  在做完更改后 自己随便手动修改下场景里物体的状态 在保存就好了 
            //         t.gameObject.AddComponent<BoxCollider>();
            //         if (t.gameObject.TryGetComponent<MeshCollider>(out MeshCollider meshCollider))
            //         {
            //             meshCollider.enabled = false;
            //         }
            //
            //     }
            // }

            // 可以通过编辑>项目设置>质量找到质量级别列表。您可以添加、删除或编辑这些。
            // int qualityLevel = QualitySettings.GetQualityLevel();
            // QualitySettings.SetQualityLevel (5, true);
            // string[] names = QualitySettings.names;

            //if (Input.GetKeyDown(KeyCode.N))
            //{
            //    //string text =
            //        //"[{\"name\": \"paintings-022\", \"id\":\"398\", \"imageUrl\": \"https://cdn1.d3ingo.com/scene_rendering/user_fodder/220517/628342319b25fefdacc58282.jpg\"}]";
            //    string text =
            //        "[{\"name\": \"paintings-022\", \"id\":\"398\", \"imageUrl\": \"https://p4.taihuoniao.com/exhibition_cover/220630/d547f9dd55b1714b1656582308823-0.jpg-m4_3.jpg\",\"quaternion\":[0.7071067811865476,0,0.7071067811865475,0],\"location\":[-0.0036220550537109375,0.003346681594848633,-0.00887298583984375],\"scale\":[0.09148947149515152,0.09148947149515152,0.12227702528948403],\"rotate\":[0,1.5707963267948966,0]}]";
            //    FindObjectOfType<JsSend>().JsReplaceArtImage(text);
            //}
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