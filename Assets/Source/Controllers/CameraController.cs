using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour, iEntityController
{

    #region Members
    [SerializeField] private CameraData c_cameraData;
    [SerializeField] private CameraPreviewData c_previewData;
    [SerializeField] private DebugAccessor debugAccessor;

    private StateData c_stateData;
    private CameraPreviewActiveData c_previewActiveData;
    private CameraLastFrameData c_lastFrameData;
    private CameraPositionData c_positionData;

    private StateMachine sm_cameraBehavior;

    private RaycastHit c_hitOut;

    iMessageClient cl_camera;
    #endregion

    /// <summary>
    /// Start this instance. Initializes all valid states for this object
    /// then adds them to the state machine
    /// </summary>
	void Start()
    {
        SetDefaultCameraData();
        InitializeStateMachine();

        c_stateData = new StateData();
        c_stateData.b_updateState = true;

        cl_camera = new CameraMessageClient(ref c_stateData, ref c_positionData);
        MessageServer.Subscribe(ref cl_camera, MessageID.PAUSE);
        MessageServer.Subscribe(ref cl_camera, MessageID.COUNTDOWN_START);
        MessageServer.Subscribe(ref cl_camera, MessageID.PLAYER_POSITION_UPDATED);
    }

    /// <summary>
    /// Update this instance. States perform actions on data, the data is then
    /// used for object-level functions (such as translations) and then the
    /// state is updated.
    /// </summary>
    void Update()
    {
        if (!c_stateData.b_updateState)
        {
            return;
        }
        EnginePull();

        EngineUpdate();
    }

    /* GOALS:
     * 
     * 1) Message sent every frame that tells the camera what the player's position and rotation is
     * 2) Camera informs normal by projecting player rotation (direction of travel, not model) onto the plane formed by
     *    where the camera is pointing to get surface-relative rotation WITHOUT getting tripped up by spinning
     * 3) The offset should be informed not by rotation but by the frame delta, which prevents the "swinging"
     * 4) The camera should drag going up and down when in the air
     * 
     * 
     * TODOS:
     * - Fix currentTargetTranslation (DONE)
     * - Prevent jerky motion on rotation change - Done by having FocusOnPlayer() not require targetTranslation be nonzero
     * - Find some way to initialize the camera to the right position every time
     * - Adjust offets to make the camera angle look nice after behavior is in place
     * - Find stopping/landing "snap" points and smooth over them (what does it look like if the direction or delta is zero?
     */ 
    void FixedUpdate()
    {
        if (!c_stateData.b_updateState)
        {
            return;
        }
        FixedEnginePull();

        c_lastFrameData.v_lastFramePosition = c_positionData.v_currentPosition;
        c_lastFrameData.q_lastFrameRotation = c_positionData.q_currentRotation;

        c_lastFrameData.v_lastFrameTargetPosition = c_positionData.v_currentTargetPosition;
        c_lastFrameData.v_lastFrameTargetRotation = c_positionData.q_currentTargetRotation;

        UpdateStateMachine();

        sm_cameraBehavior.Act();

    }

    public void EngineUpdate()
    {
        transform.position = Utils.InterpolateFixedVector(c_lastFrameData.v_lastFramePosition, c_positionData.v_currentPosition);
        transform.rotation = Utils.InterpolateFixedQuaternion(c_lastFrameData.q_lastFrameRotation, c_positionData.q_currentRotation);
    }

    public void EnginePull()
    {

    }

    public void FixedEnginePull()
    {
        c_positionData.v_currentTargetTranslation = c_lastFrameData.v_lastFrameTargetPosition - c_positionData.v_currentTargetPosition;
        CheckForGround();
    }

    public void CheckForGround()
    {
        if (Physics.Raycast(c_positionData.v_currentPosition, 
            c_positionData.q_currentTargetRotation * Vector3.down, 
            out c_hitOut,
            c_cameraData.f_followHeight,
            c_cameraData.GroundCollisionMask))
        {
            c_positionData.f_distanceToGround = c_hitOut.distance;
        }
        else
        {
            c_positionData.f_distanceToGround = c_cameraData.f_followHeight;
        }
    }

    public void UpdateStateMachine()
    {
        if (c_stateData.b_preStarted == false)
        {
            sm_cameraBehavior.Execute(Command.START_COUNTDOWN);
            c_stateData.b_preStarted = true;
        }

        if (sm_cameraBehavior.GetCurrentState() == StateRef.PREVIEW_TRACKING)
        {
            if (c_previewActiveData.f_currentShotTime >= c_previewData.PreviewShots[c_previewActiveData.i_currentPreviewIndex].Time)
            {
                sm_cameraBehavior.Execute(Command.REPEAT, false, true);
            }
            return;
        }
    }

    // TODO: rethink and redo the whole state machine
    public void UpdateStateMachineOld()
    {
        Vector3 cameraPosition = c_cameraData.v_currentPosition;
        Vector3 targetPosition = c_cameraData.v_targetPosition;
        float followDistance = c_cameraData.f_followDistance;

        float trueDistance = Vector3.Distance(cameraPosition, targetPosition);

        if (c_stateData.b_preStarted == false)
        {
            sm_cameraBehavior.Execute(Command.START_COUNTDOWN);
            c_stateData.b_preStarted = true;
        }

        if (sm_cameraBehavior.GetCurrentState() == StateRef.PREVIEW_TRACKING)
        {
            if (c_previewActiveData.f_currentShotTime >= c_previewData.PreviewShots[c_previewActiveData.i_currentPreviewIndex].Time)
            {
                sm_cameraBehavior.Execute(Command.REPEAT, false, true);
            }
            return;
        }

        // TODO: Find some check for turning, switch to directed. Switch to targeted otherwise
    }

    #region StartupFunctions

    /// <summary>
    /// Initializes the state machine.
    /// </summary>
    void InitializeStateMachine()
    {
        CameraPreviewState s_preview = new CameraPreviewState(ref c_positionData, ref c_previewData, ref c_previewActiveData);
        CameraFollowTargetState s_followTarget = new CameraFollowTargetState(ref c_cameraData, ref c_positionData);

        sm_cameraBehavior = new StateMachine(s_preview, StateRef.PREVIEW_TRACKING);
        sm_cameraBehavior.AddState(s_followTarget, StateRef.FOLLOWING);
    }

    void SetDefaultCameraData()
    {
        c_previewActiveData = new CameraPreviewActiveData();

        PlayerData playerDataIn = c_cameraData.c_targetController.SharePlayerData();

        c_cameraData.q_cameraRotation = Quaternion.Euler(c_previewData.PreviewShots[c_previewActiveData.i_currentPreviewIndex].CameraAngle);
        c_cameraData.v_currentDirection = c_cameraData.q_cameraRotation * Vector3.forward;
        c_cameraData.v_currentPosition = c_previewData.PreviewShots[c_previewActiveData.i_currentPreviewIndex].StartPosition;


        c_positionData = new CameraPositionData(c_cameraData.v_currentPosition, c_cameraData.q_cameraRotation);

        c_lastFrameData = new CameraLastFrameData();
        c_lastFrameData.v_lastFramePosition = c_cameraData.v_currentPosition;
        c_lastFrameData.q_lastFrameRotation = c_cameraData.q_cameraRotation;

        c_lastFrameData.v_lastFrameTargetPosition = c_positionData.v_currentTargetPosition;
        c_lastFrameData.v_lastFrameTargetRotation = c_positionData.q_currentTargetRotation;
    }
    #endregion
}
