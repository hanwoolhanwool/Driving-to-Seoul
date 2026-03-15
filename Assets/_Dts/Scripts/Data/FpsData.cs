using UnityEngine;
[CreateAssetMenu(fileName = "FPSData", menuName = "ScriptableObjects/FPSData")]
public class FpsData : ScriptableObject
{
    [Header("Tuning")] 
    public float SensitivityX;
    public float SensitivityY;
    public float MinPitch;
    public float MaxPitch;

    [Header("Smoothing (0 = off)")] 
    public float SmoothTime;

    [Header("Options")] 
    public bool LockCursor;
    public bool IgnoreTimeScale;
}