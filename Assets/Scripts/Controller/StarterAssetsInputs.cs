using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
    public class StarterAssetsInputs : MonoBehaviour
    {
        [Header("角色移动输入的值")] public Vector2 move;
        public Vector2 look;
        public bool jump;
        public bool sprint;
        public bool yes;
        public bool no;
        public bool applaud;
        public bool smile;
        public bool dead;

        [Header("移动设置")] public bool analogMovement;
        [Tooltip("是否能跳跃")] public bool jumpAllow = true;

        [Header("鼠标光标设置")] [Tooltip("显示鼠标")] public bool cursorLocked = false;
        [Tooltip("不锁定视角")] public bool cursorInputForLook = true;

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
        public void OnMove(InputValue value)
        {
            MoveInput(value.Get<Vector2>());
        }

        public void OnLook(InputValue value)
        {
            if (cursorInputForLook)
            {
                LookInput(value.Get<Vector2>());
            }
        }
        
        public void OnJump(InputValue value)
        {
            if (jumpAllow)
            {
                JumpInput(value.isPressed);
            }
        }

        public void OnSprint(InputValue value)
        {
            SprintInput(value.isPressed);
        }

        public void OnYes(InputValue value)
        {
            YesInput(value.isPressed);
        }

        public void OnNo(InputValue value)
        {
            NoInput(value.isPressed);
        }

        public void OnApplaud(InputValue value)
        {
            ApplaudInput(value.isPressed);
        }

        public void OnSmile(InputValue value)
        {
            SmileInput(value.isPressed);
        }
        public void OnDead(InputValue value)
        {
            DeadInput(value.isPressed);
        }
#endif


        public void MoveInput(Vector2 newMoveDirection)
        {
            move = newMoveDirection;
        }

        public void LookInput(Vector2 newLookDirection)
        {
            look = newLookDirection;
        }

        public void JumpInput(bool newJumpState)
        {
            jump = newJumpState;
        }

        public void SprintInput(bool newSprintState)
        {
            sprint = newSprintState;
        }

        public void YesInput(bool newYesState)
        {
            yes = newYesState;
        }

        public void NoInput(bool newNoState)
        {
            no = newNoState;
        }

        public void ApplaudInput(bool newWaveState)
        {
            applaud = newWaveState;
        }

        public void SmileInput(bool newSmileState)
        {
            smile = newSmileState;
        }
        
        public void DeadInput(bool newSmileState)
        {
            dead = newSmileState;
        }
        
        private void OnApplicationFocus(bool hasFocus)
        {
            SetCursorState(cursorLocked);
        }


        private void SetCursorState(bool newState)
        {
            Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
        }
    }
}