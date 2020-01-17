using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour, iEntityController {

    #region Members
    [SerializeField] private CameraData c_cameraData;
    [SerializeField] private DebugAccessor debugAccessor;

    private StateMachine c_StateMachine;

    //cartridge list
    private FocusCartridge cart_focus;
    private AngleAdjustmentCartridge cart_angle;
    private FollowCartridge cart_follow;

    // TEST REMOVE THIS
    iMessageClient cl_camera;
    #endregion

    /// <summary>
    /// Start this instance. Initializes all valid states for this object
    /// then adds them to the state machine
    /// </summary>
	void Start ()
    {
        SetDefaultCameraData();
        InitializeCartridges();
        InitializeStateMachine();

        cl_camera = new CameraMessageClient();
        MessageServer.Subscribe(ref cl_camera);

        cl_camera.SendMessage(MessageID.TEST_MSG_TWO);
    }
	
    /// <summary>
    /// Update this instance. States perform actions on data, the data is then
    /// used for object-level functions (such as translations) and then the
    /// state is updated.
    /// </summary>
	void LateUpdate ()
    {
        // TODO: the camera drags a fair amount behind, what is the reason for this?
        EnginePull();

        UpdateStateMachine();

        c_StateMachine.Act();

        EngineUpdate();
        debugAccessor.DisplayState("Current Camera State", c_StateMachine.GetCurrentState());
    }

    public void EngineUpdate()
    {
        transform.position = c_cameraData.v_currentPosition;
        transform.forward = c_cameraData.v_currentDirection;}

    public void EnginePull()
    {
        c_cameraData.v_targetPosition = c_cameraData.t_targetTransform.position;
        c_cameraData.q_targetRotation = c_cameraData.t_targetTransform.rotation;

        RaycastHit hitInfo;
        if (Physics.Raycast(c_cameraData.v_currentPosition, Vector3.down, out hitInfo, c_cameraData.f_followHeight))
        {
            c_cameraData.v_surfaceBelowCameraPosition = hitInfo.point;
        }
        else
        {
            // TODO: fix this hacky solution for the "no ground below camera" issue
            Vector3 groundlessPosition = c_cameraData.v_currentPosition;
            groundlessPosition.y -= c_cameraData.f_followHeight;
            c_cameraData.v_surfaceBelowCameraPosition = groundlessPosition;
        }
    }

    public void UpdateStateMachine()
    {
        Vector3 cameraPosition = c_cameraData.v_currentPosition;
        Vector3 targetPosition = c_cameraData.v_targetPosition;
        float followDistance = c_cameraData.f_followDistance;

        float trueDistance = Vector3.Distance(cameraPosition, targetPosition);

        if (trueDistance > followDistance)
        {
            c_StateMachine.Execute(Command.APPROACH);
        } 
        else if (trueDistance < followDistance)
        {
            c_StateMachine.Execute(Command.DRAG);
        } 
        else
        {
            // we have achieved balance!
            c_StateMachine.Execute(Command.TRACK);
        }
    }

    #region StartupFunctions

    /// <summary>
    /// Initializes the state machine.
    /// </summary>
    void InitializeStateMachine()
    {
        StationaryLookAtState s_stationary = new StationaryLookAtState (ref c_cameraData, ref cart_focus);
        FreeFollowState s_freeFollow = new FreeFollowState(ref c_cameraData, ref cart_focus, ref cart_angle, ref cart_follow);
        ApproachFollowState s_approachFollow = new ApproachFollowState(ref c_cameraData, ref cart_focus, ref cart_angle, ref cart_follow);
        AwayFollowState s_awayFollow = new AwayFollowState(ref c_cameraData, ref cart_focus, ref cart_angle, ref cart_follow);

        c_StateMachine = new StateMachine (s_stationary, StateRef.STATIONARY);
        c_StateMachine.AddState(s_freeFollow, StateRef.TRACKING);
        c_StateMachine.AddState(s_approachFollow, StateRef.APPROACHING);
        c_StateMachine.AddState(s_awayFollow, StateRef.LEAVING);
    }

    /// <summary>
    /// Initializes the cartridges.
    /// </summary>
    void InitializeCartridges()
    {
        cart_focus = new FocusCartridge ();
        cart_follow = new FollowCartridge();
        cart_angle = new AngleAdjustmentCartridge();
    }

    void SetDefaultCameraData()
    {
        Vector3 targetPosition = c_cameraData.t_targetTransform.position;
        Vector3 targetDirection = c_cameraData.t_targetTransform.forward;
        Quaternion targetRotation = c_cameraData.t_targetTransform.rotation;

        Vector3 cameraPosition = targetPosition -
            targetRotation * (targetDirection.normalized * c_cameraData.f_followDistance) +
            (Vector3.up * c_cameraData.f_followHeight);

        c_cameraData.v_currentPosition = cameraPosition;
        c_cameraData.v_currentDirection = (targetPosition - cameraPosition).normalized;
        c_cameraData.v_targetPosition = targetPosition;
        c_cameraData.v_targetDirection = targetDirection.normalized;
    }
    #endregion
}
