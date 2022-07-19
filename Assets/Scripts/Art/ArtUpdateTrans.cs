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
        private List<Renderer> renderersBuffer = new List<Renderer>();
        private List<Material> materialsBuffer = new List<Material>();
        private HashSet<Renderer> highlightedRenderers = new HashSet<Renderer>();
        private ThirdPersonController _controller;
        private static Material outlineMaterial;

        private void Awake()
        {
            _myCamera = GetComponent<Camera>();
            SetMaterial();
        }

        private void Start()
        {
            _controller = FindObjectOfType<ThirdPersonController>();
        }

        public void ResetController()
        {
            _controller = FindObjectOfType<ThirdPersonController>();
        }

        private void Update()
        {
            if (Input.GetMouseButtonUp(0))
            {
                if(_controller && _controller.hasMoveVisualAngle)  return;
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

            if (_art)
            {
                // StartCoroutine(TransformSelected());
            }
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
            GetTargetRenderers(renderersBuffer);
            RemoveHighlightedRenderers(renderersBuffer);
        }

        void AddHighlightedRenderers()
        {
            GetTargetRenderers(renderersBuffer);
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

            for (int i = 0; i < renderersBuffer.Count; i++)
            {
                Renderer render = renderersBuffer[i];
                if (render != null)
                {
                    materialsBuffer.Clear();
                    materialsBuffer.AddRange(render.sharedMaterials);

                    if (materialsBuffer.Contains(outlineMaterial))
                    {
                        materialsBuffer.Remove(outlineMaterial);
                        render.materials = materialsBuffer.ToArray();
                    }
                }

                highlightedRenderers.Remove(render);
            }

            renderersBuffer.Clear();
        }

        void AddTargetHighlightedRenderers()
        {
            if (_target == null)
            {
                return;
            }

            for (int i = 0; i < renderersBuffer.Count; i++)
            {
                Renderer render = renderersBuffer[i];

                if (!highlightedRenderers.Contains(render))
                {
                    materialsBuffer.Clear();
                    materialsBuffer.AddRange(render.sharedMaterials);

                    if (!materialsBuffer.Contains(outlineMaterial))
                    {
                        materialsBuffer.Add(outlineMaterial);
                        render.materials = materialsBuffer.ToArray();
                    }

                    highlightedRenderers.Add(render);
                }
            }

            materialsBuffer.Clear();
        }

        void SetMaterial()
        {
            if (outlineMaterial == null)
            {
                outlineMaterial = new Material(Shader.Find("Custom/Outline"));
            }
        }

        IEnumerator TransformSelected()
        {
            // Vector3 screenPos = Camera.main.WorldToScreenPoint(_target.position);
            // Vector3 mousePosOnScreen = Input.mousePosition;
            // mousePosOnScreen.z = screenPos.z;
            // Vector3 mousePosInWorld = Camera.main.ScreenToWorldPoint(mousePosOnScreen);
            // _target.position = mousePosInWorld;
            // yield return null;
            while (Input.GetMouseButton(0))
            {
                if (_controller) _controller.LockCameraPosition = true;
                Physics.IgnoreLayerCollision(LayerHelp.focusArtLayerNum, LayerHelp.playerLayerNum, true);
                Physics.IgnoreLayerCollision(LayerHelp.frameLayerNum, LayerHelp.playerLayerNum, true);
                RaycastHit hitInfo;
                int layerMask = 1 << LayerHelp.focusArtLayerNum;
                layerMask = ~layerMask;
                if (Physics.Raycast(_myCamera.ScreenPointToRay(Input.mousePosition), out hitInfo, Mathf.Infinity,
                    layerMask))
                {
                    _target.localPosition = hitInfo.point;
                }
                yield return null;
            }
            if (_controller) _controller.LockCameraPosition = false;
            Physics.IgnoreLayerCollision(LayerHelp.focusArtLayerNum, LayerHelp.playerLayerNum, false);
            Physics.IgnoreLayerCollision(LayerHelp.frameLayerNum, LayerHelp.playerLayerNum, false);
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