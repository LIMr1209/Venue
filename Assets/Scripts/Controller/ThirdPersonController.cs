using System.Collections;
using DefaultNamespace;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

/* Note: animations are called via the controller for both the character and capsule using animator null checks
 */

namespace StarterAssets
{
    [RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
    [RequireComponent(typeof(PlayerInput))]
#endif
    public class ThirdPersonController : MonoBehaviour
    {
        [Header("玩家")]
        [Tooltip("移动速度 m/s")]
        public float MoveSpeed = 2.0f;

        [Tooltip("跑步速度 m/s")]
        public float SprintSpeed = 5.335f;

        [Tooltip("角色转向面部运动方向的速度")]
        [Range(0.0f, 0.3f)]
        public float RotationSmoothTime = 0.12f;

        [Tooltip("加速和减速")]
        public float SpeedChangeRate = 10.0f;

        // public AudioClip LandingAudioClip;
        // public AudioClip[] FootstepAudioClips;
        // [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

        [Space(10)]
        [Tooltip("跳跃高度")]
        public float JumpHeight = 1.2f;

        [Tooltip("重力")]
        public float Gravity = -15.0f;
        
        [Tooltip("QE look ")]
        public float QElookSpeed = 0.1f;

        [Space(10)]
        [Tooltip("再次跳跃所需的时间。设置为0f可立即再次跳转")]
        public float JumpTimeout = 0.50f;

        [Tooltip("进入下降状态前所需的时间。用于下楼梯")]
        public float FallTimeout = 0.15f;

        [Header("玩家接地板")]
        [Tooltip("检测是否接地")]
        public bool Grounded = true;

        [Tooltip("适用于粗糙地面")]
        public float GroundedOffset = -0.14f;

        [Tooltip("接地检查的半径。应匹配CharacterController的半径")]
        public float GroundedRadius = 0.28f;

        [Tooltip("角色使用哪些层作为地面")]
        public LayerMask GroundLayers;

        [Header("虚拟摄像机")]
        [Tooltip("虚拟摄像机跟随的目标")]
        public GameObject CinemachineCameraTarget;

        [Tooltip("相机向上移动多远（以度为单位）")]
        public float TopClamp = 70.0f;

        [Tooltip("相机向下移动多远（以度为单位）")]
        public float BottomClamp = -30.0f;

        [Tooltip("附加度数以覆盖摄像头。锁定时用于微调相机位置")]
        public float CameraAngleOverride = 0.0f;

        [Tooltip("用于锁定所有轴上的摄像头位置")]
        public bool LockCameraPosition = false;

        [Tooltip("是否移动视角")] 
        public bool hasMoveVisualAngle = false;

        // cinemachine
        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;

        // player
        private float _speed;
        private float _animationBlend;
        private float _targetRotation = 0.0f;
        private float _rotationVelocity;
        private float _verticalVelocity;
        private float _terminalVelocity = 53.0f;

        // timeout deltatime
        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;

        // animation IDs
        private int _animIDSpeed;
        private int _animIDGrounded;
        private int _animIDJump;
        private int _animIDFreeFall;
        private int _animIDMotionSpeed;
        private int _animIDYes;
        private int _animIDNo;
        private int _animIDApplaud;
        private int _animIDSmile;
        private int _animIDDead;

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
        private PlayerInput _playerInput;
#endif
        private Animator _animator;
        private CharacterController _controller;
        private StarterAssetsInputs _input;
        private GameObject _mainCamera;
        private PlayerAgent _playerAgent;

        private const float _threshold = 0.01f;

        private bool _hasAnimator;

        private bool IsCurrentDeviceMouse
        {
            get
            {
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
                return _playerInput.currentControlScheme == "KeyboardMouse";
#else
				return false;
#endif
            }
        }
        

        private void Awake()
        {
            // get a reference to our main camera
            if (_mainCamera == null)
            {
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            }
        }

        private void Start()
        {
            _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;
            
            _playerAgent = GetComponent<PlayerAgent>();
            _hasAnimator = TryGetComponent(out _animator);
            _controller = GetComponent<CharacterController>();
            _input = GetComponent<StarterAssetsInputs>();
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
            _playerInput = GetComponent<PlayerInput>();
#else
			Debug.LogError( "Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
#endif

            AssignAnimationIDs();

            // reset our timeouts on start
            _jumpTimeoutDelta = JumpTimeout;
            _fallTimeoutDelta = FallTimeout;
        }

        private void Update()
        {
            _hasAnimator = TryGetComponent(out _animator);

            JumpAndGravity();
            GroundedCheck();
            Move();
            OtherAction();
        }

        private void LateUpdate()
        {
            CameraRotation();
            // if (Mouse.current.leftButton.isPressed)
            // {
            //     CameraRotation();
            //     Cursor.lockState = CursorLockMode.Locked;
            // }
            // else
            // {
            //     Cursor.lockState = CursorLockMode.None;
            // }
        }

        private void AssignAnimationIDs()
        {
            _animIDSpeed = Animator.StringToHash("Speed");
            _animIDGrounded = Animator.StringToHash("Grounded");
            _animIDJump = Animator.StringToHash("Jump");
            _animIDFreeFall = Animator.StringToHash("FreeFall");
            _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
            _animIDYes = Animator.StringToHash("Yes");
            _animIDNo = Animator.StringToHash("No");
            _animIDApplaud = Animator.StringToHash("Applaud");
            _animIDSmile = Animator.StringToHash("Smile");
            _animIDDead = Animator.StringToHash("Dead");
        }

        private void GroundedCheck()
        {
            // 使用偏移设置球体位置
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
                transform.position.z);
            // 检测球体 检测是否被碰撞
            Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
                QueryTriggerInteraction.Ignore);

            // update animator if using character
            if (_hasAnimator)
            {
                _animator.SetBool(_animIDGrounded, Grounded);
            }
        }

        private void CameraRotation()
        {
            // 如果有输入且摄像头位置不固定 并且鼠标左键按下
            if (_input.look.sqrMagnitude >= _threshold && !LockCameraPosition && Input.GetMouseButton(0))
            {
                //Don't multiply mouse input by Time.deltaTime;
                float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

                _cinemachineTargetYaw += _input.look.x * deltaTimeMultiplier;
                _cinemachineTargetPitch += _input.look.y * deltaTimeMultiplier;
                hasMoveVisualAngle = true;
                StopCoroutine("MyMethod");
                StartCoroutine("MyMethod");
            }else if (Keyboard.current[Key.Q].isPressed)
            {
                float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

                _cinemachineTargetYaw -= QElookSpeed * deltaTimeMultiplier;
                _input.qlook = false;
            }else if (Keyboard.current[Key.E].isPressed)
            {
                float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

                _cinemachineTargetYaw += QElookSpeed * deltaTimeMultiplier;
                _input.elook = false;
            }

            // 夹紧旋转，使我们的值限制为360度
            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

            // 修正虚拟摄像机旋转
            CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
                _cinemachineTargetYaw, 0.0f);
        }

