using System;
using Cinemachine;
using StarterAssets;
using UnityEngine;

namespace DefaultNamespace
{
    public class AddController : MonoBehaviour
    {
        private bool visual = false;// 第一人称 第三人称切换

        private GameObject playerFollowCameraClone;
        private GameObject mainCameraClone;
        private bool disableController = true; // 视角控制器禁用启用
        
        private string firstAsset = "/AssetBundles/" + "firstprefabs.plugin";
        private string thirdAsset = "/AssetBundles/" + "thirdprefabs.plugin"; // //最后一个字符串是AssetBundle的名字

        private AssetBundle firstAssetBundle;  // 第一人称 assetbindle
        private AssetBundle thirdAssetBundle;  // 第三人称 assetbindle
        
        //第二种方式就是使用AssetBundle(AssetBundle是一个资源的集合，可以是Unity3D所支持的任何资源格式)。   

        private void Awake()
        {
            loadAssetBundle();
            mainCameraClone = FindObjectOfType<CinemachineBrain>().gameObject;
            addThird();
        }

        public void loadAssetBundle()
        {
            //从本地加载AssetBundle资源（LoadFromFile）
            firstAssetBundle=AssetBundle.LoadFromFile(Application.dataPath + firstAsset);
            if(firstAssetBundle==null)
            {
                return;
            }
            thirdAssetBundle=AssetBundle.LoadFromFile(Application.dataPath + thirdAsset);
            if(thirdAssetBundle==null)
            {
                return;
            }
        }

        private void Start()
        {
            
        }
        private void Update()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            Vector3 location = player.transform.localPosition;
            Vector3 rotation = player.transform.localRotation.eulerAngles;
            if (Input.GetKeyDown("v"))
            {
                swithVisul(location, rotation);
            }
            // if (Input.GetKeyDown("n"))
            // {
            //     disableController = !disableController;
            //     Cursor.visible = !disableController;
            //     Cursor.lockState=CursorLockMode.None; // 光标行为是不修改
            //     if (player.TryGetComponent<FirstPersonController>(out FirstPersonController first))
            //         first.enabled = disableController;
            //     if (player.TryGetComponent<ThirdPersonController>(out ThirdPersonController third))
            //         third.enabled = disableController;
            // }
        }
        
        public void swithVisul(Vector3 location , Vector3 rotation)
        {
            // firstAssetBundle.Unload(true);  // true 卸载assetBundle和加载的资源  false 卸载 assetBundle
            // thirdAssetBundle.Unload(true);
            // Destroy(GameObject.Find("PlayerFollowCamera"));
            // Destroy(GameObject.FindGameObjectWithTag("Player"));
            // loadAssetBundle();
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject p in players)
            {
                Destroy(p);
            }
            //
            CinemachineVirtualCamera[] playerFollowCameras = FindObjectsOfType<CinemachineVirtualCamera>();
            foreach (CinemachineVirtualCamera p in playerFollowCameras)
            {
                Destroy(p.gameObject);
            }

