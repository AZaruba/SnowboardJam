using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrackingData
{
    private Vector3 Position;
    private Vector3 Position_LastFrame;

    public CameraTrackingData()
    {
        this.Position = Vector3.zero;
        this.Position_LastFrame = Vector3.zero;
    }

    public Vector3 v_position
    {
        get { return Position; }
        set { Position = value; }
    }

    public Vector3 v_position_lastFrame
    {
        get { return Position_LastFrame; }
        set { Position_LastFrame = value; }
    }
}

[CreateAssetMenu(fileName = "CameraMotionData", menuName = "Camera Motion Data")]
public class CameraMotionData : ScriptableObject
{
    [SerializeField] private float MaxCameraVerticalVelocity; // how fast the camera can move vertically
    [SerializeField] private float MaxCameraLateralVelocity; // how fast the camera can move horizontally
    [SerializeField] private float MaxCameraRotationalVelocity; // how fast the camera can rotate

    [SerializeField] private float MaxFollowDistance; // the distance at which the camera will bne moving its fastest
    [SerializeField] private float MinFollowDistance; // the distance at which the camera stops moving
    [SerializeField] private float MaxVerticalAngle; // the angle before the camera starts moving vertically down
    [SerializeField] private float MinVerticalAngle; // the angle before the camera starts moving vertically up

    private float CurrentCameraVerticalVelocity;
    private float CurrentCameraLateralVelocity;
    private float CurrentCameraRotationalVelocity;

    public float f_maxVerticalVelocity
    {
        get { return MaxCameraVerticalVelocity; }
    }
    public float f_maxLateralVelocity
    {
        get { return MaxCameraLateralVelocity; }
    }
    public float f_maxRotationalVelocity
    {
        get { return MaxCameraRotationalVelocity; }
    }
    public float f_maxFollowDistance
    {
        get { return MaxFollowDistance; }
    }
    public float f_minFollowDistance
    {
        get { return MinFollowDistance; }
    }
    public float f_maxVerticalAngle
    {
        get { return MaxVerticalAngle; }
    }
    public float f_minVerticalAngle
    {
        get { return MinVerticalAngle; }
    }


    public float f_currentVerticalVelocity
    {
        get { return CurrentCameraVerticalVelocity; }
        set { CurrentCameraVerticalVelocity = value; }
    }
    public float f_currentLateralVelocity
    {
        get { return CurrentCameraLateralVelocity; }
        set { CurrentCameraLateralVelocity = value; }
    }
    public float f_currentRotationalVelocity
    {
        get { return CurrentCameraRotationalVelocity; }
        set { CurrentCameraRotationalVelocity = value; }
    }


}
