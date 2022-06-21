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
            // int width = Tools.GetWindowWidth();
            // int height = Tools.GetWindowHeight();
            // Screen.SetResolution(width, height, false);
        }

        private void Start()
        {
            // sceneModel = string.IsNullOrEmpty(Globle.sceneId) ? sceneModel : "scene_" + Globle.sceneId;
            StartCoroutine(
                AbInit.instances.OnWebRequestLoadAssetBundleGameObject(sceneModel, "", (obj) =>
                {
                    AddController controller = FindObjectOfType<AddController>();
                    controller.AddThird();
                }));
        }


        private void Update()
        {
        }
    }
}