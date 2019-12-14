using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraData : MonoBehaviour {

    #region Members
    [SerializeField] private float FollowDistance;
    [SerializeField] private Vector3 OffsetVector;
    [SerializeField] private Transform TargetTransform;

    private float FieldOfView;
    private float CameraAngle;

    private Vector3 CurrentPosition;
    private Vector3 CurrentDirection;

    private Vector3 TargetPosition;
    private Vector3 TargetDirection;
    #endregion

    #region SerializedProperties
    public float f_followDistance
    {
        get { return FollowDistance; }
        set { FollowDistance = value; }
    }

    public Vector3 v_offsetVector
    {
        get { return OffsetVector; }
        set { OffsetVector = value; }
    }

    public Transform t_targetTransform
    {
        get { return TargetTransform; }
        set { TargetTransform = value; }
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
    #endregion
}
