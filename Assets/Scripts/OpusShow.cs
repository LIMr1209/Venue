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
        AddController addController;
        private RaycastHit _raycastHit;
        private Vector3 _velocity = Vector3.zero;
        private Vector3 startPoint;
        private Quaternion startQuaternion;
        public float smoothTime = 1.0f;
        public int maxDistance = 10;
        private bool isPlayerMove;
        private bool isClick;
        //private List<Showcase> showcaseList = new List<Showcase>();
        private bool AddshowcaseList = true;
        private bool IsActionTi = false;
        private bool IsActionTifalse = true;
        private Transform Player;
        private ThirdPersonController controller;
        private CinemachineVirtualCamera virtualCamera;
        private SkinnedMeshRenderer playerMeshRender;
        Transform TiTrans;
        Transform TargetArt;

        private void Awake()
        {
            enabled = false; // 默认禁用 场景加载完启用
        }

        private void Start()
        {
            addController = GetComponent<AddController>();
            isClick = true;
            isPlayerMove = false;
            Player = GameObject.FindWithTag("Player").transform;
            controller = Player.GetComponent<ThirdPersonController>();
            virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
            playerMeshRender = Player.GetComponentInChildren<SkinnedMeshRenderer>();

        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                GameObject art1 = GameObject.Find("paintings-023");
                GameObject art2 = GameObject.Find("paintings-029");
                GameObject art3 = GameObject.Find("paintings-024");
                GameObject art4 = GameObject.Find("showcase-024");
                Debug.Log("paintings-016" + art1.transform.right);
                Debug.Log("paintings-029" + art2.transform.right);
                Debug.Log("paintings-021" + art3.transform.right);
                Debug.Log("paintings-023" + art4.transform.right);
                //Debug.Log(art3.transform.localRotation+"/"+art4.transform.rotation);
                //art1.transform.Translate(-Vector3.right * 2, Space.Self);
                //art2.transform.Translate(-Vector3.right * 2, Space.Self);
                //art3.transform.Translate(-Vector3.right * 2, Space.Self);
                //art4.transform.Translate(-Vector3.right * 2, Space.Self);
                //art3.transform.parent.rotation = art3.transform.rotation;
                //art3.transform.parent.eulerAngles = art3.transform.parent.eulerAngles + new Vector3(180,0,-90);
                //art3.transform.parent.eulerAngles = art3.transform.parent.eulerAngles + new Vector3(180, 0, -90);
            }
        


            OnFocusArtDic();
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
                        if (art.layer == 6)
                        {
#if !UNITY_EDITOR && UNITY_WEBGL
                            if (!art.TryGetComponent<CustomAttr>(out CustomAttr customAttr))
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

        // 聚焦
        private void FocusArt(Transform art)
        {
            isClick = false;
            // 禁用人物控制器
            if (controller) controller.enabled = false;
            if (virtualCamera) virtualCamera.enabled = false;
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
                Tools.showFocusWindow(customAttr.artId);
            }
#endif
        }


        // 前端发送消息聚焦
        public void OnFocusArt(string name)
        {
            GameObject art = GameObject.Find(name);
            if (art == null)
            {
                throw (new Exception("画框不存在"));
            }

            OnFocusArt(art.GetComponent<Transform>());
        }

        // 取消聚焦
        public void CancelFocusArt()
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

        public static void ReplaceArtImage(JsonData.ArtData[] artDataList)
        {
            foreach (JsonData.ArtData i in artDataList)
            {
                GameObject art = GameObject.Find(i.name);
                if (art == null)
                {
                    throw (new Exception("画框不存在"));
                }
                if (i.id != null && i.imageUrl != null)
                {
                    // 设置自定义id
                    CustomAttr customAttr = art.AddComponent(typeof(CustomAttr)) as CustomAttr;
                    customAttr.artId = i.id;
                    AbInit.instances.ReplaceMaterialImage(art, i.imageUrl);
                }

                OnSetArtV3(art, i);
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
                if (art.gameObject.TryGetComponent<CustomAttr>(out CustomAttr customAttr))
                {
                    Debug.Log("传递作品id"+customAttr.artId);
                    Tools.showFocusWindow(customAttr.artId);
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

            if (IsActionTi)
            {

                Ray ray = new Ray(Player.position + new Vector3(0, 1.7f, 0), Player.forward * 3);
                Debug.DrawRay(Player.position + new Vector3(0, 1.7f, 0), Player.forward * 3, Color.blue);

                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 3))
                {
                    //name.Contains("paintings")
                    if (hit.collider.gameObject.layer == 6)
                    {
#if !UNITY_EDITOR && UNITY_WEBGL
                        if (!hit.collider.gameObject.TryGetComponent<CustomAttr>(out CustomAttr customAttr))
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
    }
}