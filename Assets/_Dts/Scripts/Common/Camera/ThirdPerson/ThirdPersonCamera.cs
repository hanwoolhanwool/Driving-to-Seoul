using UnityEngine;
using UnityEngine.InputSystem;

public sealed class ThirdPersonCamera : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;
    [Header("Data")]
    [SerializeField] private ThirdPersonData data;

    private float _yaw;
    private float _pitch;

    private float _yawVel;
    private float _pitchVel;

    private Vector3 _posVel;

    private void Awake()
    {
        if (!target) return;

        Vector3 e = transform.eulerAngles;
        _yaw = e.y;
        _pitch = NormalizePitch(e.x);
    }

    public void _LateUpdate(Vector2 mouseDelta)
    {
        Debug.Log("ThirdPersonCamera");
        if (!target) return;

        float dt = data.IgnoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime;


        float zoom = Mouse.current != null ? Mouse.current.scroll.ReadValue().y * 0.01f : 0f;

        data.Distance = Mathf.Clamp(data.Distance - zoom * 2.0f, data.MinDistance, data.MaxDistance);

        _yaw += mouseDelta.x * data.SensitivityX * dt;
        _pitch -= mouseDelta.y * data.SensitivityY * dt;
        _pitch = Mathf.Clamp(_pitch, data.MinPitch, data.MaxPitch);

        float smYaw = (_yaw);
        float smPitch = (_pitch);

        if (data.RotationSmoothTime > 0f)
        {
            smYaw = Mathf.SmoothDampAngle(transform.eulerAngles.y, _yaw, ref _yawVel, data.RotationSmoothTime, Mathf.Infinity, dt);
            smPitch = Mathf.SmoothDampAngle(NormalizePitch(transform.eulerAngles.x), _pitch, ref _pitchVel, data.RotationSmoothTime, Mathf.Infinity, dt);
        }

        Quaternion rot = Quaternion.Euler(smPitch, smYaw, 0f);

        Vector3 pivot = target.position + data.PivotOffset;

        // 원하는 카메라 위치(충돌 전)
        Vector3 desiredCamPos = pivot - (rot * Vector3.forward) * data.Distance;

        // 충돌 처리: pivot -> desiredCamPos 방향으로 sphere cast
        Vector3 dir = (desiredCamPos - pivot);
        float desiredLen = dir.magnitude;
        if (desiredLen > 0.0001f)
        {
            dir /= desiredLen;

            if (Physics.SphereCast(pivot, data.ProbeRadius, dir, out RaycastHit hit, desiredLen, data.CollisionMask, QueryTriggerInteraction.Ignore))
            {
                float hitDist = Mathf.Max(hit.distance - data.CollisionPadding, 0.0f);
                desiredCamPos = pivot + dir * hitDist;
            }
        }

        // 위치 스무딩
        if (data.PositionSmoothTime > 0f)
            transform.position = Vector3.SmoothDamp(transform.position, desiredCamPos, ref _posVel, data.PositionSmoothTime, Mathf.Infinity, dt);
        else
            transform.position = desiredCamPos;

        transform.rotation = rot;
    }

    private static float NormalizePitch(float x)
    {
        if (x > 180f) x -= 360f;
        return x;
    }
}