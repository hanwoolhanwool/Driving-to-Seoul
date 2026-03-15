using _Dts.Scripts.Data;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public enum CameraMode
    {
        Fps = 0,
        Third = 1,
    }

    [Header("Cameras")]
    [SerializeField] private FpsCamera fpsCamera;
    [SerializeField] private ThirdPersonCamera thirdPersonCamera;
    [Header("InputReader")]
    [SerializeField] private InputReader inputReader;
    public CameraMode CurrentCameraMode
    {
        get { return currentCameraMode; }
        set
        {
            if (currentCameraMode == value) return;
            currentCameraMode = value;
            OnCameraModeChanged?.Invoke(value);
        }
    }

    private CameraMode currentCameraMode;
    private event System.Action<CameraMode> OnCameraModeChanged;
    private Vector2 _look = new();

    void OnEnable()
    {
        inputReader.LookEvent += OnLook;
        inputReader.Interact_V_Event += OnInteractV;
    }
    void OnDisable()
    {
        inputReader.LookEvent -= OnLook;
        inputReader.Interact_V_Event -= OnInteractV;

    }

    public void OnLook(Vector2 value)
    {
        _look = value;
    }
    public void OnInteractV()
    {
        CurrentCameraMode =
            CurrentCameraMode == CameraMode.Fps
            ? CameraMode.Third
            : CameraMode.Fps;
    }

    private void Update()
    {
        if (CurrentCameraMode == CameraMode.Fps)
            fpsCamera._Update(_look);
    }
    private void LateUpdate()
    {
        if (CurrentCameraMode == CameraMode.Third)
            thirdPersonCamera._LateUpdate(_look);
    }
}
