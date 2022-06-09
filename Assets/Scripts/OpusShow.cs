using Cinemachine;
using StarterAssets;
using UnityEngine;

namespace DefaultNamespace
{
    public class OpusShow : MonoBehaviour
    {
        private RaycastHit _raycastHit;
        private Vector3 _velocity = Vector3.zero;
        public float smoothTime = 1.0f;
        public int maxDistance = 1000;
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
                    Quaternion rotation = Quaternion.LookRotation(art.transform.forward, Vector3.up);
                    transform.rotation = rotation;
                    transform.LookAt(art.transform);
                    transform.rotation = Quaternion.Euler(new Vector3(art.transform.rotation.eulerAngles.x, art.transform.rotation.eulerAngles.y, art.transform.rotation.eulerAngles.z));
                    transform.localPosition = Vector3.SmoothDamp(transform.localPosition, _raycastHit.point,
                        ref _velocity, smoothTime);
                    // transform.localPosition = new Vector3(art.transform.position.x+5, art.transform.position.y,art.transform.position.z);
                }
            }
            
            if (Input.GetKeyDown("w") || Input.GetKeyDown("s") || Input.GetKeyDown("a") || Input.GetKeyDown("d"))
            {
                // 启用人物控制器
                ThirdPersonController controller = FindObjectOfType<ThirdPersonController>();
                if (controller) controller.enabled = true;
                CinemachineVirtualCamera virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
                if (virtualCamera) virtualCamera.enabled = true;
            }
        }
    }
}