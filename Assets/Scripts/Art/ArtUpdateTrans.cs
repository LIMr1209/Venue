using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

namespace DefaultNamespace
{
    public class ArtUpdateTrans : MonoBehaviour
    {
        private Transform _target;
        private GameObject _art;
        private Camera _myCamera;
        private List<Renderer> _renderersBuffer = new List<Renderer>();
        private List<Material> _materialsBuffer = new List<Material>();
        private HashSet<Renderer> _highlightedRenderers = new HashSet<Renderer>();
        private ThirdPersonController _controller;
        private static Material _outlineMaterial;
        private Vector3 _deviationPosition;
        public float _distance; // 画框和摄像机两点距离的平方
        private GameObject _player;
        private float _weightX = 0.25f;
        private float _weightY = 0.25f;
        public bool groundCheck;
        public bool wallCheck;
        public float checkRadius = 0.5f;

        private void Awake()
        {
            _myCamera = GetComponent<Camera>();
            SetMaterial();
        }

        private void Start()
        {
            _controller = FindObjectOfType<ThirdPersonController>();
            _player = GameObject.FindGameObjectWithTag("Player");
        }

        public void ResetController()
        {
            _controller = FindObjectOfType<ThirdPersonController>();
            _player = GameObject.FindGameObjectWithTag("Player");
        }

        private void Update()
        {
            if (Input.GetMouseButtonUp(0))
            {
                if (_controller && _controller.hasMoveVisualAngle) return;
                RaycastHit hitInfo;
                if (Physics.Raycast(_myCamera.ScreenPointToRay(Input.mousePosition), out hitInfo, Mathf.Infinity))
                {
                    int layer = hitInfo.collider.gameObject.layer;

                    if (layer == LayerHelp.focusArtLayerNum || layer == LayerHelp.lockArtLayerNum)
                    {
                        ClearAndAddTarget(hitInfo.transform.parent);
                        _art = hitInfo.collider.gameObject;
                    }
                    else
                    {
                        ClearTarget();
                    }
                }
            }

            // if (_art && Input.GetMouseButtonDown(0))
            // {
            //     RaycastHit hitInfo;
            //     if (Physics.Raycast(_myCamera.ScreenPointToRay(Input.mousePosition), out hitInfo, Mathf.Infinity))
            //     {
            //         if (_target == hitInfo.collider.gameObject.transform.parent)
            //         {
            //             _deviationPosition = hitInfo.point - _target.localPosition; // 拖动时候得偏移量
            //             _distance = Vector3.Distance(_myCamera.transform.position, _target.transform.position); // 计算距离
            //             StartCoroutine(TransformSelected());
            //         }
            //     }
            // }
        }


        public void ClearTarget()
        {
            ClearHighlightedRenderers();
            _target = null;
            _art = null;
#if !UNITY_EDITOR && UNITY_WEBGL
            Tools.unSelectTrans();
#endif
        }

        public void AddTarget(Transform art)
        {
            _target = art;
            AddHighlightedRenderers();
            SendArtData(_target);
        }

        public void ClearAndAddTarget(Transform art)
        {
            ClearTarget();
            AddTarget(art);
        }

        void ClearHighlightedRenderers()
        {
            GetTargetRenderers(_renderersBuffer);
            RemoveHighlightedRenderers(_renderersBuffer);
        }

        void AddHighlightedRenderers()
        {
            GetTargetRenderers(_renderersBuffer);
            AddTargetHighlightedRenderers();
        }

        void GetTargetRenderers(List<Renderer> renderers)
        {
            renderers.Clear();
            if (_target != null)
            {
                _target.GetComponentsInChildren<Renderer>(true, renderers);
            }
        }

        void RemoveHighlightedRenderers(List<Renderer> renderers)
        {
            if (_target == null)
            {
                return;
            }

            for (int i = 0; i < _renderersBuffer.Count; i++)
            {
                Renderer render = _renderersBuffer[i];
                if (render != null)
                {
                    _materialsBuffer.Clear();
                    _materialsBuffer.AddRange(render.sharedMaterials);

                    if (_materialsBuffer.Contains(_outlineMaterial))
                    {
                        _materialsBuffer.Remove(_outlineMaterial);
                        render.materials = _materialsBuffer.ToArray();
                    }
                }

                _highlightedRenderers.Remove(render);
            }

            _renderersBuffer.Clear();
        }

        void AddTargetHighlightedRenderers()
        {
            if (_target == null)
            {
                return;
            }

            for (int i = 0; i < _renderersBuffer.Count; i++)
            {
                Renderer render = _renderersBuffer[i];

                if (!_highlightedRenderers.Contains(render))
                {
                    _materialsBuffer.Clear();
                    _materialsBuffer.AddRange(render.sharedMaterials);

                    if (!_materialsBuffer.Contains(_outlineMaterial))
                    {
                        _materialsBuffer.Add(_outlineMaterial);
                        render.materials = _materialsBuffer.ToArray();
                    }

                    _highlightedRenderers.Add(render);
                }
            }

            _materialsBuffer.Clear();
        }

        void SetMaterial()
        {
            if (_outlineMaterial == null)
            {
                _outlineMaterial = new Material(Shader.Find("Custom/Outline"));
            }
        }

