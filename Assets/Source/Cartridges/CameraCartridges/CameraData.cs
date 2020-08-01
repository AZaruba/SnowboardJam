using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraData : MonoBehaviour {

    #region Members
    [SerializeField] private float FollowHeight;
    [SerializeField] private Vector3 OffsetVector;
    [SerializeField] private Vector3 TargetOffsetVector;
    [SerializeField] private PlayerController PlayerTarget;

    private float FieldOfView;
    private float CameraAngle;
    private float FollowDistance;

    private Vector3 CurrentPosition;
    private Vector3 CurrentDirection;

    private Vector3 TargetPosition;
    private Vector3 TargetDirection;

    private Vector3 SurfaceBelowCameraPosition;

    private Quaternion CameraRotation;
    private Quaternion TargetRotation;
    #endregion

    #region SerializedProperties
    public PlayerController c_targetController
    {
        get { return PlayerTarget; }

    }

    public float f_followDistance
    {
        get { return FollowDistance; }
        set { FollowDistance = value; }
    }

    public float f_followHeight
    {
        get { return FollowHeight; }
        set { FollowHeight = value; }
    }

    public Vector3 v_offsetVector
    {
        get { return OffsetVector; }
        set { OffsetVector = value; }
    }

    public Vector3 v_targetOffsetVector
    {
        get { return TargetOffsetVector; }
        set { TargetOffsetVector = value; }
    }
    #endregion

    #region PrivateProperties
    public float f_fov
    {
        get { return FieldOfView; }
        set { FieldOfView = value; }
    }

    public float f_cameraAngle
    {
        get { return CameraAngle; }
        set { CameraAngle = value; }
    }

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

    public Vector3 v_targetPosition
    {
        get { return TargetPosition; }
        set { TargetPosition = value; }
    }

    public Vector3 v_targetDirection
    {
        get { return TargetDirection; }
        set { TargetDirection = value; }
    }

    public Quaternion q_targetRotation
    {
        get { return TargetRotation; }
        set { TargetRotation = value; }
    }

    public Quaternion q_cameraRotation
    {
        get { return CameraRotation; }
        set { CameraRotation = value; }
    }

    public Vector3 v_surfaceBelowCameraPosition
    {
        get { return SurfaceBelowCameraPosition; }
        set { SurfaceBelowCameraPosition = value; }
    }
    #endregion
}
