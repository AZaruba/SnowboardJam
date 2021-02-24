using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPreviewActiveData
{
    public CameraPreviewActiveData()
    {
        f_currentShotTime = Constants.ZERO_F;
        i_currentPreviewIndex = Constants.ZERO;
    }

    public int i_currentPreviewIndex;
    public float f_currentShotTime;
}

public class CameraData : MonoBehaviour {

    #region Members
    [SerializeField] private float FollowHeight; // relative to CURRENT GROUND, not player
    [SerializeField] private float FollowDistance;
    [SerializeField] private Vector3 OffsetVector;
    [SerializeField] private Vector3 TargetOffsetVector;
    [SerializeField] private PlayerController PlayerTarget;
    [SerializeField] public LayerMask GroundCollisionMask;

    [SerializeField] public float MaxOrbitSpeed;
    [SerializeField] public float MaxRotationSpeed;
    [SerializeField] public float MinFollowDistance;
    [SerializeField] public float MaxFollowDistance;
    [SerializeField] public float MaxCameraAngle;
    [SerializeField] public float OffsetDistance;
    [SerializeField] public float OffsetHeight;

    private float FieldOfView;
    private float CameraAngle;

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

    public float f_targetOffset
    {
        get { return OffsetDistance; }
        set { OffsetDistance = value; }
    }
    public float f_offsetHeight
    {
        get { return OffsetHeight; }
        set { OffsetHeight = value; }
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

public class CameraPositionData
{
    public CameraPositionData(Vector3 posIn, Vector3 targetIn, Quaternion rotIn, Quaternion targetRotIn)
    {
        this.v_currentPosition = posIn;
        this.v_currentTargetPosition = targetIn;

        this.q_currentRotation = rotIn;
        this.q_currentTargetRotation = targetRotIn;

        v_currentTargetTranslation = Vector3.zero;
        this.f_distanceToGround = Constants.ZERO_F;
    }

    public Vector3 v_currentPosition;
    public Vector3 v_currentTargetPosition;
    public Vector3 v_currentTargetTranslation;

    public Quaternion q_currentRotation;
    public Quaternion q_currentTargetRotation;

    public float f_currentFollowDistance;
    public float f_distanceToGround;
}

public class CameraLastFrameData
{
    public Vector3 v_lastFramePosition;
    public Quaternion q_lastFrameRotation;

    public Vector3 v_lastFrameTargetPosition;
    public Quaternion v_lastFrameTargetRotation;
}
