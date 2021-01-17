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

    private StateMachine sm_translation;
    private StateMachine sm_orientation;

    //cartridge list
    private FocusCartridge cart_focus;
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
    void LateUpdate()
    {
        if (!c_stateData.b_updateState)
        {
            return;
        }

        EnginePull();

        UpdateStateMachine();

        sm_translation.Act();
        sm_orientation.Act();

        EngineUpdate();
    }

    public void EngineUpdate()
    {
        //debugAccessor.DisplayState("Camera State", sm_translation.GetCurrentState());
        //debugAccessor.DisplayFloat("Ratio", c_previewActiveData.f_currentShotTime/ c_previewData.PreviewShots[c_previewActiveData.i_currentPreviewIndex].Time);

        transform.position = c_cameraData.v_currentPosition;
        transform.rotation = c_cameraData.q_cameraRotation;
        transform.forward = c_cameraData.v_currentDirection;
    }

    public void EnginePull()
    {
        Vector3 targetPosition = playerTransform.position;
        Quaternion targetRotation = playerTransform.rotation;

        c_cameraData.q_targetRotation = targetRotation;
        c_cameraData.v_targetPosition = targetPosition +
             c_cameraData.q_cameraRotation * c_cameraData.v_targetOffsetVector;

    }

    public void UpdateStateMachine()
    {
        Vector3 cameraPosition = c_cameraData.v_currentPosition;
        Vector3 targetPosition = c_cameraData.v_targetPosition;
        float followDistance = c_cameraData.f_followDistance;

        float trueDistance = Vector3.Distance(cameraPosition, targetPosition);

        if (c_stateData.b_preStarted == false)
        {
            sm_translation.Execute(Command.START_COUNTDOWN);
            sm_orientation.Execute(Command.POINT_AT_POSITION);
            c_stateData.b_preStarted = true;
        }

        if (sm_translation.GetCurrentState() == StateRef.PREVIEW_TRACKING)
        {
            if (c_previewActiveData.f_currentShotTime >= c_previewData.PreviewShots[c_previewActiveData.i_currentPreviewIndex].Time)
            {
                sm_translation.Execute(Command.REPEAT, false, true);
            }
            return;
        }

        if (trueDistance > followDistance)
        {
            sm_translation.Execute(Command.APPROACH);
        }
        else if (trueDistance < followDistance)
        {
            sm_translation.Execute(Command.DRAG);
        }
        else
        {
            // we have achieved balance!
            sm_translation.Execute(Command.TRACK);
        }

        // TODO: Find some check for turning, switch to directed. Switch to targeted otherwise
    }

    #region StartupFunctions

    /// <summary>
    /// Initializes the state machine.
    /// </summary>
    void InitializeStateMachine()
    {
        CameraPreviewState s_preview = new CameraPreviewState(ref c_cameraData, ref c_previewData, ref c_previewActiveData);
        FreeFollowState s_freeFollow = new FreeFollowState(ref c_cameraData, ref cart_angle, ref cart_follow);
        ApproachFollowState s_approachFollow = new ApproachFollowState(ref c_cameraData, ref cart_follow);
        AwayFollowState s_awayFollow = new AwayFollowState(ref c_cameraData, ref cart_follow);

        sm_translation = new StateMachine(s_preview, StateRef.PREVIEW_TRACKING);
        sm_translation.AddState(s_freeFollow, StateRef.TRACKING);
        sm_translation.AddState(s_approachFollow, StateRef.APPROACHING);
        sm_translation.AddState(s_awayFollow, StateRef.LEAVING);

        LookAtDirectionState s_lookDir = new LookAtDirectionState(ref c_cameraData, ref cart_focus);
        LookAtPositionState s_lookPos = new LookAtPositionState(ref c_cameraData, ref cart_focus);
        LookAtTargetState s_lookTarget = new LookAtTargetState(ref c_cameraData, ref cart_focus);

        sm_orientation = new StateMachine(s_lookDir, StateRef.DIRECTED);
        sm_orientation.AddState(s_lookPos, StateRef.POSED);
        sm_orientation.AddState(s_lookTarget, StateRef.TARGETED);
    }

    /// <summary>
    /// Initializes the cartridges.
    /// </summary>
    void InitializeCartridges()
    {
        cart_focus = new FocusCartridge();
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
    }
    #endregion
}
