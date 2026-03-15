using UnityEngine;

public class FpsCamera : MonoBehaviour
{
    [SerializeField] private Transform yawTarget;
    [SerializeField] private Transform pitchTarget;
    [SerializeField] FpsData fpsData;

    private float _yaw;
    private float _pitch;
    
    private float _yawVelocity;
    private float _pitchVelocity;


    void Awake()
    {

        _yaw = yawTarget.eulerAngles.y;
        _pitch = NormalizePitch(pitchTarget.localEulerAngles.x);

        if (fpsData.LockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void _Update(Vector2 mouseDelta)
    {
        Debug.Log("FpsCamera");
        float dt = fpsData.IgnoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime;

        _yaw += mouseDelta.x * fpsData.SensitivityX * dt;
        _pitch += mouseDelta.y * fpsData.SensitivityY * dt;
        _pitch = Mathf.Clamp(_pitch, fpsData.MinPitch, fpsData.MaxPitch);

        if(fpsData.SmoothTime > 0f)
        {
            float sy = Mathf.SmoothDampAngle(yawTarget.eulerAngles.y, _yaw, ref _yawVelocity, fpsData.SmoothTime, Mathf.Infinity, dt);
            float sp = Mathf.SmoothDampAngle(NormalizePitch(pitchTarget.localEulerAngles.x), _pitch, ref _pitchVelocity, fpsData.SmoothTime, Mathf.Infinity, dt);
            
            yawTarget.rotation = Quaternion.Euler(0f, _yaw, 0f);
            pitchTarget.localRotation = Quaternion.Euler(_pitch, 0f, 0f);
        }
    }

    // 로컬 x가 0~ 360으로 튀는걸 -180~ 180으로 정규화
    private static float NormalizePitch(float pitch)
    {
        if (pitch > 180f)
            pitch -= 360f;
        return pitch;
    }
}
