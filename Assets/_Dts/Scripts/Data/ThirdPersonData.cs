using UnityEngine;

[CreateAssetMenu(fileName = "ThirdPersonData", menuName = "ScriptableObjects/ThirdPersonData")]
public class ThirdPersonData : ScriptableObject
{
    [Header("Target")]
    public Vector3 PivotOffset = new Vector3(0f, 1.6f, 0f);
    [Header("Orbit")]
    public float Distance = 4f;
    public float MinDistance = 1.5f;
    public float MaxDistance = 8f;
    public float SensitivityX = 220f;
    public float SensitivityY = 180f;
    public float MinPitch = -35f;
    public float MaxPitch = 70f;
    [Header("Smoothing")]
    public float RotationSmoothTime = 0.04f;
    public float PositionSmoothTime = 0.03f;
    [Header("Collision")]
    public float ProbeRadius = 0.25f;
    public float CollisionPadding = 0.1f;
    public LayerMask CollisionMask = ~0;
    [Header("Options")]
    public bool IgnoreTimeScale = true;
}