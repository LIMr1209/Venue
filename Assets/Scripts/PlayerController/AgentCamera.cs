﻿using UnityEngine;

namespace DefaultNamespace
{
    public class AgentCamera : MonoBehaviour
    {
        private PlayerAgent _playerAgent;
        private RaycastHit _raycastHit;

        private void Awake()
        {
            enabled = false;
        }

        private void Start()
        {
            _playerAgent = FindObjectOfType<PlayerAgent>();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
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
