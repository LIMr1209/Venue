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
        private AddController addController;
        private RaycastHit _raycastHit;
        private Vector3 startPoint;
        private Quaternion startQuaternion;
        public int maxDistance = 200;
        public bool isPlayerMove;
        private bool isClick;
        //private List<Showcase> showcaseList = new List<Showcase>();
        private bool AddshowcaseList = true;
        private bool IsActionTi = false;
        private bool IsActionTifalse = true;
        private Transform Player;
        private ThirdPersonController controller;
        private CinemachineVirtualCamera virtualCamera;
        private SkinnedMeshRenderer playerMeshRender;
        private ArtUpdateTrans _artUpdateTrans;
        private Transform TiTrans;
        private Transform TargetArt;

        private void Awake()
        {
            enabled = false; // 默认禁用 场景加载完启用
        }

        public virtual void Start()
        {
            addController = GetComponent<AddController>();
            isClick = true;
            isPlayerMove = false;
            virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
            _artUpdateTrans = FindObjectOfType<ArtUpdateTrans>();
        }

        private void OnEnable()
        {
            Player = GameObject.FindWithTag("Player").transform;
            controller = Player.GetComponent<ThirdPersonController>();
            playerMeshRender = Player.GetComponentInChildren<SkinnedMeshRenderer>();
        }

        private void Update()
        {
            if(_artUpdateTrans.enabled) return;
            OnFocusArtDic();
            // 鼠标按下的时候发射射线
            if (Input.GetMouseButtonUp(0) && !controller.hasMoveVisualAngle)
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
                        if (art.layer == LayerHelp.focusArtLayerNum ||art.layer == LayerHelp.lockArtLayerNum)
                        {
#if !UNITY_EDITOR && UNITY_WEBGL
                            if (!art.transform.parent.TryGetComponent<CustomAttr>(out CustomAttr customAttr))
                            {
                                return;
                            }
#endif
                            OnFocusArt(art.transform);
                        }
                    }
                }
            }

            if (isPlayerMove)
            {
                if (Input.GetKeyDown("w") || Input.GetKeyDown("s") || Input.GetKeyDown("a") || Input.GetKeyDown("d"))
                {
                    CancelFocusArt(); // 取消聚焦
                }
            }
        }

        // 前端发送消息聚焦
        public void OnFocusArt(string name)
        {
            GameObject art = GameObject.Find(name);
            if (art == null)
            {
                throw (new Exception("画框不存在"));
            }
            Transform paining = art.transform.GetChild(1);
            OnFocusArt(paining);
        }

        // 取消聚焦
        public virtual void CancelFocusArt()
        {
            Player.position = startPoint;
            Player.rotation = startQuaternion;
            if (!addController.visual)
            {
                playerMeshRender.enabled = true;
            }
            transform.DOMove(Player.position, 0.1f);
            transform.DORotateQuaternion(Player.rotation, 0.1f).OnComplete(() =>
            {
                // 启用人物控制器
                controller.cinemachineTargetYaw = startQuaternion.eulerAngles.y;
                if (controller) controller.enabled = true;
                if (virtualCamera) virtualCamera.enabled = true;
                isClick = true;
                IsActionTi = true;
#if !UNITY_EDITOR && UNITY_WEBGL
                Tools.canalFocus();  // 调用前端取消聚焦
#endif
            });
            isPlayerMove = false;
        }
        
        // 聚焦
        public void OnFocusArt(Transform art)
        {
            IsActionTi = false;
            OnActionTi(false);
            isClick = false;
            isPlayerMove = false;
            // 禁用人物控制器
            if (controller) controller.enabled = false;
            if (virtualCamera) virtualCamera.enabled = false;
            TargetArt = art;
            Vector3 point = Vector3.zero;
            float index = OnGetArtLengDic(art) * -2;
            int indexDot = Vector3.Dot(art.parent.up, transform.position - art.parent.position) <= 0 ? 1 : -1;
            art.localPosition = new Vector3(art.localPosition.x, art.localPosition.y - (index * indexDot),
                art.localPosition.z);
            //point = art.position;
            for (int a = 0; a < art.parent.childCount; a++)
            {
                if (art.parent.GetChild(a).name.Contains("point"))
                {
                    point = art.parent.GetChild(a).position;
                    startPoint = point;
                }
            }
            startPoint = point;
            art.localPosition = new Vector3(art.localPosition.x, art.localPosition.y + (index * indexDot),
                art.localPosition.z);
            point.y = art.position.y;
            Vector3 forwordDir = point - art.position;
            Quaternion lookAtRot = Quaternion.LookRotation(-forwordDir);
            startQuaternion = lookAtRot;
            transform.DOMove(point, 1);
            transform.DORotateQuaternion(lookAtRot, 1).OnComplete(() =>
            {
                Player.position = point + new Vector3(-0.3f * indexDot, -1f, 0);
                if (!addController.visual)
                {
                    playerMeshRender.enabled = false;
                }
                isPlayerMove = true;
#if !UNITY_EDITOR && UNITY_WEBGL
                if (art.transform.parent.TryGetComponent<CustomAttr>(out CustomAttr customAttr))
                {
                    Debug.Log("传递作品id"+customAttr.id);
                    Tools.showFocusWindow(customAttr.id);
                }
#endif
            });
        }

        public float OnGetArtLengDic(Transform art)
        {
            Vector3 length = art.GetComponent<MeshFilter>().mesh.bounds.size;
            float index;
            if (art.localScale.z == art.localScale.y)
            {
                index = art.localScale.y * length.y;
            }
            else
            {
                index = art.localScale.z < art.localScale.y ? art.localScale.z * length.z : art.localScale.y * length.y;
            }
            return index;
        }




        public void OnActionTi(bool isAction)
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            Tools.showFocusTipsWindow(isAction);
#endif
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

            if (AddshowcaseList)
            {
                IsActionTi = true;
                AddshowcaseList = false;
            }

            if (IsActionTi && Player)
            {

                Ray ray = new Ray(Player.position + new Vector3(0, 1.7f, 0), Player.forward * 3);
                Debug.DrawRay(Player.position + new Vector3(0, 1.7f, 0), Player.forward * 3, Color.blue);

                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 3))
                {
                    GameObject art = hit.collider.gameObject;
                    if (art.layer == LayerHelp.focusArtLayerNum ||art.layer == LayerHelp.lockArtLayerNum)
                    {
#if !UNITY_EDITOR && UNITY_WEBGL
                        if (!hit.collider.gameObject.transform.parent.TryGetComponent<CustomAttr>(out CustomAttr customAttr))
                        {
                            return;
                        }
#endif

                        if (TiTrans == null || TiTrans != hit.transform)
                        {
                            TiTrans = hit.transform;
                            OnActionTi(true);
                            IsActionTifalse = true;
                        }
                        if (Input.GetKeyDown(KeyCode.T))
                        {
                            OnFocusArt(hit.transform);
                        }
                    }
                    else
                    {
                        if (IsActionTifalse)
                        {
                            TiTrans = null;
                            OnActionTi(false);
                            IsActionTifalse = false;
                        }
                    }
                }
            }
        }



        public static void OnSetArtV3(GameObject art, JsonData.ArtData i)
        {
            art.transform.localPosition = new Vector3(-i.location[0], i.location[1], i.location[2]);
            if (i.name.Contains("showcase"))
            {
                Transform sss = null;
                for (int s = 0; s < art.transform.childCount; s++)
                {
                    if (art.transform.GetChild(s).name.Contains("paintings"))
                    {
                        sss = art.transform.GetChild(s);
                    }
                }
                if (art.transform.right.y != 1)
                {
                    art.transform.rotation = sss.rotation;
                    art.transform.eulerAngles = art.transform.eulerAngles + new Vector3(180, 0, -90);
                }
                else
                {
                    art.transform.rotation = sss.rotation;
                    art.transform.eulerAngles = art.transform.eulerAngles + new Vector3(180, 0, 90);
                }

            }
            else
            {
                art.transform.localRotation = new Quaternion(i.quaternion[3], i.quaternion[1], -i.quaternion[2], i.quaternion[0]);
            }
            art.transform.localScale = new Vector3(i.scale[0], i.scale[2], i.scale[1]);

            if (i.name.Contains("painting"))
            {
                Transform frames = null;
                Transform paintings = null;
                for (int a = 0; a < art.transform.parent.childCount; a++)
                {
                    if (art.transform.parent.GetChild(a).name.Contains("frames"))
                    {
                        frames = art.transform.parent.GetChild(a);
                    }
                    if (art.transform.parent.GetChild(a).name.Contains("paintings"))
                    {
                        paintings = art.transform.parent.GetChild(a);
                    }
                }
                float index = paintings.right.x+ paintings.right.z > 0 ? 0.01f : -0.01f;
                paintings.localPosition = new Vector3(frames.localPosition.x, paintings.localPosition.y + index, frames.localPosition.z);
            }
        }
    }
}