        IEnumerator MyMethod()
        {
            yield return new WaitForSeconds(0.2f);
            hasMoveVisualAngle = false;

        }

        private void Move()
        {
            // 根据移动速度、冲刺速度和是否按下冲刺设置目标速度
            float targetSpeed = _input.sprint ? SprintSpeed : MoveSpeed;

            // 一种简单的加速和减速设计，易于拆卸、更换或重复

            // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // 如果没有输入，将目标速度设置为0
            if (_input.move == Vector2.zero) targetSpeed = 0.0f;
            if (_playerAgent)
            {
                targetSpeed = _playerAgent.agent.hasPath? _playerAgent.agent.velocity.magnitude: targetSpeed;
            }

            // a reference to the players current horizontal velocity
            float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

            float speedOffset = 0.1f;
            float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

            // 加速或减速至目标速度
            if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                // creates curved result rather than a linear one giving a more organic speed change
                // note T in Lerp is clamped, so we don't need to clamp our speed
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                    Time.deltaTime * SpeedChangeRate);

                // 将速度四舍五入到小数点后3位
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = targetSpeed;
            }

            _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
            if (_animationBlend < 0.01f) _animationBlend = 0f;

            // 归一化输入方向
            Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

            // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // 如果有移动输入，则在播放器移动时旋转播放器
            if (_input.move != Vector2.zero)
            {
                _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                  _mainCamera.transform.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                    RotationSmoothTime);

                // 相对于相机位置旋转到面输入方向
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }


            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

            // 移动玩家
            _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) +
                             new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

            // 如果使用角色，则更新animator
            if (_hasAnimator)
            {
                _animator.SetFloat(_animIDSpeed, _animationBlend);
                _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
            }
        }

        private void JumpAndGravity()
        {
            if (Grounded) // 接地
            {
                // 重置下降超时计时器
                _fallTimeoutDelta = FallTimeout;

                // 如果使用角色，则更新animator
                if (_hasAnimator)
                {
                    _animator.SetBool(_animIDJump, false);
                    _animator.SetBool(_animIDFreeFall, false);
                }

                // 阻止我们的速度在着陆时无限下降
                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -2f;
                }

                // Jump
                if (_input.jump && _jumpTimeoutDelta <= 0.0f)
                {
                    // the square root of H * -2 * G = how much velocity needed to reach desired height
                    _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

                    // update animator if using character
                    if (_hasAnimator)
                    {
                        _animator.SetBool(_animIDJump, true);
                    }
                }

                // jump timeout
                if (_jumpTimeoutDelta >= 0.0f)
                {
                    _jumpTimeoutDelta -= Time.deltaTime;
                }
            }
            else
            {
                // reset the jump timeout timer
                _jumpTimeoutDelta = JumpTimeout;

                // fall timeout
                if (_fallTimeoutDelta >= 0.0f)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }
                else
                {
                    // update animator if using character
                    if (_hasAnimator)
                    {
                        _animator.SetBool(_animIDFreeFall, true);
                    }
                }

                // if we are not grounded, do not jump
                _input.jump = false;
            }

            // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += Gravity * Time.deltaTime;
            }
        }

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        // 辅助线
        private void OnDrawGizmosSelected()
        {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            if (Grounded) Gizmos.color = transparentGreen;
            else Gizmos.color = transparentRed;

            // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
            Gizmos.DrawSphere(
                new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z),
                GroundedRadius);
        }

        private void OtherAction()
        {
            if (_hasAnimator)
            {
                _animator.SetBool(_animIDYes, _input.yes);
                _animator.SetBool(_animIDNo, _input.no);
                _animator.SetBool(_animIDApplaud, _input.applaud);
                _animator.SetBool(_animIDSmile, _input.smile);
                _animator.SetBool(_animIDDead, _input.dead);
            }
        }

        private void OnFootstep(AnimationEvent animationEvent)
        {
            // if (animationEvent.animatorClipInfo.weight > 0.5f)
            // {
            //     if (FootstepAudioClips.Length > 0)
            //     {
            //         var index = Random.Range(0, FootstepAudioClips.Length);
            //         AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(_controller.center), FootstepAudioVolume);
            //     }
            // }
        }

        private void OnLand(AnimationEvent animationEvent)
        {
            // if (animationEvent.animatorClipInfo.weight > 0.5f)
            // {
            //     AudioSource.PlayClipAtPoint(LandingAudioClip, transform.TransformPoint(_controller.center), FootstepAudioVolume);
            // }
        }
    }
}