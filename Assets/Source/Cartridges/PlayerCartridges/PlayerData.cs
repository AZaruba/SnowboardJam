using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour {

    // Note: these values should be universal base values that will be modified by character stats
    #region Members
    [SerializeField] private float TopSpeed;
    [SerializeField] private float Acceleration;
    [SerializeField] private float StartBoost;
    [SerializeField] private float BrakePower;
    [SerializeField] private float TurnSpeed;
    [SerializeField] private float TurnAcceleration;
    [SerializeField] private float TurnSpeedDeceleration;
    [SerializeField] private float JumpPower;
    [SerializeField] private float BaseJumpPower;
    [SerializeField] private float JumpChargeRate;
    [SerializeField] private float GravityFactor;
    [SerializeField] private float TerminalVelocity;
    [SerializeField] private float CrashRecoveryTime;
    [SerializeField] private float BoostTopSpeed;
    [SerializeField] private float BoostAcceleration;
    [SerializeField] private Vector3 BackVectorOffset;
    [SerializeField] private Vector3 FrontVectorOffset;
    [SerializeField] private Transform CenterOfGravity;

    private float CurrentSpeed;
    private float CurrentTopSpeed;
    private float CurrentAcceleration;
    private float CurrentAirVelocity;
    private float CurrentJumpCharge;
    private float CrashTimer;
    private float CurrentRaycastDistance;
    private float CurrentForwardRaycastDistance;
    private float SurfaceAngleDifference;

    private Vector3 CurrentPosition;
    private Vector3 CurrentModelDirection;
    private Vector3 CurrentAirDirection;
    private Vector3 CurrentNormal;
    private Vector3 CurrentDown;

    private Quaternion CurrentRotation;
    private Quaternion TargetRotation;

    #endregion

    #region EngineMembers
    [SerializeField] private float RaycastDistance;
    [SerializeField] private float ForwardRaycastDistance;
    private bool JumpBtnPressed;
    private bool ObstacleInRange;
    private Vector3 ObstacleNormal;
    #endregion

    #region SerializedProperties
    public float f_topSpeed
    {
        get { return TopSpeed; }
        set { TopSpeed = value; }
    }

    public float f_currentTopSpeed
    {
        get { return CurrentTopSpeed; }
        set { CurrentTopSpeed = value; }
    }

    public float f_acceleration
    {
        get { return Acceleration; }
        set { Acceleration = value; }
    }

    public float f_startBoost
    {
        get { return StartBoost; }
        set { StartBoost = value; }
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

    public float f_turnAcceleration
    {
        get { return TurnAcceleration; }
        set { TurnAcceleration = value; }
    }

    public float f_turnSpeedDeceleration
    {
        get { return TurnSpeedDeceleration; }
        set { TurnSpeedDeceleration = value; }
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

    public float f_surfaceAngleDifference
    {
        get { return SurfaceAngleDifference; }
        set { SurfaceAngleDifference = value; }
    }

    public float f_boostBonus
    {
        get { return BoostTopSpeed; }
        set { BoostTopSpeed = value; }
    }

    public float f_boostAcceleration
    {
        get { return BoostAcceleration; }
        set { BoostAcceleration = value; }
    }
    #endregion
    #region SerializedActives
    public float f_currentSpeed
    {
        get { return CurrentSpeed; }
        set { CurrentSpeed = value; }
    }

    public float f_currentAcceleration
    {
        get { return CurrentAcceleration; }
        set { CurrentAcceleration = value; }
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

    public Quaternion q_targetRotation
    {
        get { return TargetRotation; }
        set { TargetRotation = value; }
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

    public Vector3 v_currentObstacleNormal
    {
        get { return ObstacleNormal; }
        set { ObstacleNormal = value; }
    }
    #endregion

    public Transform t_centerOfGravity
    {
        get { return CenterOfGravity; }
        set { CenterOfGravity = value; }
    }
}

public class PlayerInputData
{
    private float InputAxisLHoriz;
    private float InputAxisLVert;

    public float f_inputAxisLHoriz
    {
        get { return InputAxisLHoriz; }
        set { InputAxisLHoriz = value; }
    }

    public float f_inputAxisLVert
    {
        get { return InputAxisLVert; }
        set { InputAxisLVert = value; }
    }

    /// <summary>
    /// Constructor
    /// </summary>
    public PlayerInputData()
    {
        f_inputAxisLHoriz = 0.0f;
        f_inputAxisLVert = 0.0f;
    }
}

/// <summary>
/// Stores active turn data, with acceleration to give weight
/// </summary>
public class PlayerHandlingData
{
    public float f_currentTurnSpeed;
    public float f_currentRealTurnSpeed;
    public float f_currentSurfaceFactor;
    public float f_turnAcceleration;
    public float f_turnSpeedDeceleration;
    public float f_turnTopSpeed;
    public float f_lastFrameTurnSpeed;
    public float f_turnResetSpeed;

    private float BoostTurnRatio;

    public PlayerHandlingData(float turnSpeedIn, float turnAccelIn, float decelIn, float turnResetIn, float handlingStat)
    {
        this.f_currentTurnSpeed = Constants.ZERO_F;
        this.f_currentRealTurnSpeed = Constants.ZERO_F;
        this.f_currentSurfaceFactor = Constants.ONE;
        this.f_turnAcceleration = turnAccelIn;
        this.f_turnTopSpeed = turnSpeedIn;
        this.f_turnResetSpeed = turnResetIn;
        this.f_turnSpeedDeceleration = decelIn;
        this.f_boostTurnRatio = handlingStat * 0.1f;
    }

    public float f_boostTurnRatio
    {
        get { return BoostTurnRatio; }
        set { BoostTurnRatio = value; }
    }
}

public class LastFramePositionData
{
    public Vector3 v_lastFramePosition;
    public Quaternion q_lastFrameRotation;
    public Quaternion q_lastFrameCoGRotation;
}