            if(visual){
                addThird(location, rotation);
                // GameObject playerArmature=thirdAssetBundle.LoadAsset<GameObject>("PlayerArmature.prefab"); //字符串是AssetBundle中资源名称
                // GameObject playerArmatureClone = Instantiate(playerArmature, location, Quaternion.Euler(rotation));
                // CinemachineVirtualCamera virtualCamera = playerFollowCameraClone.GetComponent<CinemachineVirtualCamera>();
                // // Transform cinemachineTarget =
                //     // GameObject.FindGameObjectWithTag("CinemachineTarget").GetComponent<Transform>();
                // Transform cinemachineTarget =
                //     playerArmatureClone.transform.Find("PlayerCameraRoot").GetComponent<Transform>();
                // virtualCamera.Follow = cinemachineTarget;
                visual = false;
            }
            else
            {
                addFirst(location, rotation);
                // GameObject playerCapsule=firstAssetBundle.LoadAsset<GameObject>("PlayerCapsule.prefab"); //字符串是AssetBundle中资源名称
                // GameObject playerCapsuleClone = Instantiate(playerCapsule, location, Quaternion.Euler(rotation));
                // CinemachineVirtualCamera virtualCamera = playerFollowCameraClone.GetComponent<CinemachineVirtualCamera>();
                // // Transform cinemachineTarget =
                //     // GameObject.FindGameObjectWithTag("CinemachineTarget").GetComponent<Transform>();
                // Transform cinemachineTarget =
                //     playerCapsuleClone.transform.Find("PlayerCameraRoot").GetComponent<Transform>();
                // virtualCamera.Follow = cinemachineTarget
                visual = true;
            }
        }

        public void addFirst()
        {
           Vector3 location = new Vector3(-85, -2, 0);
           Vector3 rotation = new Vector3(0, 90, 0);
           addFirst(location, rotation);
        }

        public void addFirst(Vector3 location , Vector3 rotation)
        {
            GameObject playerFollowCamera=firstAssetBundle.LoadAsset<GameObject>("PlayerFollowCamera.prefab"); //字符串是AssetBundle中资源名称
            playerFollowCameraClone = Instantiate(playerFollowCamera);
            // GameObject mainCamera=firstAssetBundle.LoadAsset<GameObject>("MainCamera.prefab"); //字符串是AssetBundle中资源名称
            // mainCameraClone = Instantiate(mainCamera);
            GameObject playerCapsule=firstAssetBundle.LoadAsset<GameObject>("PlayerCapsule.prefab"); //字符串是AssetBundle中资源名称
            GameObject playerCapsuleClone = Instantiate(playerCapsule, location, Quaternion.Euler(rotation));
            CinemachineVirtualCamera virtualCamera = playerFollowCameraClone.GetComponent<CinemachineVirtualCamera>();
            // Transform cinemachineTarget =
            //     GameObject.FindGameObjectWithTag("CinemachineTarget").GetComponent<Transform>();
            Transform cinemachineTarget =
                playerCapsuleClone.transform.Find("PlayerCameraRoot").GetComponent<Transform>();
            virtualCamera.Follow = cinemachineTarget;
        }

        public void addThird()
        {
            Vector3 location = new Vector3(-85, -1.5f, 0);
            Vector3 rotation = new Vector3(0, 90, 0);
            addThird(location, rotation);
        }

        public void addThird(Vector3 location , Vector3 rotation)
        {
            GameObject playerFollowCamera=thirdAssetBundle.LoadAsset<GameObject>("PlayerFollowCamera.prefab"); //字符串是AssetBundle中资源名称
            playerFollowCameraClone = Instantiate(playerFollowCamera);
            // GameObject mainCamera=thirdAssetBundle.LoadAsset<GameObject>("MainCamera.prefab"); //字符串是AssetBundle中资源名称
            // mainCameraClone = Instantiate(mainCamera);
            GameObject playerArmature=thirdAssetBundle.LoadAsset<GameObject>("PlayerArmature.prefab"); //字符串是AssetBundle中资源名称
            GameObject playerArmatureClone = Instantiate(playerArmature, location, Quaternion.Euler(rotation));
            CinemachineVirtualCamera virtualCamera = playerFollowCameraClone.GetComponent<CinemachineVirtualCamera>();
            // Transform cinemachineTarget =
            //     GameObject.FindGameObjectWithTag("CinemachineTarget").GetComponent<Transform>();
            Transform cinemachineTarget =
                playerArmatureClone.transform.Find("PlayerCameraRoot").GetComponent<Transform>();
            virtualCamera.Follow = cinemachineTarget;
            // playerArmatureClone.GetComponent<ThirdPersonController>()._mainCamera = mainCameraClone; // 切换视角时 相机缺失 很奇怪
        }
    }
}