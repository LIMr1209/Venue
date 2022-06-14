using Cinemachine;
using StarterAssets;
using UnityEngine;
using DG.Tweening;

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

        private void Start()
        {
            isPlayerMove = true;
        }

        private void Update()
        {

            // 鼠标按下的时候发射射线
            if (Input.GetMouseButtonDown(0))
            {
                // 发射射线
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out _raycastHit, maxDistance,
                    1 << 6))
                {
                    GameObject art = _raycastHit.collider.gameObject;
                    // 禁用人物控制器
                    ThirdPersonController controller = FindObjectOfType<ThirdPersonController>();
                    if (controller) controller.enabled = false;
                    CinemachineVirtualCamera virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
                    if (virtualCamera) virtualCamera.enabled = false;
                    OnCameraMove(art.transform);
                }
            }

            if (isPlayerMove)
            {
                if (Input.GetKeyDown("w") || Input.GetKeyDown("s") || Input.GetKeyDown("a") || Input.GetKeyDown("d"))
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
                        });
                    }
                }
            }
        }

        private void OnCameraMove(Transform art)
        {
            isPlayerMove = false;
            startPoint = transform.position;
            startRotation = transform.localRotation;
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
                    point = art.position + new Vector3(0, 0, -5);
                    qqq = Quaternion.Euler(new Vector3(art.rotation.x, art.rotation.y, art.rotation.z));
                    Debug.Log(111);
                }
                else if (art.forward.x == 1)
                {
                    point = art.position + new Vector3(5, 0, 0);
                    Debug.Log(222);
                }
                else if (art.forward.z == 1)
                {
                    point = art.position + new Vector3(0, 0, 5);
                    qqq = Quaternion.Euler(new Vector3(art.rotation.x, art.rotation.y + 180, art.rotation.z));
                    Debug.Log(333);
                }
                else
                {
                    point = art.position + new Vector3(-5, 0, 0);
                    qqq = Quaternion.Euler(new Vector3(art.rotation.x, art.rotation.y + 90, art.rotation.z));
                    Debug.Log(444);
                }
            }
            transform.DOMove(point, 1);
            transform.DORotateQuaternion(qqq, 1).OnComplete(()=> { isPlayerMove = true; });
        }
    }
}