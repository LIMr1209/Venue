using System;
using UnityEngine;
using UnityEditor;
using Object = System.Object;

namespace DefaultNamespace
{
    public class InitialScene : MonoBehaviour
    {
        // 初始化 加载场景预制体
        // 在Unity3D中Project视窗中创建文件夹：Resources。
        // 将需要动态加载的文件放入其中，例如Texture，Sprite，prefab等等。
        // 在脚本中调用API接口Resources.Load()相关接口即可。
        // 此种方式只能访问Resources文件夹下的资源。
        public string sceneModel = "Models/场馆";

        private void Awake()
        {
            GameObject model = Resources.Load<GameObject>(sceneModel);
            Debug.Log(model.GetInstanceID()); // ID
            // Destroy(model); // 不允许销毁资产以避免数据丢失。如果确实要删除资产，请使用DestroyImmediate
            // Resources.UnloadUnusedAssets(); // 卸载无引用的所有资源  消耗性能
            GameObject newModel = Instantiate(model, new Vector3(1,1,1), Quaternion.identity);
            // Resources.UnloadAsset(model); // 卸载资源 被引用无法卸载
            var tArray = Resources.FindObjectsOfTypeAll(typeof(MeshRenderer ));
            foreach (Object t in tArray)
            {
                MeshRenderer d = t as MeshRenderer;
                d.gameObject.AddComponent<MeshCollider>();
                
            }
            // MeshRenderer[] meshRender = newModel.gameObject.GetComponentsInChildren<MeshRenderer>();
            // foreach (MeshRenderer t in meshRender)
            // {
            //     t.gameObject.AddComponent<MeshCollider>();
            // }
            
            Camera[] cameras = newModel.gameObject.GetComponentsInChildren<Camera>();
            foreach (Camera c in cameras)
            {
                c.enabled = false;
            }
        }
    }
}