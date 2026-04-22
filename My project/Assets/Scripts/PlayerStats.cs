using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Player Movement")]
public class PlayerStats : ScriptableObject 
{
    [Header("Move")]
    [Range(1f, 100f)] public float MoveSpeed = 13f;
    [Range(0.25f, 50f)] public float GroundAcceleration = 5f;
    [Range(0.25f, 50f)] public float GroundDeceleration = 20f;
    [Range(0.25f, 50f)] public float AirAcceleration = 5f;
    [Range(0.25f, 50f)] public float AirDeceleration = 5f;

    [Header("Grounded/Collision Checks")]
    public LayerMask GroundLayer;
    public float GroundDetectionRayLength = 0.02f;
    public float HeadDetectionRayLength = 0.02f;
    [Range(0f, 1f)] public float HeadWidth = 0.75f;

    [Header("Jump")]
    public float JumpHeight = 6.5f;
    [Range(1f, 1.1f)] public float JumpHeightCompensationFactor = 1.054f;
    public float TimeTillJumpApex = 0.5f;
    [Range(0.01f, 5f)] public float GravityOnReleaseMultiplier = 2f;
    public float MaxFallSpeed = 27;
    [Range(1, 5)] public int NumberOfJumpsAllowed = 1;

    [Header("Jump Cut")]
    [Range(0.02f, 0.3f)] public float TimeForUpwardsCancel = 0.027f;
    [Range(0f, 1f)] public float JumpCutMultiplier = 0.7f;

    [Header("Jump Apex")]
    [Range(0.5f, 1f)] public float ApexThreshold = 0.97f;
    [Range(0.01f, 5f)] public float ApexHangTime = 0.075f;

    [Header("Jump Buffer")]
    [Range(0f, 1f)] public float JumpBufferTime = 0.125f;

    [Header("Jump Coyote Time")]
    [Range(0f, 1f)] public float JumpCoyoteTime = 0.1f;



    public float Gravity { get; private set; }

    public float InitialJumpVelocity { get; private set; }
    public float AdjustedJumpHeight { get; private set; }

    private void OnValidate()
    {
        CalculateValues();
    }

    private void OnEnable()
    {
        CalculateValues();
    }

    private void CalculateValues()
    {
        AdjustedJumpHeight = JumpHeight * JumpHeightCompensationFactor;
        Gravity = -(2f * AdjustedJumpHeight) / Mathf.Pow(TimeTillJumpApex, 2f);
        InitialJumpVelocity = Mathf.Abs(Gravity) * TimeTillJumpApex;
    }

    public void ResetToDefaults()
    {
        MoveSpeed = 13f;
        GroundAcceleration = 5f;
        GroundDeceleration = 20f;
        AirAcceleration = 5f;
        AirDeceleration = 5f;
        GroundDetectionRayLength = 0.02f;
        HeadDetectionRayLength = 0.02f;
        HeadWidth = 0.75f;
        JumpHeight = 6.5f;
        JumpHeightCompensationFactor = 1.054f;
        TimeTillJumpApex = 0.5f;
        GravityOnReleaseMultiplier = 2f;
        MaxFallSpeed = 27;
        NumberOfJumpsAllowed = 1;
        TimeForUpwardsCancel = 0.027f;
        JumpCutMultiplier = 0.7f;
        ApexThreshold = 0.97f;
        ApexHangTime = 0.075f;
        JumpBufferTime = 0.125f;
        JumpCoyoteTime = 0.1f;
    }
}
