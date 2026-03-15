using _Dts.Scripts.Data;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMove : MonoBehaviour
{
    [SerializeField] private MoveData moveData;
    [SerializeField] private InputReader inputReader;
    
    private CharacterController _cc;
    private Transform _mainCameraTransform;
    
    private float _walkSpeed;
    private float _sprintSpeed;
    private float _jumpHeight;
    private float _crouchWalkSpeed;
    private float _crouchSprintSpeed;
    private float _acceleration;
    private float _deceleration;
    private float _rotationSmoothTime;
    private float _gravity;
    private float _maxFallSpeed;
    private float _groundedStickForce;

    private float _speed;
    private float _verticalVelocity;
    private float _rotationVelocity;
    
    // State
    private Vector2 _moveInput;
    private bool _isSprinting;
    private bool _isCrouching;

    private void SetValue()
    {
        if (moveData == null)
        {
            GameLogger.LogError("moveData is null");
            return;
        }
        _walkSpeed =  moveData.WalkSpeed;
        _sprintSpeed = moveData.SprintSpeed;
        _jumpHeight = moveData.JumpHeight;
        _crouchWalkSpeed = moveData.CrouchWalkSpeed;
        _crouchSprintSpeed = moveData.CrouchSprintSpeed;
        _acceleration = moveData.Acceleration;
        _deceleration = moveData.Deceleration;
        _rotationSmoothTime = moveData.RotationSmoothTime;
        _gravity = -Mathf.Abs(moveData.Gravity);
        _maxFallSpeed = -Mathf.Abs(moveData.MaxFallSpeed);
        _groundedStickForce = -Mathf.Abs(moveData.GroundedStickForce);
    }
    private void Awake()
    {
        Init();
        SetValue();
    }

    private void Update()
    {
        ProcessMovement();
    }

    private void OnEnable()
    {
        moveData.OnValuesChanged += SetValue;
        inputReader.MoveEvent += HandleMoveInput;
        inputReader.JumpEvent += HandleJump;
        inputReader.SprintEvent += HandleSprint;
        inputReader.SprintCanceledEvent += HandleSprintCanceled;
        inputReader.CrouchEvent += HandleCrouch;
        inputReader.CrouchCanceledEvent += HandleCrouchCanceled;
    }

    private void OnDisable()
    {
        moveData.OnValuesChanged -= SetValue;
        inputReader.MoveEvent -= HandleMoveInput;
        inputReader.JumpEvent -= HandleJump;
        inputReader.SprintEvent -= HandleSprint;
        inputReader.SprintCanceledEvent -= HandleSprintCanceled;
        inputReader.CrouchEvent -= HandleCrouch;
        inputReader.CrouchCanceledEvent -= HandleCrouchCanceled;
    }
    private void Init()
    {
        _cc = GetComponent<CharacterController>();
        if (Camera.main != null)
        {
            _mainCameraTransform = Camera.main.transform;
        }
        else
        {
            GameLogger.LogError("mainCameraTransform is null");
        }
    }
    
#region input handlers
    private void HandleMoveInput(Vector2 input)
    {
        _moveInput = input;
    }
    private void HandleJump()
    {
        if (_cc.isGrounded)
            _verticalVelocity = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
    }

    private void HandleSprint() => _isSprinting = true;
    private void HandleSprintCanceled() => _isSprinting = false;
    private void HandleCrouch() => _isCrouching = true;
    
    private void HandleCrouchCanceled() => _isCrouching = false;
#endregion

    private void ProcessMovement()
    {
        // 목표 속도 설정
        float targetSpeed = GetTargetSpeed();
        // 데드존 임계값 설정
        const float sqrThreshold = 0.01f * 0.01f;
        Vector2 input = _moveInput;
        Vector3 moveDir = Vector3.zero;
        
        if (input.sqrMagnitude > sqrThreshold)
        {
            Vector3 camForward = Vector3.forward;
            Vector3 camRight = Vector3.right;
            
            camForward = _mainCameraTransform.forward;
            camRight = _mainCameraTransform.right;

            camForward.y = 0f;
            camRight.y = 0f;
            
            camForward.Normalize();
            camRight.Normalize();
            
            moveDir = camRight * input.x + camForward * input.y;
        }
        else
        {
            targetSpeed = 0f;
        }
        // 입력을 월드 이동 방향으로 전환
        float inputMagnitude = Mathf.Clamp01(input.magnitude);

        // 입력이 없으면 감속, 있으면 증감 적용
        float rate = targetSpeed == 0f ? _deceleration : _acceleration;
        
        // 속도 계산
        _speed = Mathf.MoveTowards(_speed, targetSpeed * inputMagnitude, rate * Time.deltaTime);
        Vector3 horizontalVelocity = moveDir * _speed;
        TransformRotation();
        // 수직 속도 계산 (중력 계산)
        float verticalVelocity = GetGravity();
        
        // 최종 벡터 병합(최종 속도)
        Vector3 finalVelocity = new Vector3(horizontalVelocity.x, verticalVelocity, horizontalVelocity.z);
        
        _cc.Move(finalVelocity * Time.deltaTime);
    }

    private float GetTargetSpeed()
    {
        float targetSpeed = _isCrouching
            ? (_isSprinting ? _crouchSprintSpeed : _crouchWalkSpeed)
            : (_isSprinting ? _sprintSpeed : _walkSpeed);
        return targetSpeed;
    }
    private float GetGravity()
    {
        if (_cc.isGrounded)
        {
            if (_verticalVelocity < 0f)
            {
                _verticalVelocity = _groundedStickForce;
            }
        }
        else
        {
            _verticalVelocity = Mathf.Max(_verticalVelocity + _gravity * Time.deltaTime, _maxFallSpeed);
        }
        return _verticalVelocity;
    }

    private void TransformRotation()
    {
        float targetYaw = _mainCameraTransform.eulerAngles.y;
        float newYaw = Mathf.SmoothDampAngle(
            transform.eulerAngles.y,
            targetYaw,
            ref _rotationVelocity,
            _rotationSmoothTime
        );
        transform.rotation = Quaternion.Euler(0f, newYaw, 0f);
    }


}
