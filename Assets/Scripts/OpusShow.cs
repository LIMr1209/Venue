using System;
using Cinemachine;
using StarterAssets;
using UnityEngine;
using DG.Tweening;
// using UnityEngine.EventSystems;

namespace DefaultNamespace
{
    public class OpusShow : MonoBehaviour
    {
        private RaycastHit _raycastHit;
        private Vector3 _velocity = Vector3.zero;
        public float smoothTime = 1.0f;
        public int maxDistance = 10;
        private Vector3 startPoint;
        private Quaternion startRotation;
        private bool isPlayerMove;
        private bool isClick;

        private void Start()
        {
            isClick = true;
            isPlayerMove = true;
            startPoint = transform.position;
            startRotation = transform.localRotation;
        }

        private void Update()
        {
            // 鼠标按下的时候发射射线
            if (Input.GetMouseButtonDown(0))
            {
                // if (isClick && !EventSystem.current.IsPointerOverGameObject())
                if (isClick)    
                {
                    // 发射射线
                    if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out _raycastHit,
                        maxDistance))
                    {
                        // 作品的图层是6
                        GameObject art = _raycastHit.collider.gameObject;
                        if (art.layer == 6 )
                        {
                            FocusArt(art.transform);
                        }
                    }
                }
            }

            if (isPlayerMove)
            {
                if (Input.GetKeyDown("w") || Input.GetKeyDown("s") || Input.GetKeyDown("a") || Input.GetKeyDown("d"))
                {
                    CancelFocusArt();  // 取消聚焦
                }
            }
        }
        
        // 聚焦
        private void FocusArt(Transform art)
        {
            isClick = false;
            // 禁用人物控制器
            ThirdPersonController controller = FindObjectOfType<ThirdPersonController>();
            if (controller) controller.enabled = false;
            CinemachineVirtualCamera virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
            if (virtualCamera) virtualCamera.enabled = false;
            startPoint = transform.position;
            startRotation = transform.localRotation;
            isPlayerMove = false;
            Vector3 point = Vector3.zero;
            Quaternion qqq = art.rotation;
            if (Vector3.Dot(art.forward, art.position - transform.position) > 0)
            {
                if (art.forward.z == -1)
                {
                    point = art.position + new Vector3(0, 0, 5);
                }
                else if (art.forward.x == 1)
                {
                    point = art.position + new Vector3(-5, 0, 0);
                }
                else if (art.forward.z == 1)
                {
                    point = art.position + new Vector3(0, 0, -5);
                }
                else
                {
                    point = art.position + new Vector3(5, 0, 0);
                }
            }
            else
            {
                if (art.forward.z == -1)
                {
                    Debug.Log(1);
                    point = art.position + new Vector3(0, 0, -5);
                    qqq = Quaternion.Euler(new Vector3(art.rotation.x, art.rotation.y, art.rotation.z));
                }
                else if (art.forward.x == 1)
                {
                    Debug.Log(2);
                    point = art.position + new Vector3(5, 0, 0);
                }
                else if (art.forward.z == 1)
                {
                    Debug.Log(3);
                    point = art.position + new Vector3(0, 0, 5);
                    qqq = Quaternion.Euler(new Vector3(art.rotation.x, art.rotation.y + 180, art.rotation.z));
                }
                else
                {
                    Debug.Log(4);
                    point = art.position + new Vector3(-5, 0, 0);
                    qqq = Quaternion.Euler(new Vector3(art.rotation.x, art.rotation.y + 90, art.rotation.z));
                }
            }

            transform.DOMove(point, 1);
            transform.DORotateQuaternion(qqq, 1).OnComplete(() => { isPlayerMove = true; });
#if !UNITY_EDITOR && UNITY_WEBGL            
            // 通知前端显示聚焦后ui
            if (art.gameObject.TryGetComponent<CustomAttr>(out CustomAttr customAttr))
            { 
                Tools.showFocusWindow(customAttr.id);
            }
#endif
        }
        
        
        // 前端发送消息聚焦
        public void FocusArt(string name)
        {
            GameObject art = GameObject.Find(name);
            if (art == null)
            {
                throw (new Exception("画框不存在"));
            }

            FocusArt(art.GetComponent<Transform>());
        }
        
        // 取消聚焦
        public void CancelFocusArt()
        {
            if (startPoint != Vector3.zero)
            {
                transform.DOMove(startPoint, 1);
                transform.DORotateQuaternion(startRotation, 1).OnComplete(() =>
                {
                    // 启用人物控制器
                    startPoint = Vector3.zero;
                    ThirdPersonController controller = FindObjectOfType<ThirdPersonController>();
                    if (controller) controller.enabled = true;
                    CinemachineVirtualCamera virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
                    if (virtualCamera) virtualCamera.enabled = true;
                    isClick = true;
                });
            }
        }

        public static void ReplaceArtImage(JsonData.ArtData[] artDataList)
        {
            foreach (JsonData.ArtData i in artDataList)
            {
                GameObject art = GameObject.Find(i.name);
                if (art == null)
                {
                    throw (new Exception("画框不存在"));
                }
                
                // 设置自定义id
                CustomAttr customAttr = art.AddComponent(typeof(CustomAttr)) as CustomAttr;
                customAttr.id = i.id;

                AbInit.instances.ReplaceMaterialImage(art, i.imageUrl);
                // 轴方向不一样 可能会有问题
                // art.transform.localPosition = new Vector3(i.position[0], i.position[1], i.position[2]);
                // art.transform.localScale = new Vector3(i.scale[0], i.scale[1], i.scale[2]);
                // art.transform.localRotation = Quaternion.Euler(i.position[0], i.position[1], i.position[2]);
            }
        }
    }
}