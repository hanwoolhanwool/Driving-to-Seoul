using System;
using UnityEngine;

namespace _Dts.Scripts.Data
{
    [CreateAssetMenu(fileName = "MoveData", menuName = "ScriptableObjects/MoveData")]
    public class MoveData : ScriptableObject
    {
        public event Action OnValuesChanged;
        [Header("Speed")]
        public float WalkSpeed;
        public float SprintSpeed;
        
        [Header("Jump")]
        public float JumpHeight;
        
        [Header("Crouch")]
        public float CrouchWalkSpeed;
        public float CrouchSprintSpeed;
    
        [Header("Acceleration")]
        public float Acceleration;
        public float Deceleration;

        [Header("Rotation")] 
        public float RotationSmoothTime;


        [Header("Gravity")] 
        public float Gravity;
        public float MaxFallSpeed;
        public float GroundedStickForce;


        public void OnValidate()
        {
            OnValuesChanged?.Invoke();
        }
    }
}