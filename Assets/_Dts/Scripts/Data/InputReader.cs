using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

namespace _Dts.Scripts.Data
{
    [CreateAssetMenu(fileName = "InputReader", menuName = "ScriptableObjects/InputReader")]
    public class InputReader : ScriptableObject, InputSystem_PlayerControls.IPlayerActions
    {
        public event UnityAction<Vector2> MoveEvent;
        public event UnityAction<Vector2> LookEvent;
        public event UnityAction JumpEvent;
        public event UnityAction JumpCanceledEvent;
        
        public event UnityAction AttackEvent;
        public event UnityAction InteractEvent;
        public event UnityAction CrouchEvent;
        public event UnityAction CrouchCanceledEvent; // 앉기 해제 감지용
        public event UnityAction SprintEvent;
        public event UnityAction SprintCanceledEvent; // 달리기 해제 감지용
        public event UnityAction NextEvent;
        public event UnityAction PreviousEvent;
    
        private InputSystem_PlayerControls _inputPC;

        private void OnEnable()
        {
            if (_inputPC == null)
            {
                _inputPC = new InputSystem_PlayerControls();
                _inputPC.Player.SetCallbacks(this);
            }
            _inputPC.Player.Enable();
        }

        private void OnDisable()
        {
            _inputPC.Player.Disable();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            MoveEvent?.Invoke(context.ReadValue<Vector2>());
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            LookEvent?.Invoke(context.ReadValue<Vector2>());
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if(context.phase == InputActionPhase.Performed)
                JumpEvent?.Invoke();
            if(context.phase == InputActionPhase.Canceled)
                JumpCanceledEvent?.Invoke();
        }


        public void OnAttack(InputAction.CallbackContext context)
        {
            AttackEvent?.Invoke();
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if(context.phase == InputActionPhase.Performed)
                InteractEvent?.Invoke();
        }

        public void OnCrouch(InputAction.CallbackContext context)
        {
            if(context.phase == InputActionPhase.Performed)
                CrouchEvent?.Invoke();
            
            if(context.phase == InputActionPhase.Canceled)
                CrouchCanceledEvent?.Invoke();
        }
        public void OnSprint(InputAction.CallbackContext context)
        {
            if(context.performed)
                SprintEvent?.Invoke();
            if(context.phase == InputActionPhase.Canceled)
                SprintCanceledEvent?.Invoke();
        }

        public void OnNext(InputAction.CallbackContext context)
        {
            if(context.phase == InputActionPhase.Performed)
                NextEvent?.Invoke();
        }
        public void OnPrevious(InputAction.CallbackContext context)
        {
            if(context.phase == InputActionPhase.Performed)
                PreviousEvent?.Invoke();
        }
    }
}