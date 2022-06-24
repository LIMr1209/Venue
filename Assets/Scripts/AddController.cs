using Cinemachine;
using UnityEngine;

namespace DefaultNamespace
{
    public class AddController : MonoBehaviour
    {
        private bool visual = false; // 第一人称 第三人称切换

        private GameObject playerFollowCameraClone;

        // private bool hasController = false;
        private string controllerAb = "controller";
        private string firstFollowCameraAb = "firstplayerfollowcamera";

        private string thirdFollowCameraAb = "thirdplayerfollowcamera";

        // private string capsuleAb = "playercapsule";
        private string armatureAb = "playerarmature";
        private GameObject _firstPlayerFollowCamera = null;
        private GameObject _thirdPlayerFollowCamera = null;
        private GameObject _player = null;
        public float zoomSpeed = 10.0f;

        private void Start()
        {
            // AddThird();
        }

        private void Awake()
        {
            StartCoroutine(
                AbInit.instances.OnWebRequestLoadAssetBundleGameObject(firstFollowCameraAb, controllerAb, (obj) =>
                    {
                        _firstPlayerFollowCamera = obj;
                        _firstPlayerFollowCamera.SetActive(false);
                    }
                ));
            StartCoroutine(
                AbInit.instances.OnWebRequestLoadAssetBundleGameObject(thirdFollowCameraAb, controllerAb, (obj) =>
                    {
                        _thirdPlayerFollowCamera = obj;
                        _thirdPlayerFollowCamera.SetActive(false);
                    }
                ));
            StartCoroutine(
                AbInit.instances.OnWebRequestLoadAssetBundleGameObject(armatureAb, controllerAb, (obj) =>
                    {
                        _player = obj;
                        _player.SetActive(false);
                    }
                ));
        }

        private void Update()
        {
            // if (!hasController)
            // {
            //     AddThird();
            // }
            if (Input.GetKeyDown("v"))
            {
                if (_player)
                {
                    Vector3 location = _player.transform.localPosition;
                    Vector3 rotation = _player.transform.localRotation.eulerAngles;
                    SwithVisul(location, rotation);
                }
            }

            // float scroll = Input.GetAxis("Mouse ScrollWheel");
            // Debug.Log(_thirdPlayerFollowCamera.GetComponent<CinemachineVirtualCamera>()
            //     .GetCinemachineComponent<Cinemachine3rdPersonFollow>().ShoulderOffset);
            // _thirdPlayerFollowCamera.GetComponent<CinemachineVirtualCamera>()
            //         .GetCinemachineComponent<Cinemachine3rdPersonFollow>().ShoulderOffset +=
            //     _player.transform.forward * scroll * zoomSpeed;
        }

        private void SwithVisul(Vector3 location, Vector3 rotation)
        {
            if (visual)
            {
                _firstPlayerFollowCamera.SetActive(false);
                AddThird(location, rotation);
                visual = false;
            }
            else
            {
                _thirdPlayerFollowCamera.SetActive(false);
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
            if (_firstPlayerFollowCamera)
            {
                _firstPlayerFollowCamera.SetActive(true);
                
            }

            if (_player)
            {
                _player.SetActive(true);
                Transform cinemachineTarget;
                cinemachineTarget =
                    _player.transform.Find("PlayerCameraRoot").GetComponent<Transform>();
                _firstPlayerFollowCamera.GetComponent<CinemachineVirtualCamera>().Follow = cinemachineTarget;
                _player.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
            }
        }

        public void AddThird()
        {
            // Vector3 location = new Vector3(-85, -1.5f, 0);
            // Vector3 rotation = new Vector3(0, 90, 0);
            Vector3 location = Vector3.zero;
            Vector3 rotation = Vector3.zero;
            AddThird(location, rotation);
        }

        public void AddThird(Vector3 location, Vector3 rotation)
        {
            if (_thirdPlayerFollowCamera)
            {
                _thirdPlayerFollowCamera.SetActive(true);
            }

            if (_player)
            {
                _player.SetActive(true);
                Transform cinemachineTarget;
                cinemachineTarget =
                    _player.transform.Find("PlayerCameraRoot").GetComponent<Transform>();
                _thirdPlayerFollowCamera.GetComponent<CinemachineVirtualCamera>().Follow = cinemachineTarget;
                _player.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
                // hasController = true;
            }
        }
    }
}