using Cinemachine;
using StarterAssets;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEditor;
using System.Collections.Generic;

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
            OnFocusArtDic();
            // 鼠标按下的时候发射射线
            if (Input.GetMouseButtonDown(0))
            {
                if (isClick && !EventSystem.current.IsPointerOverGameObject())
                {
                    // 发射射线
                    if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out _raycastHit,
                        maxDistance))
                    {
                        Debug.Log(_raycastHit.collider.name);
                        // 作品的图层是6
                        GameObject art = _raycastHit.collider.gameObject;
                        if (art.layer == 6)
                        {
                            Debug.Log(art.name);
                            //FocusArt(art.transform);
                            OnFocusArt(art.transform);
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
            Tools.showFocusWindow();
#endif
        }
        
        
        // 前端发送消息聚焦
        public void FocusArt(string name)
        {
            GameObject art = GameObject.Find(name);
            if (art == null)
            {
            }

            FocusArt(art.GetComponent<Transform>());
        }
        
        // 取消聚焦
        public void CancelFocusArt()
        {
            if (startPoint != Vector3.zero)
            {
                //AddshowcaseList = false;
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
                    IsActionTi = true;
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
                }

                AbInit.instances.ReplaceMaterialImage(art, i.imageUrl);
            }
        }

        public void OnFocusArt(Transform art)
        {
            IsActionTi = false;
            OnActionTi(false);
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
            int indexDot = Vector3.Dot(art.parent.up, transform.position - art.parent.position) <= 0 ? 1 : -1;
            Debug.Log(indexDot);
            art.localPosition = new Vector3(art.localPosition.x, art.localPosition.y - (0.025f * indexDot), art.localPosition.z);
            point = art.position;
            art.localPosition = new Vector3(art.localPosition.x, art.localPosition.y + (0.025f * indexDot), art.localPosition.z);
            Vector3 forwordDir = point - art.position;
            Quaternion lookAtRot = Quaternion.LookRotation(-forwordDir);
            transform.DOMove(point, 1);
            transform.DORotateQuaternion(lookAtRot, 1).OnComplete(() => { isPlayerMove = true; });
        }

        private List<Showcase> showcaseList = new List<Showcase>();
        private bool AddshowcaseList = true;
        private bool IsActionTi = false;
        Transform Player;
        GameObject Ti;

        public void OnActionTi(bool isAction)
        {
            Ti.SetActive(isAction);
        }

        public void OnFocusArtDic()
        {
            //if (GameObject.Find("scene(Clone)") && AddshowcaseList)
            //{
            //    Transform scene = GameObject.Find("scene(Clone)").transform;
            //    for (int i = 0; i < scene.childCount; i++)
            //    {
            //        if (scene.GetChild(i).name.Contains("showcase"))
            //        {
            //            scene.GetChild(i).gameObject.AddComponent<Showcase>();
            //            showcaseList.Add(scene.GetChild(i).GetComponent<Showcase>());
            //        }
            //    }
            //    AddshowcaseList = false;
            //}

            if (AddshowcaseList&&GameObject.Find("PlayerArmature(Clone)"))
            {
                Player = GameObject.Find("PlayerArmature(Clone)").transform;
                
                Ti = Instantiate(Resources.Load("Ti") as GameObject);
                Ti.transform.SetParent(GameObject.Find("TiRoot").transform);
                Ti.transform.localPosition = Vector3.zero;
                OnActionTi(false);
                IsActionTi = true;
                AddshowcaseList = false;
            }
            if (IsActionTi)
            {
                Ray ray = new Ray(Player.position + new Vector3(0, 2, 0), Player.forward*3 );
                Debug.DrawRay(Player.position + new Vector3(0, 2, 0), Player.forward*3, Color.blue);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 3))
                {
                    if (hit.collider.name.Contains("paintings"))
                    {
                        OnActionTi(true);
                        if (Input.GetKeyDown(KeyCode.T))
                        {
                            OnFocusArt(hit.transform);
                        }
                    }
                    else
                    {
                        OnActionTi(false);
                    }
                }
            }


        }


    }
}