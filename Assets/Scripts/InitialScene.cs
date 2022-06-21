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
        private string sceneUrl;

        private void Awake()
        {
            sceneUrl = "https://s3.taihuoniao.com/unity/scene.ab";
        }

        private void Start()
        {
            if (!string.IsNullOrEmpty(sceneUrl))
            {
                StartCoroutine(
                    AbInit.instances.OnWebRequestLoadAssetBundleGameObjectUrl("scene",sceneUrl, (obj) =>
                    {
                        AddController controller = FindObjectOfType<AddController>();
                        controller.AddThird();
                        AbInit.instances.FinishSlider();
                    })); 
            }
            else
            {
                // sceneModel = string.IsNullOrEmpty(Globle.sceneId) ? sceneModel : "scene_" + Globle.sceneId;
                StartCoroutine(
                    AbInit.instances.OnWebRequestLoadAssetBundleGameObject(sceneModel, "", (obj) =>
                    {
                        AddController controller = FindObjectOfType<AddController>();
                        controller.AddThird();
                        AbInit.instances.FinishSlider();
                    })); 
            }
        }


        private void Update()
        {
            
        }
    }
}