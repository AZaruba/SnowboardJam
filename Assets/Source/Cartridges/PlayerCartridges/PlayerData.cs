﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour {
    
    // TODO: break out data into multiple piles similar to state machines
    #region Members
    [SerializeField] private float TopSpeed;
    [SerializeField] private float Acceleration;
    [SerializeField] private float BrakePower;
    [SerializeField] private float TurnSpeed;
    [SerializeField] private float JumpPower;
    [SerializeField] private float BaseJumpPower;
    [SerializeField] private float JumpChargeRate;
    [SerializeField] private float GravityFactor;
    [SerializeField] private float TerminalVelocity;
    [SerializeField] private float CrashRecoveryTime;
    [SerializeField] private Vector3 BackVectorOffset;
    [SerializeField] private Vector3 FrontVectorOffset;

    private float CurrentSpeed;
    private float CurrentAirVelocity;
    private float CurrentJumpCharge;
    private float CrashTimer;
    private float CurrentRaycastDistance;
    private float CurrentForwardRaycastDistance;

    private Vector3 CurrentPosition;
    private Vector3 CurrentDirection;
    private Vector3 CurrentModelDirection;
    private Vector3 CurrentAirDirection;
    private Vector3 CurrentNormal;
    private Vector3 CurrentDown;

    private Quaternion CurrentRotation;
    #endregion

    #region EngineMembers
    [SerializeField] private float RaycastDistance;
    [SerializeField] private float ForwardRaycastDistance;
    private bool JumpBtnPressed;
    private bool ObstacleInRange;
    private float InputAxisTurn { get; set; }
    private float InputAxisLVert;
    private Vector3 SurfaceNormal { get; set; } // the normal of whatever surfaace we've collided with
    private Vector3 SurfaceAttachPoint { get; set; }
    private Vector3 ObstacleNormal;
    #endregion

    #region SerializedProperties
    public float f_topSpeed
    {
        get { return TopSpeed; }
        set { TopSpeed = value; }
    }

    public float f_acceleration
    {
        get { return Acceleration; }
        set { Acceleration = value; }
    }

    public float f_brakePower
    {
        get { return BrakePower; }
        set { BrakePower = value; }
    }

    public float f_turnSpeed
    {
        get { return TurnSpeed; }
        set { TurnSpeed = value; }
    }

    public float f_jumpPower
    {
        get { return JumpPower; }
        set { JumpPower = value; }
    }

    public float f_baseJumpPower
    {
        get { return BaseJumpPower; }
        set { BaseJumpPower = value; }
    }

    public float f_jumpChargeRate
    {
        get { return JumpChargeRate; }
        set { JumpChargeRate = value; }
    }

    public float f_gravity
    {
        get { return GravityFactor; }
        set { GravityFactor = value; }
    }

    public float f_terminalVelocity
    {
        get { return TerminalVelocity; }
        set { TerminalVelocity = value; }
    }

    public float f_raycastDistance
    {
        get { return RaycastDistance; }
        set { RaycastDistance = value; }
    }

    public float f_forwardRaycastDistance
    {
        get { return ForwardRaycastDistance; }
        set { ForwardRaycastDistance = value; }
    }

    public float f_currentRaycastDistance
    {
        get { return CurrentRaycastDistance; }
        set { CurrentRaycastDistance = value; }
    }

    public float f_currentForwardRaycastDistance
    {
        get { return CurrentForwardRaycastDistance; }
        set { CurrentForwardRaycastDistance = value; }
    }

    public float f_crashRecoveryTime
    {
        get { return CrashRecoveryTime; }
        set { CrashRecoveryTime = value; }
    }
    #endregion
    #region SerializedActives
    public float f_currentSpeed
    {
        get { return CurrentSpeed; }
        set { CurrentSpeed = value; }
    }

    public float f_currentJumpCharge
    {
        get { return CurrentJumpCharge; }
        set { CurrentJumpCharge = value; }
    }

    public float f_currentAirVelocity
    {
        get { return CurrentAirVelocity; }
        set { CurrentAirVelocity = value; }
    }

    public float f_currentCrashTimer
    {
        get { return CrashTimer; }
        set { CrashTimer = value; }
    }
    #endregion
    #region Vectors
    public Vector3 v_currentPosition
    {
        get { return CurrentPosition; }
        set { CurrentPosition = value; }
    }

    public Vector3 v_currentDirection
    {
        get { return CurrentDirection; }
        set { CurrentDirection = value; }
    }

    public Vector3 v_currentModelDirection
    {
        get { return CurrentModelDirection; }
        set { CurrentModelDirection = value; }
    }

    public Vector3 v_currentAirDirection
    {
        get { return CurrentAirDirection; }
        set { CurrentAirDirection = value; }
    }

    public Vector3 v_currentNormal
    {
        get { return CurrentNormal; }
        set { CurrentNormal = value; }
    }

    public Vector3 v_currentDown
    {
        get { return CurrentDown; }
        set { CurrentDown = value; }
    }

    public Vector3 v_frontOffset
    {
        get { return FrontVectorOffset; }
        set { FrontVectorOffset = value; }
    }

    public Vector3 v_backOffset
    {
        get { return BackVectorOffset; }
        set { BackVectorOffset = value; }
    }
    #endregion
    #region Quaternions
    public Quaternion q_currentRotation
    {
        get { return CurrentRotation; }
        set { CurrentRotation = value; }
    }
    #endregion
    #region IOProperties
    public bool b_jumpBtnPressed
    {
        get { return JumpBtnPressed; }
        set { JumpBtnPressed = value; }
    }

    public bool b_obstacleInRange
    {
        get { return ObstacleInRange; }
        set { ObstacleInRange = value; }
    }
    public float f_inputAxisTurn
    {
        get { return InputAxisTurn; }
        set { InputAxisTurn = value; }
    }

    public float f_inputAxisLVert
    {
        get { return InputAxisLVert; }
        set { InputAxisLVert = value; }
    }

    public Vector3 v_currentSurfaceNormal
    {
        get { return SurfaceNormal; }
        set { SurfaceNormal = value; }
    }

    public Vector3 v_currentSurfaceAttachPoint
    {
        get { return SurfaceAttachPoint; }
        set { SurfaceAttachPoint = value; }
    }

    public Vector3 v_currentObstacleNormal
    {
        get { return ObstacleNormal; }
        set { ObstacleNormal = value; }
    }
    #endregion
}
