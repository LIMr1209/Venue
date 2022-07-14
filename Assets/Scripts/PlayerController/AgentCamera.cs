using UnityEngine;

namespace DefaultNamespace
{
    public class AgentCamera : MonoBehaviour
    {
        private PlayerAgent _playerAgent;
        private RaycastHit _raycastHit;
        private OpusShow _opusShow;
        private void Awake()
        {
            _opusShow = FindObjectOfType<OpusShow>();
            enabled = false;
        }

        private void Start()
        {
            _playerAgent = FindObjectOfType<PlayerAgent>();
        }

        private void Update()
        {
            if (_opusShow.enabled && !_opusShow.isPlayerMove && Input.GetMouseButtonDown(1))
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out _raycastHit, 1000))
                {
                    if(_raycastHit.collider.gameObject.layer == LayerMask.NameToLayer(Globle.navMeshLayer) && _playerAgent)
                    {
                        _playerAgent.MovePoint(_raycastHit.point);
                    }
                } 
            }
        }

    }
}