        IEnumerator TransformSelected()
        {
            MeshCollider[] artColliders =
            {
                _target.transform.GetChild(0).gameObject.GetComponent<MeshCollider>(),
                
                _target.transform.GetChild(1).gameObject.GetComponent<MeshCollider>()
            };
            foreach (var i in artColliders)
            {
                i.enabled = false;
            }

            while (Input.GetMouseButton(0))
            {
                if (_controller) _controller.LockCameraPosition = true; // 禁用控制器 旋转视角

                // 忽略 人物与 画框的碰撞
                // Physics.IgnoreLayerCollision(LayerHelp.focusArtLayerNum, LayerHelp.playerLayerNum, true);
                // Physics.IgnoreLayerCollision(LayerHelp.frameLayerNum, LayerHelp.playerLayerNum, true);

                // 设置坐标系
                // float mouseY = Input.GetAxis("Mouse Y");
                // _yaw += mouseY * _weightY;

                // float mouseX = Input.GetAxis("Mouse X");
                // _arcLength += mouseX * weightX;
                // Debug.Log("距离 "+ _distance);
                // Debug.Log("弧长 "+ _arcLength);
                // 圆的周长 
                // double perimeter = _distance * 2 * Math.PI;
                // double angle = 360 * (_distance / perimeter);
                // Debug.Log("角度 "+ angle);


                // float x = Mathf.Sin(0 * Mathf.Deg2Rad) * _distance;
                // float z = Mathf.Cos(0 * Mathf.Deg2Rad) * _distance;
                // Vector3 worldPoint = _myCamera.transform.TransformPoint(new Vector3(x, _yaw, z)-_deviationPosition);
                // _target.localPosition = worldPoint;
                // 画框位置跟随鼠标
                Vector3 mousePosOnScreen = Input.mousePosition;
                mousePosOnScreen.z = _distance-_myCamera.transform.position.z;
                Vector3 mousePosInWorld = _myCamera.ScreenToWorldPoint(mousePosOnScreen);
                _target.position = mousePosInWorld - _deviationPosition;
                // // 画框朝向跟随摄像头
                // Vector3 lookPos = _myCamera.transform.position - _target.position;
                // lookPos.y = 0;
                // Quaternion rotation = Quaternion.LookRotation(lookPos);
                // _target.rotation = rotation;
                // _target.transform.Rotate(new Vector3(0, 90, 0));
                // // 触地后鼠标继续向下  距离减少
                // // 检测地面 鼠标上下鼠标 调整距离 上距离 最高不超过初始距离
                // Vector3 bottomPosition = _target.transform.position;
                // groundCheck = Physics.CheckSphere(bottomPosition, checkRadius, 1<<LayerHelp.groundLayerNum,
                //     QueryTriggerInteraction.Ignore);
                //
                // // 检测墙体 做吸附效果
                // Vector3 spherePosition = _target.transform.position;
                // wallCheck = Physics.CheckSphere(spherePosition, checkRadius, 1<<LayerHelp.wallLayerNum,
                //     QueryTriggerInteraction.Ignore);
                
                

                yield return null;
            }

            foreach (var i in artColliders)
            {
                i.enabled = true;
            }

            if (_controller) _controller.LockCameraPosition = false;
            // Physics.IgnoreLayerCollision(LayerHelp.focusArtLayerNum, LayerHelp.playerLayerNum, false);
            // Physics.IgnoreLayerCollision(LayerHelp.frameLayerNum, LayerHelp.playerLayerNum, false);
        }

        private void OnDrawGizmosSelected()
        {
            if (_target)
            {
                Color transparentGreen = Color.green;
                Color transparentBlue = Color.blue;
                Color transparentRed = Color.red;

                if (groundCheck) Gizmos.color = transparentGreen;
                else if (wallCheck) Gizmos.color = transparentBlue;
                else Gizmos.color = transparentRed;
                Gizmos.DrawSphere(_target.position,checkRadius); 
            }
        }


        public void SendArtData(Transform target)
        {
            CustomAttr customAttr = CustomAttr.GetCustomAttr(target.gameObject);
            JsonData.ArtData artData = new JsonData.ArtData();
            artData.name = target.gameObject.name;
            customAttr.GetArtData(ref artData);
#if !UNITY_EDITOR && UNITY_WEBGL
            Tools.selectTrans(JsonUtility.ToJson(artData));
#endif
        }

        public void UpdateCustomAttr(Vector3 movement)
        {
            CustomAttr customAttr = CustomAttr.GetCustomAttr(_target.gameObject);
            customAttr.location[0] = customAttr.location[0] + Convert.ToSingle(Math.Round(movement.x, 2));
            customAttr.location[1] = customAttr.location[1] + Convert.ToSingle(Math.Round(movement.y, 2));
            customAttr.location[2] = customAttr.location[2] + Convert.ToSingle(Math.Round(movement.z, 2));
            // customAttr.location[0] = customAttr.location[0] + movement.x;
            // customAttr.location[1] = customAttr.location[1] + movement.y;
            // customAttr.location[2] = customAttr.location[2] + movement.z;
        }
    }
}