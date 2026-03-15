using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

namespace _Dts.Scripts.Data
{
    [CreateAssetMenu(fileName = "InputReader", menuName = "ScriptableObjects/InputReader")]
    public class InputReader : ScriptableObject,
        InputSystem_PlayerControls.ICommonActions,
        InputSystem_PlayerControls.IPlayerActions, 
        InputSystem_PlayerControls.ICombatActions,
        InputSystem_PlayerControls.IVehicleActions,
        InputSystem_PlayerControls.IDialogueActions,
        InputSystem_PlayerControls.ISpectatorActions
    {
        // Common
        public event UnityAction PauseEvent;
        
        // Player
        public event UnityAction<Vector2> MoveEvent;
        public event UnityAction<Vector2> LookEvent;
        public event UnityAction CrouchEvent;
        public event UnityAction CrouchCanceledEvent; // 앉기 해제 감지용
        public event UnityAction JumpEvent;
        public event UnityAction JumpCanceledEvent;
        public event UnityAction PreviousEvent;
        public event UnityAction SprintEvent;
        public event UnityAction SprintCanceledEvent; // 달리기 해제 감지용
        public event UnityAction Interact_E_Event;
        public event UnityAction Interact_V_Event;

        // Combat
        public event UnityAction AttackEvent;
        
        // Vehicle
        public event UnityAction<Vector2> DriveMoveEvent;
        public event UnityAction<Vector2> DriveLookEvent;
        public event UnityAction AccelerateEvent;
        public event UnityAction Join_ExitEvent;
        
        // Dialogue
        public event UnityAction NextEvent;
        public event UnityAction SkipEvent;
        public event UnityAction NavigateChoiceEvent;
        public event UnityAction SubmitChoiceEvent;
        
        // Spectator
        public event UnityAction CycleTargetEvent;
        public event UnityAction<Vector2> SpectatorLookEvent;
    
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

        // Common
        public void OnPause(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                PauseEvent?.Invoke();
        }
        // Player
        public void OnMove(InputAction.CallbackContext context)
        {
            MoveEvent?.Invoke(context.ReadValue<Vector2>());
        }
        public void OnLook(InputAction.CallbackContext context)
        {
            LookEvent?.Invoke(context.ReadValue<Vector2>());
        }
        public void OnCrouch(InputAction.CallbackContext context)
        {
            if(context.phase == InputActionPhase.Performed)
                CrouchEvent?.Invoke();
            
            if(context.phase == InputActionPhase.Canceled)
                CrouchCanceledEvent?.Invoke();
        }
        public void OnJump(InputAction.CallbackContext context)
        {
            if(context.phase == InputActionPhase.Performed)
                JumpEvent?.Invoke();
            if(context.phase == InputActionPhase.Canceled)
                JumpCanceledEvent?.Invoke();
        }
        public void OnPrevious(InputAction.CallbackContext context)
        {
            if(context.phase == InputActionPhase.Performed)
                PreviousEvent?.Invoke();
        }
        public void OnSprint(InputAction.CallbackContext context)
        {
            if(context.performed)
                SprintEvent?.Invoke();
            if(context.phase == InputActionPhase.Canceled)
                SprintCanceledEvent?.Invoke();
        }
        public void OnInteractE(InputAction.CallbackContext context)
        {
            if(context.phase == InputActionPhase.Performed)
                Interact_E_Event?.Invoke();
        }
        public void OnInteractV(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                Interact_V_Event?.Invoke();
        }
        // Combat
        public void OnAttack(InputAction.CallbackContext context)
        {
            if(context.phase == InputActionPhase.Performed)
                AttackEvent?.Invoke();
        }
        // Vehicle
        public void OnDriveMove(InputAction.CallbackContext context)
        {
            DriveMoveEvent?.Invoke(context.ReadValue<Vector2>());
        }
        public void OnDriveLook(InputAction.CallbackContext context)
        {
            if(context.phase == InputActionPhase.Performed)
                DriveLookEvent?.Invoke(context.ReadValue<Vector2>());
        }
        public void OnAccelerate(InputAction.CallbackContext context)
        {
            if(context.phase == InputActionPhase.Performed)
                AccelerateEvent?.Invoke();
        }
        public void OnJoin_Exit(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                Join_ExitEvent?.Invoke();
        }
        // Dialogue
        public void OnNext(InputAction.CallbackContext context)
        {
            if(context.phase == InputActionPhase.Performed)
                NextEvent?.Invoke();
        }
        public void OnSkip(InputAction.CallbackContext context)
        {
            SkipEvent?.Invoke();
        }
        public void OnNavigateChoice(InputAction.CallbackContext context)
        {
            if(context.phase == InputActionPhase.Performed)
                NavigateChoiceEvent?.Invoke();
        }
        public void OnSubmitChoice(InputAction.CallbackContext context)
        {
            if(context.phase == InputActionPhase.Performed)
                SubmitChoiceEvent?.Invoke();
        }
        // Spectator
        public void OnCycleTarget(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                CycleTargetEvent?.Invoke();
        }
        public void OnSpectatorLook(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                SpectatorLookEvent?.Invoke(context.ReadValue<Vector2>());
        }
    }
}