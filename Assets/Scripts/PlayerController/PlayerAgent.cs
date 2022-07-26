﻿using StarterAssets;
using UnityEngine;
using UnityEngine.AI;

namespace DefaultNamespace
{
    public class PlayerAgent : MonoBehaviour
    {
        public NavMeshAgent agent;
        private LineRenderer _lineRenderer;

        private const string Aoe = "aoe";
        private GameObject _aoe;
        private GameObject _target;
        private Animator _animator;
        private CharacterController _character;
        private int _animIDSpeed;
        private int _animIDJump;
        private OpusShow _opusShow;
        private ThirdPersonController _controller;
        private bool _stop;

        private void Awake()
        {
            _opusShow = FindObjectOfType<OpusShow>();
            _controller = FindObjectOfType<ThirdPersonController>();
            _target = GameObject.FindWithTag("aoe");
            int childCount = _target.transform.childCount;
            if (childCount==0)
            {
                StartCoroutine(
                    AbInit.instances.OnWebRequestLoadAssetBundleGameObject(Aoe, "", (obj) =>
                        {
                            _aoe = obj;
                            _aoe.transform.SetParent(_target.transform, false);
                            _aoe.SetActive(false);
                            ShaderProblem.ResetMeshShader(_aoe);
                        }
                    ));
            }
            else
            {
                _aoe = _target.transform.GetChild(0).gameObject;
            }
        }

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            if (!agent)
            {
                Debug.Log("导航代理未找到");
            }
            agent.updatePosition = false; // 当为 false 时：代理位置不会应用于变换位置，反之亦然。 解决走不动的问题
            _animator = GetComponent<Animator>();
            _character = GetComponent<CharacterController>();
            _animIDSpeed = Animator.StringToHash("Speed");
            _animIDJump = Animator.StringToHash("Jump");
            AgentCamera agentCamera = FindObjectOfType<AgentCamera>();
            if (agentCamera) agentCamera.enabled = true;

        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space) ||Input.GetKeyDown(KeyCode.W) ||Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D)){
                _stop = false;
                if (agent.hasPath) // 代理中 触发 人物控制器后 停止代理
                {
                    agent.SetDestination(transform.position);
                }
            }
            DrawPath(); // 绘制路径
            if (_aoe) {
                _aoe.SetActive(agent.hasPath);
            }
            if (_animator && agent.hasPath)
            {
                _animator.SetFloat(_animIDSpeed, agent.velocity.magnitude); // 设置动画
            }
        }

        private void LateUpdate()
        {
            if (agent.hasPath || _stop)
            {
                _character.transform.position = agent.nextPosition;  // _character的位置 跟着代理位置
            }
        }

        public void MovePoint(Vector3 targetPos)
        {
            if (_opusShow.enabled && _opusShow.isPlayerMove)
            {
                return;
            }

            if (_controller.hasMoveVisualAngle)
            {
                return;
            }

            if (_animator.GetBool(_animIDJump))
            {
                return;
            }

            if (agent.hasPath)
            {
                _stop = true;
                agent.Warp(targetPos);
                return;
            }

            if (!agent.hasPath)
            {
                agent.enabled = false; // 重新启动 让agent代理的位置跟着_character
                agent.enabled = true;
            }
            agent.SetDestination(targetPos); // 导航
            if (_aoe)
            {
                _aoe.transform.position = targetPos;
            }
        }   

        public void DrawPath()
        {
            if (_lineRenderer != null)
            {
                _lineRenderer.positionCount = agent.path.corners.Length;
                _lineRenderer.SetPositions(agent.path.corners);
            }
        }
    }
}