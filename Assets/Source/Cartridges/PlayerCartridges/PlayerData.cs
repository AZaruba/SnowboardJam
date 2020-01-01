using System.Collections;
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
    [SerializeField] private float JumpChargeRate;
    [SerializeField] private float GravityFactor;
    [SerializeField] private float TerminalVelocity;
    [SerializeField] private float CrashRecoveryTime;

    private float CurrentSpeed;
    private float CurrentAirVelocity;
    private float CurrentJumpCharge;
    private float CrashTimer;
    private float CurrentRaycastDistance;

    private Vector3 CurrentPosition;
    private Vector3 CurrentDirection;
    private Vector3 CurrentNormal;
    private Vector3 CurrentDown;

    private Quaternion CurrentRotation;
    #endregion

    #region EngineMembers
    [SerializeField] private float RaycastDistance;
    private bool JumpBtnPressed;
    private float InputAxisTurn { get; set; }
    private float InputAxisLVert;
    private Vector3 SurfaceNormal { get; set; } // the normal of whatever surfaace we've collided with
    private Vector3 SurfaceAttachPoint { get; set; }
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

    public float f_currentRaycastDistance
    {
        get { return CurrentRaycastDistance; }
        set { CurrentRaycastDistance = value; }
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
    #endregion
}
