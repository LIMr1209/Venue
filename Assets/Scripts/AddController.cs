using Cinemachine;
using UnityEngine;

namespace DefaultNamespace
{
    public class AddController : MonoBehaviour
    {
        public bool visual = false; // 第一人称 第三人称切换  true第一人称  false第三人称

        private string controllerAb = "controller";

        private string thirdFollowCameraAb = "playerfollowcamera";

        // private string armatureAb = "playerarmature";
        private string armatureAb = "figure01";

        private GameObject _playerFollowCamera;
        private CinemachineVirtualCamera _cinemachineVirtualCamera;
        private Cinemachine3rdPersonFollow _playerFollowCamera3rdBody;
        private GameObject _player;
        private Transform cinemachineTarget;
        public float zoomSpeed = 10.0f;

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
            if (Input.GetKeyDown("v") && _player) SwithVisul();

            if (Input.GetKeyDown(KeyCode.Z))
            {
                for (int i = 0; i < 50; i++)
                {
                    StartCoroutine(
                        AbInit.instances.OnWebRequestLoadAssetBundleGameObject(armatureAb, controllerAb,
                            new Vector3(0, 0, 0), new Vector3(0, -180, 0), (obj) =>
                            {
                                ShaderProblem.ResetShader(_player); // 解决shader问题
                            }
                        ));
                }
            }

            // float scroll = Input.GetAxis("Mouse ScrollWheel");
            // Debug.Log(_playerFollowCamera3rdBody.ShoulderOffset);
            // _playerFollowCamera3rdBody.ShoulderOffset += _player.transform.forward * scroll * zoomSpeed;
        }

        private void SwithVisul()
        {
            if (visual) AddThird();
            else AddFirst();
        }

        public void AddFirst()
        {
            Vector3 location = Vector3.zero;
            Vector3 rotation = Vector3.zero;
            AddFirst(location, rotation);
        }

        private void AddFirst(Vector3 location, Vector3 rotation)
        {
            if (_player)
            {
                _player.SetActive(true);
                _cinemachineVirtualCamera.Follow = cinemachineTarget;
                _player.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
                visual = true;
                SetFollowCameraBody();
            }
        }

        public void AddThird()
        {
            Vector3 location = Vector3.zero;
            Vector3 rotation = Vector3.zero;
            AddThird(location, rotation);
        }

        public void AddThird(Vector3 location, Vector3 rotation)
        {
            if (_player)
            {
                _player.SetActive(true);
                _cinemachineVirtualCamera.Follow = cinemachineTarget;
                _player.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
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