using System.Collections;
using System.IO;
using Cinemachine;
using UnityEngine;
using UnityEngine.Networking;

namespace DefaultNamespace
{
    public class AddController : MonoBehaviour
    {
        private bool visual = false;// 第一人称 第三人称切换

        private GameObject playerFollowCameraClone;
        private GameObject mainCameraClone;
        private bool disableController = true; // 视角控制器禁用启用
        private bool hasController = false;

        private AssetBundle firstAssetBundle;  // 第一人称 assetbindle
        private AssetBundle thirdAssetBundle;  // 第三人称 assetbindle
        
        //第二种方式就是使用AssetBundle(AssetBundle是一个资源的集合，可以是Unity3D所支持的任何资源格式)。   

        private void Awake()
        {
            StartCoroutine(loadAssetBundle());
            mainCameraClone = FindObjectOfType<CinemachineBrain>().gameObject;
        }

        IEnumerator loadAssetBundle()
        {
             // 从本地加载AssetBundle资源（LoadFromFile）
             string firstPath = Path.Combine(Application.dataPath, Globle.AssetBundleDir, Globle.FirstAssetBundle);
             string thirdPath = Path.Combine(Application.dataPath, Globle.AssetBundleDir, Globle.ThirdAssetBundle);
             firstAssetBundle=AssetBundle.LoadFromFile(firstPath);
             if(firstAssetBundle==null)
             {
                 yield return null;
             }
             thirdAssetBundle=AssetBundle.LoadFromFile(thirdPath);
             if(thirdAssetBundle==null)
             {
                 yield return null;
             }
            // string firstUri = "http://127.0.0.1:8000/StreamingAssets/"+Globle.FirstAssetBundle;
            // UnityWebRequest firstRequest = UnityWebRequestAssetBundle.GetAssetBundle(firstUri); // 获取assetBundle
            // yield return firstRequest.SendWebRequest();
            //
            // if (firstRequest.result != UnityWebRequest.Result.Success)
            // {
            //     Debug.Log(firstRequest.error);
            // }
            // else
            // {
            //     firstAssetBundle = DownloadHandlerAssetBundle.GetContent(firstRequest);
            //     Debug.Log(firstAssetBundle);
            // }
            // string thirdUri = "http://127.0.0.1:8000/StreamingAssets/"+Globle.ThirdAssetBundle;
            // UnityWebRequest thirdRequest = UnityWebRequestAssetBundle.GetAssetBundle(thirdUri); // 获取assetBundle
            // yield return thirdRequest.SendWebRequest();
            //
            // if (thirdRequest.result != UnityWebRequest.Result.Success)
            // {
            //     Debug.Log(thirdRequest.error);
            // }
            // else
            // {
            //     thirdAssetBundle = DownloadHandlerAssetBundle.GetContent(thirdRequest);
            //     Debug.Log(thirdAssetBundle);
            // }
        }

        private void Start()
        {
            
        }
        private void Update()
        {
            if (!hasController)
            {
                addThird();
            }
            else
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
            if (firstAssetBundle)
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
                hasController = true;
            }
        }

        public void addThird()
        {
            Vector3 location = new Vector3(-85, -1.5f, 0);
            Vector3 rotation = new Vector3(0, 90, 0);
            addThird(location, rotation);
        }

        public void addThird(Vector3 location , Vector3 rotation)
        {
            if (thirdAssetBundle)
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
                hasController = true;
                // playerArmatureClone.GetComponent<ThirdPersonController>()._mainCamera = mainCameraClone; // 切换视角时 相机缺失 很奇怪
            }
        }
    }
}