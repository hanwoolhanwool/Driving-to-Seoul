using _Dts.Scripts.Data;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private MoveData moveData;
    [SerializeField] private InputReader inputPC;
    
    private CharacterController _cc;
    private float _walkSpeed;
    private float _sprintSpeed;
    private float _jumpHeight;
    private float _crouchWalkSpeed;
    private float _crouchSprintSpeed;
    private float _acceleration;
    private float _deceleration;
    private float _rotationSmoothTime;
    private float _gravity;
    private float _groundedStickForce;
    private Vector3 _velocity;
    private float _targetRotation;
    private float _rotationVelocity;
    
    private Vector2 _moveInput;

    private void SetValue()
    {
        _walkSpeed =  moveData.WalkSpeed;
        _sprintSpeed = moveData.SprintSpeed;
        _jumpHeight = moveData.JumpHeight;
        _crouchWalkSpeed = moveData.CrouchWalkSpeed;
        _crouchSprintSpeed = moveData.CrouchSprintSpeed;
        _acceleration = moveData.Acceleration;
        _deceleration = moveData.Deceleration;
        _rotationSmoothTime = moveData.RotationSmoothTime;
        _gravity = moveData.Gravity;
        _groundedStickForce = moveData.GroundedStickForce;
    }
    private void Awake()
    {
        _cc = GetComponent<CharacterController>();
    }

    private void Update()
    {
        ApplyMovement();
        ApplyGravity();
    }

    private void OnEnable()
    {
        inputPC.MoveEvent += HandleMoveInput;
        inputPC.JumpEvent += HandleJump;
        inputPC.SprintEvent += HandleSprint;
        inputPC.SprintCanceledEvent += HandleSprintCanceled;
        inputPC.CrouchEvent += HandleCrouch;
        inputPC.CrouchCanceledEvent += HandleCrouchCanceled;
    }

    private void OnDisable()
    {
        inputPC.MoveEvent -= HandleMoveInput;
        inputPC.JumpEvent -= HandleJump;
        inputPC.SprintEvent -= HandleSprint;
        inputPC.SprintCanceledEvent -= HandleSprintCanceled;
        inputPC.CrouchEvent -= HandleCrouch;
        inputPC.CrouchCanceledEvent -= HandleCrouchCanceled;
    }

    private void HandleMoveInput(Vector2 input)
    {
        
    }
    private void HandleJump()
    {
        if (_cc.isGrounded)
        {
            _velocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
        }
    }

    private void ApplyMovement()
    {
        if (_moveInput == Vector2.zero)
            return;
        
        float targetAngle = Mathf.Atan2(_moveInput.y, _moveInput.x) * Mathf.Rad2Deg;
    }

    private void ApplyGravity()
    {
        if (_cc.isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f;
        }
        
        _velocity.y += _gravity * Time.deltaTime;
        _cc.Move(_velocity * Time.deltaTime);
    }
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    private void HandleSprint()
    {
        
    }
    private void HandleSprintCanceled()
    {
        
    }
    private void HandleCrouch()
    {
        
    }
    
    private void HandleCrouchCanceled()
    {
        
    }

    
}
