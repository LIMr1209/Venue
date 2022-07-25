using System;
using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DefaultNamespace
{
    public class AddController : MonoBehaviour
    {
        public bool visual = false; // 第一人称 第三人称切换  true第一人称  false第三人称

        private string controllerAb = "controller";

        private string thirdFollowCameraAb = "playerfollowcamera";

        // private string armatureAb = "playerarmature";
        private string armatureAb = "figurebase";
        [HideInInspector] public string characterId="00";
        [HideInInspector] public string characterAb;

        private GameObject _playerFollowCamera;
        private CinemachineVirtualCamera _cinemachineVirtualCamera;
        private Cinemachine3rdPersonFollow _playerFollowCamera3rdBody;
        private GameObject _player;
        private Transform cinemachineTarget;
        private OpusShow _opusShow;
        private GameObject _mask;
        private bool _switchCharacter;
        public float zoomSpeed = 10.0f;
        private ArtUpdateTrans _artUpdateTrans;
        private AgentCamera _agentCamera;

        private void Awake()
        {
            _opusShow = FindObjectOfType<OpusShow>();
            _artUpdateTrans = FindObjectOfType<ArtUpdateTrans>();
            _agentCamera = FindObjectOfType<AgentCamera>();
            StartCoroutine(
                AbInit.instances.OnWebRequestLoadAssetBundleGameObject(thirdFollowCameraAb, controllerAb, (obj) =>
                    {
                        _playerFollowCamera = obj;
                        _cinemachineVirtualCamera = _playerFollowCamera.GetComponent<CinemachineVirtualCamera>();
                        _playerFollowCamera3rdBody = _playerFollowCamera.GetComponent<CinemachineVirtualCamera>()
                            .GetCinemachineComponent<Cinemachine3rdPersonFollow>();
                    }
                ));
            
            characterAb = String.Format("figure{0}", characterId.PadLeft(2, '0'));

            StartCoroutine(
                AbInit.instances.OnWebRequestLoadAssetBundleGameObject(characterAb, controllerAb,
                    new Vector3(0, 0, 0), new Vector3(0, -180, 0), false, (obj) =>
                    {
                        _player = obj;
                        _player.SetActive(false);
                        ShaderProblem.ResetMeshShader(_player); // 解决shader问题
                            cinemachineTarget =
                            _player.transform.Find("PlayerCameraRoot").GetComponent<Transform>();
                    }
                ));
        }

        private void Update()
        {
            if (_switchCharacter)
            {
                SetCharacterColorAni();
            }
            else
            {
                if (Input.GetKeyDown("v") && _player) SwithVisul();
                // float scroll = Input.GetAxis("Mouse ScrollWheel");
                // Debug.Log(_playerFollowCamera3rdBody.ShoulderOffset);
                // _playerFollowCamera3rdBody.ShoulderOffset += _player.transform.forward * scroll * zoomSpeed;
            }
        }

        private void SwithVisul()
        {
            if (visual) StartCoroutine(AddThird());
            else StartCoroutine(AddFirst());
        }

        public IEnumerator AddFirst()
        {
            yield return _player != null;
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
            if (_opusShow && !_opusShow.enabled) _opusShow.enabled = true;
            if (_artUpdateTrans && _artUpdateTrans.enabled) _artUpdateTrans.ResetController();
        }

        public IEnumerator  AddThird()
        {
            yield return _player != null;
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
            if (_opusShow && !_opusShow.enabled) _opusShow.enabled = true;
            if (_artUpdateTrans && _artUpdateTrans.enabled) _artUpdateTrans.ResetController();
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

        public void UpdateCharacter()
        {
            _opusShow.enabled = false;
            _agentCamera.enabled = false;
            characterAb = String.Format("figure{0}", characterId.PadLeft(2, '0'));
            _switchCharacter = true;
            Vector3 oldPosition = _player.transform.position;
            Quaternion oldQuaternion = _player.transform.Find("PlayerCameraRoot").transform.rotation;
            Destroy(_player);
            
            StartCoroutine(
                AbInit.instances.OnWebRequestLoadAssetBundleGameObject(characterAb, controllerAb, oldPosition,
                    _player.transform.rotation.eulerAngles, false, (obj) =>
                    {
                        _player = obj;
                        _player.transform.Find("PlayerCameraRoot").transform.rotation = oldQuaternion;
                        _player.GetComponent<PlayerInput>().enabled = false;
                        _player.GetComponent<PlayerInput>().enabled = true;
                        cinemachineTarget =
                            _player.transform.Find("PlayerCameraRoot").GetComponent<Transform>();
                        _cinemachineVirtualCamera.Follow = cinemachineTarget;
                        ShaderProblem.ResetMeshShader(_player); // 解决shader问题
                        _opusShow.enabled = true;
                        _agentCamera.enabled = true;
                        _switchCharacter = false;
#if !UNITY_EDITOR && UNITY_WEBGL
                        Tools.updateEnd();
#endif
                    }
                ));
        }

        public void SetCharacterColorAni()
        {
            // float t = Mathf.PingPong(Time.time, 1);
            // Color c = Color.Lerp(Color.white, Color.gray, t);
            // SkinnedMeshRenderer rend = _player.GetComponentInChildren<SkinnedMeshRenderer>();
            // foreach (Material material in rend.sharedMaterials)
            // {
            //     material.color = c;
            // }
        }
    }
}