using Cinemachine;
using UnityEngine;

namespace DefaultNamespace
{
    public class AddController : MonoBehaviour
    {
        private bool visual = false; // 第一人称 第三人称切换

        // private bool hasController = false;
        private string controllerAb = "controller";
        // private string firstFollowCameraAb = "firstplayerfollowcamera";

        private string thirdFollowCameraAb = "playerfollowcamera";

        // private string capsuleAb = "playercapsule";
        // private string armatureAb = "playerarmature";
        private string armatureAb = "figure01";

        // private GameObject _firstPlayerFollowCamera;
        // private GameObject _thirdPlayerFollowCamera;
        private GameObject _playerFollowCamera;
        private CinemachineVirtualCamera _cinemachineVirtualCamera;
        private Cinemachine3rdPersonFollow _playerFollowCamera3rdBody;
        private GameObject _player;
        private Transform cinemachineTarget;
        public float zoomSpeed = 10.0f;

        private void Start()
        {
            // AddThird();
        }

        private void Awake()
        {
            // StartCoroutine(
            //     AbInit.instances.OnWebRequestLoadAssetBundleGameObject(firstFollowCameraAb, controllerAb, (obj) =>
            //         {
            //             _firstPlayerFollowCamera = obj;
            //             _firstPlayerFollowCamera.SetActive(false);
            //         }
            //     ));
            // StartCoroutine(
            //     AbInit.instances.OnWebRequestLoadAssetBundleGameObject(thirdFollowCameraAb, controllerAb, (obj) =>
            //         {
            //             _thirdPlayerFollowCamera = obj;
            //             _thirdPlayerFollowCamera.SetActive(false);
            //         }
            //     ));
            StartCoroutine(
                AbInit.instances.OnWebRequestLoadAssetBundleGameObject(thirdFollowCameraAb, controllerAb, (obj) =>
                    {
                        _playerFollowCamera = obj;
                        // _playerFollowCamera.SetActive(false);
                        _cinemachineVirtualCamera = _playerFollowCamera.GetComponent<CinemachineVirtualCamera>();
                        _playerFollowCamera3rdBody = _playerFollowCamera.GetComponent<CinemachineVirtualCamera>()
                            .GetCinemachineComponent<Cinemachine3rdPersonFollow>();
                    }
                ));
            StartCoroutine(
                AbInit.instances.OnWebRequestLoadAssetBundleGameObject(armatureAb, controllerAb, new Vector3(0,0,0) ,new Vector3(0,-180,0),(obj) =>
                    {
                        _player = obj;
                        _player.SetActive(false);
                        ShaderProblem.ResetShader(_player); // 解决shader问题
                        cinemachineTarget =
                            _player.transform.Find("PlayerCameraRoot").GetComponent<Transform>();
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
            // Debug.Log(_playerFollowCamera3rdBody.ShoulderOffset);
            // _playerFollowCamera3rdBody.ShoulderOffset += _player.transform.forward * scroll * zoomSpeed;
        }

        private void SwithVisul(Vector3 location, Vector3 rotation)
        {
            // if (visual)
            // {
            //     _firstPlayerFollowCamera.SetActive(false);
            //     AddThird(location, rotation);
            //     visual = false;
            // }
            // else
            // {
            //     _thirdPlayerFollowCamera.SetActive(false);
            //     AddFirst(location, rotation);
            //     visual = true;
            // }
            if (visual)
            {
                AddThird(location, rotation);
            }
            else
            {
                AddFirst(location, rotation);
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
            // if (_firstPlayerFollowCamera)
            // {
            //     _firstPlayerFollowCamera.SetActive(true);
            //     
            // }

            if (_player)
            {
                _player.SetActive(true);
                _cinemachineVirtualCamera.Follow = cinemachineTarget;
                // _firstPlayerFollowCamera.GetComponent<CinemachineVirtualCamera>().Follow = cinemachineTarget;
                _player.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
                visual = true;
                SetFollowCameraBody();
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
            // if (_thirdPlayerFollowCamera)
            // {
            //     _thirdPlayerFollowCamera.SetActive(true);
            // }

            if (_player)
            {
                _player.SetActive(true);
                _cinemachineVirtualCamera.Follow = cinemachineTarget;
                // _thirdPlayerFollowCamera.GetComponent<CinemachineVirtualCamera>().Follow = cinemachineTarget;
                _player.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
                // hasController = true;
                visual = false;
                SetFollowCameraBody();
            }
        }

        public void SetFollowCameraBody()
        {
            if (visual)
            {
                _playerFollowCamera3rdBody.Damping = new Vector3(0.0f, 0.0f, 0.0f);
                _playerFollowCamera3rdBody.ShoulderOffset = new Vector3(0.0f, 0.0f, 0.0f);
                _playerFollowCamera3rdBody.CameraDistance = 0.0f;
            }
            else
            {
                _playerFollowCamera3rdBody.Damping = new Vector3(0.1f, 0.25f, 0.3f);
                _playerFollowCamera3rdBody.ShoulderOffset = new Vector3(1.0f, 0.0f, 0.0f);
                _playerFollowCamera3rdBody.CameraDistance = 4.0f;
            }
        }
    }
}