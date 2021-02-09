using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour, iEntityController
{

    #region Members
    [SerializeField] private CameraData c_cameraData;
    [SerializeField] private CameraPreviewData c_previewData;
    [SerializeField] private DebugAccessor debugAccessor;
    [SerializeField] private Transform playerTransform;

    private StateData c_stateData;
    private CameraPreviewActiveData c_previewActiveData;
    private CameraLastFrameData c_lastFrameData;
    private CameraPositionData c_positionData;

    private StateMachine sm_cameraBehavior;

    //cartridge list
    private AngleAdjustmentCartridge cart_angle;
    private FollowCartridge cart_follow;

    iMessageClient cl_camera;
    #endregion

    /// <summary>
    /// Start this instance. Initializes all valid states for this object
    /// then adds them to the state machine
    /// </summary>
	void Start()
    {
        SetDefaultCameraData();
        InitializeCartridges();
        InitializeStateMachine();

        c_stateData = new StateData();
        c_stateData.b_updateState = true;

        cl_camera = new CameraMessageClient(ref c_stateData);
        MessageServer.Subscribe(ref cl_camera, MessageID.PAUSE);
        MessageServer.Subscribe(ref cl_camera, MessageID.COUNTDOWN_START);
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

    }

    void FixedUpdate()
    {
        if (!c_stateData.b_updateState)
        {
            return;
        }

        EnginePull();

        c_lastFrameData.v_lastFramePosition = c_positionData.v_currentPosition;
        c_lastFrameData.q_lastFrameRotation = c_positionData.q_currentRotation;

        c_lastFrameData.v_lastFrameTargetPosition = playerTransform.position;
        c_lastFrameData.v_lastFrameTargetRotation = playerTransform.rotation;

        UpdateStateMachine();

        sm_cameraBehavior.Act();

        EngineUpdate();
    }

    public void EngineUpdate()
    {
        transform.position = Utils.InterpolateFixedVector(c_lastFrameData.v_lastFramePosition, c_positionData.v_currentPosition);
        transform.rotation = Utils.InterpolateFixedQuaternion(c_lastFrameData.q_lastFrameRotation, c_positionData.q_currentRotation);
    }

    public void EnginePull()
    {
        Vector3 targetPosition = playerTransform.position;
        Quaternion targetRotation = playerTransform.rotation;

        c_positionData.v_currentTargetTranslation = targetPosition - c_positionData.v_currentTargetPosition;
        c_positionData.v_currentTargetPosition = targetPosition;
        c_positionData.q_currentTargetRotation = targetRotation;
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

    /// <summary>
    /// Initializes the cartridges.
    /// </summary>
    void InitializeCartridges()
    {
        cart_follow = new FollowCartridge();
        cart_angle = new AngleAdjustmentCartridge();
    }

    void SetDefaultCameraData()
    {
        c_previewActiveData = new CameraPreviewActiveData();

        PlayerData playerDataIn = c_cameraData.c_targetController.SharePlayerData();

        c_cameraData.q_cameraRotation = Quaternion.Euler(c_previewData.PreviewShots[c_previewActiveData.i_currentPreviewIndex].CameraAngle);
        c_cameraData.v_currentDirection = c_cameraData.q_cameraRotation * Vector3.forward;
        c_cameraData.v_currentPosition = c_previewData.PreviewShots[c_previewActiveData.i_currentPreviewIndex].StartPosition;

        c_lastFrameData = new CameraLastFrameData();
        c_lastFrameData.v_lastFramePosition = c_cameraData.v_currentPosition;
        c_lastFrameData.q_lastFrameRotation = c_cameraData.q_cameraRotation;

        c_lastFrameData.v_lastFrameTargetPosition = playerTransform.position;
        c_lastFrameData.v_lastFrameTargetRotation = playerTransform.rotation;

        c_positionData = new CameraPositionData(c_cameraData.v_currentPosition, playerTransform.position, c_cameraData.q_cameraRotation, playerTransform.rotation);
    }
    #endregion
}
