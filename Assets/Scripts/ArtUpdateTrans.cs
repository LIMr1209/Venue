using System.Collections;
using System.Collections.Generic;
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
        private static Material outlineMaterial;
        private int _focusArtLayerNum;
        private int _lockArtLayerNum;


        private void Awake()
        {
            _myCamera = GetComponent<Camera>();
            _focusArtLayerNum = LayerMask.NameToLayer(Globle.focusArtLayer);
            _lockArtLayerNum = LayerMask.NameToLayer(Globle.lockArtlayer);
            SetMaterial();
        }

        private void Start()
        {
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hitInfo;
                if (Physics.Raycast(_myCamera.ScreenPointToRay(Input.mousePosition), out hitInfo, Mathf.Infinity))
                {
                    int layer = hitInfo.collider.gameObject.layer;

                    if (layer == _focusArtLayerNum || layer == _lockArtLayerNum)
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
            while (Input.GetMouseButton(0))
            {
                Physics.IgnoreLayerCollision(6, 8, true);
                RaycastHit hitInfo;
                int layerMask = 1 << 6;
                layerMask = ~layerMask;
                // int layerMask = ~ LayerMask.NameToLayer(Globle.lockArtlayer);
                if (Physics.Raycast(_myCamera.ScreenPointToRay(Input.mousePosition), out hitInfo, Mathf.Infinity,
                    layerMask))
                {
                    _target.localPosition = hitInfo.point;
                }

                yield return null;
            }

            Physics.IgnoreLayerCollision(6, 8, false);
        }


        public void SendArtData(Transform target)
        {
#if !UNITY_EDITOR && UNITY_WEBGL            
            CustomAttr customAttr = OpusShow.GetCustomAttr(target.gameObject);
            JsonData.ArtData artData = new JsonData.ArtData();
            artData.name = target.gameObject.name;
            artData.id = customAttr.id;
            artData.imageUrl = customAttr.imageUrl;
            artData.isDel = customAttr.isDel;
            artData.nKind = customAttr.nKind;
            artData.cloneBase = customAttr.cloneBase;
            artData.location = customAttr.location;
            artData.scaleS = customAttr.scaleS;
            artData.rotateS = customAttr.rotateS;
            Tools.selectTrans(JsonUtility.ToJson(artData));
#endif
        }

        public void UpdateCustomAttr(Vector3 movement)
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            CustomAttr customAttr = OpusShow.GetCustomAttr(_target.gameObject);
            if (!customAttr)
            {
                customAttr = _target.gameObject.AddComponent(typeof(CustomAttr)) as CustomAttr;
            }
            customAttr.location[0] = customAttr.location[0] + movement.x;
            customAttr.location[1] = customAttr.location[1] + movement.y;
            customAttr.location[2] = customAttr.location[2] + movement.z;
#endif
        }
    }
}