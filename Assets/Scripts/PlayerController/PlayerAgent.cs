using UnityEngine;
using UnityEngine.AI;

namespace DefaultNamespace
{
    public class PlayerAgent : MonoBehaviour
    {
        public NavMeshAgent agent;
        private LineRenderer _lineRenderer;

        private const string Aoe = "aoe";
        private GameObject _target;
        private Animator _animator;
        private CharacterController _character;
        private int _animIDSpeed;

        private void Start()
        {
            StartCoroutine(
                AbInit.instances.OnWebRequestLoadAssetBundleGameObject(Aoe, "", (obj) =>
                    {
                        _target = obj;
                        _target.SetActive(false);
                        ShaderProblem.ResetParticleShader(_target);
                    }
            ));
            agent = GetComponent<NavMeshAgent>();
            if (!agent)
            {
                Debug.Log("导航代理未找到");
            }
            agent.updatePosition = false; // 当为 false 时：代理位置不会应用于变换位置，反之亦然。 解决走不动的问题
            _animator = GetComponent<Animator>();
            _character = GetComponent<CharacterController>();
        
            _animIDSpeed = Animator.StringToHash("Speed");
            AgentCamera agentCamera = FindObjectOfType<AgentCamera>();
            if (agentCamera) agentCamera.enabled = true;

        }

        private void Update()
        {
            if(agent.hasPath && (Input.GetKeyDown(KeyCode.Space) ||Input.GetKeyDown(KeyCode.W) ||Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D) ))  // 代理中 触发 人物控制器后 停止代理
            {
                agent.SetDestination(transform.position);
            }
            DrawPath(); // 绘制路径
            if (_target) {
                _target.SetActive(agent.hasPath);
            }
            if (_animator && agent.hasPath)
            {
                _animator.SetFloat(_animIDSpeed, agent.velocity.magnitude); // 设置动画
            }
        }

        private void LateUpdate()
        {
            if (agent.hasPath)
            {
                _character.transform.position = agent.nextPosition;  // 人物控制器的位置 跟着代理
            }
        }

        public void MovePoint(Vector3 targetPos)
        {
            if (!agent.hasPath)
            {
                agent.enabled = false; // 重新启动 让代理的位置跟着代理  问题就是 连续点击寻路时会卡顿
                agent.enabled = true;
            }
            agent.SetDestination(targetPos); // 导航
            if (_target)
            {
                _target.transform.position = targetPos;
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