using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour {
    
    #region Members
    [SerializeField] private float f_topSpeed;
    [SerializeField] private float f_acceleration;
    [SerializeField] private float f_turnSpeed;
    [SerializeField] private float f_jumpPower;
    [SerializeField] private float f_gravityFactor;

    private float f_currentSpeed;
    private float f_currentJumpPower;
    private float f_currentAirVelocity;

    private Vector3 v_currentPosition;
    private Vector3 v_currentDirection;
    private Vector3 v_currentNormal;

    private Quaternion q_bufferedRotation;
    #endregion

    #region EngineMembers

    private float f_inputAxisTurn { get; set; }
    private Vector3 v_surfaceNormal { get; set; } // the normal of whatever surfaace we've collided with
    private Vector3 v_surfaceAttachPoint { get; set; }
    #endregion

    #region SerializedProperties
    public float TopSpeed
    {
        get { return f_topSpeed; }
        set { f_topSpeed = value; }
    }

    public float Acceleration
    {
        get { return f_acceleration; }
        set { f_acceleration = value; }
    }

    public float TurnSpeed
    {
        get { return f_turnSpeed; }
        set { f_turnSpeed = value; }
    }

    public float JumpPower
    {
        get { return f_jumpPower; }
        set { f_jumpPower = value; }
    }

    public float Gravity
    {
        get { return f_gravityFactor; }
        set { f_gravityFactor = value; }
    }
    #endregion
    #region SerializedActives
    public float CurrentSpeed
    {
        get { return f_currentSpeed; }
        set { f_currentSpeed = value; }
    }

    public float CurrentJumpPower
    {
        get { return f_currentJumpPower; }
        set { f_currentJumpPower = value; }
    }

    public float CurrentAirVelocity
    {
        get { return f_currentAirVelocity; }
        set { f_currentAirVelocity = value; }
    }
    #endregion
    #region Vectors
    public Vector3 CurrentPosition
    {
        get { return v_currentPosition; }
        set { v_currentPosition = value; }
    }

    public Vector3 CurrentDirection
    {
        get { return v_currentDirection; }
        set { v_currentDirection = value; }
    }

    public Vector3 CurrentNormal
    {
        get { return v_currentNormal; }
        set { v_currentNormal = value; }
    }
    #endregion
    #region Quaternions
    public Quaternion RotationBuffer
    {
        get { return q_bufferedRotation; }
        set { q_bufferedRotation = value; }
    }
    #endregion
    #region IOProperties
    public float InputAxisTurn
    {
        get { return f_inputAxisTurn; }
        set { f_inputAxisTurn = value; }
    }

    public Vector3 CurrentSurfaceNormal
    {
        get { return v_surfaceNormal; }
        set { v_surfaceNormal = value; }
    }

    public Vector3 CurrentSurfaceAttachPoint
    {
        get { return v_surfaceAttachPoint; }
        set { v_surfaceAttachPoint = value; }
    }
    #endregion
}
