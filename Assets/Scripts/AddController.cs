using Cinemachine;
using UnityEngine;

namespace DefaultNamespace
{
    public class AddController : MonoBehaviour
    {
        private bool visual = false; // 第一人称 第三人称切换
        private GameObject playerFollowCameraClone;
        private bool disableController = true; // 视角控制器禁用启用
        private string controllerAb = "controller";
        private string firstFollowCameraAb = "firstplayerfollowcamera";
        private string thirdFollowCameraAb = "thirdplayerfollowcamera";
        private string capsuleAb = "playercapsule";
        private string armatureAb = "playerarmature";
        private GameObject _firstPlayerFollowCamera = null;
        private GameObject _thirdPlayerFollowCamera = null;
        // private GameObject _firstPlayer = null;
        // private GameObject _thirdPlayer = null;
        private GameObject _player = null;
        
        private void Start()
        {
            AddThird();
        }

        private void Update()
        {
            if (Input.GetKeyDown("v"))
            {
                GameObject[] player = GameObject.FindGameObjectsWithTag("Player");
                if (player[0])
                {
                    Vector3 location = player[0].transform.localPosition;
                    Vector3 rotation = player[0].transform.localRotation.eulerAngles;
                    SwithVisul(location, rotation);
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

        private void SwithVisul(Vector3 location, Vector3 rotation)
        {
            if (visual)
            {
                _firstPlayerFollowCamera.SetActive(false);
                // _firstPlayer.SetActive(false);
                AddThird(location, rotation);
                visual = false;
            }
            else
            {
                _thirdPlayerFollowCamera.SetActive(false);
                // _thirdPlayer.SetActive(false);
                AddFirst(location, rotation);
                visual = true;
            }
        }

        public void AddFirst()
        {
            // Vector3 location = new Vector3(-85, -2, 0);
            // Vector3 rotation = new Vector3(0, 90, 0);
            Vector3 location = Vector3.zero;
            Vector3 rotation = Vector3.zero;
            AddFirst(location, rotation);
        }

        private void AddFirst(Vector3 location, Vector3 rotation)
        {
            if (!_firstPlayerFollowCamera)
            {
                StartCoroutine(
                    GameManager.instances.OnWebRequestLoadAssetBundleGameObject(firstFollowCameraAb, controllerAb, location,
                        rotation, gameObject, "AddFollow"));
            }
            else
            {
                _firstPlayerFollowCamera.SetActive(true);
            }

            /*if (!_firstPlayer)
            {
                StartCoroutine(
                    GameManager.instances.OnWebRequestLoadAssetBundleGameObject(capsuleAb, controllerAb, location,
                        rotation, false, gameObject, "AddFollow"));
            }
            else
            {
                _firstPlayer.SetActive(true);
                _firstPlayer.transform.position = location;
                _firstPlayer.transform.rotation = Quaternion.Euler(rotation);
            }*/
            if (!_player)
            {
                StartCoroutine(
                    GameManager.instances.OnWebRequestLoadAssetBundleGameObject(armatureAb, controllerAb, location,
                        rotation, gameObject, "AddFollow"));
            }
            else
            {
                _player.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
            }
            // else
            // {
            //     _player.transform.position = location;
            //     _player.transform.rotation = Quaternion.Euler(rotation);
            // }
        }

        private void AddThird()
        {
            // Vector3 location = new Vector3(-85, -1.5f, 0);
            // Vector3 rotation = new Vector3(0, 90, 0);
            Vector3 location = Vector3.zero;
            Vector3 rotation = Vector3.zero;
            AddThird(location, rotation);
        }

        private void AddThird(Vector3 location, Vector3 rotation)
        {
            if (!_thirdPlayerFollowCamera)
            {
                StartCoroutine(
                    GameManager.instances.OnWebRequestLoadAssetBundleGameObject(thirdFollowCameraAb, controllerAb));
            }
            else
            {
                _thirdPlayerFollowCamera.SetActive(true);
            }

            /*if (!_thirdPlayer)
            {
                StartCoroutine(
                    GameManager.instances.OnWebRequestLoadAssetBundleGameObject(armatureAb, controllerAb, location,
                        rotation, false, gameObject, "AddFollow"));
            }
            else
            {
                _thirdPlayer.SetActive(true);
                _thirdPlayer.transform.position = location;
                _thirdPlayer.transform.rotation = Quaternion.Euler(rotation);
            }*/
            if (!_player)
            {
                StartCoroutine(
                    GameManager.instances.OnWebRequestLoadAssetBundleGameObject(armatureAb, controllerAb, location,
                        rotation, gameObject, "AddFollow"));
            }
            else
            {
                _player.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
            }
            // else
            // {
            //     _player.transform.position = location;
            //     _player.transform.rotation = Quaternion.Euler(rotation);
            // }
        }

        public void AddFollow()
        {
            Transform cinemachineTarget = null;
            _player = GameObject.FindGameObjectWithTag("Player");
            if (!visual)
            {
                _thirdPlayerFollowCamera = GameObject.Find("ThirdPlayerFollowCamera(Clone)");
                // _thirdPlayer = GameObject.Find("PlayerArmature(Clone)");
                cinemachineTarget =
                    _player.transform.Find("PlayerCameraRoot").GetComponent<Transform>();
                _thirdPlayerFollowCamera.GetComponent<CinemachineVirtualCamera>().Follow = cinemachineTarget;
                _player.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
            }
            else
            {
                _firstPlayerFollowCamera = GameObject.Find("FirstPlayerFollowCamera(Clone)");
                // _firstPlayer = GameObject.Find("PlayerCapsule(Clone)");
                cinemachineTarget =
                    _player.transform.Find("PlayerCameraRoot").GetComponent<Transform>();
                _firstPlayerFollowCamera.GetComponent<CinemachineVirtualCamera>().Follow = cinemachineTarget;
                _player.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
            }
            
            
        }
    }
}