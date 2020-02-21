using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour, iEntityController {

    #region Members
    [SerializeField] private CameraData c_cameraData;
    [SerializeField] private DebugAccessor debugAccessor;

    private StateMachine sm_translation;
    private StateMachine sm_orientation;

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
        MessageServer.Subscribe(ref cl_camera, MessageID.TEST_MSG_ONE);

        cl_camera.SendMessage(MessageID.TEST_MSG_TWO, new Message(0.0f));
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

        sm_translation.Act();
        sm_orientation.Act();

        EngineUpdate();
    }

    public void EngineUpdate()
    {
        transform.position = c_cameraData.v_currentPosition;
        transform.forward = c_cameraData.v_currentDirection;
    }

    public void EnginePull()
    {
        c_cameraData.q_targetRotation = c_cameraData.t_targetTransform.rotation;
        c_cameraData.q_cameraRotation = transform.rotation;
        c_cameraData.v_targetPosition = c_cameraData.t_targetTransform.position +
             c_cameraData.q_cameraRotation * c_cameraData.v_targetOffsetVector;

    }

    public void UpdateStateMachine()
    {
        Vector3 cameraPosition = c_cameraData.v_currentPosition;
        Vector3 targetPosition = c_cameraData.v_targetPosition;
        float followDistance = c_cameraData.f_followDistance;

        float trueDistance = Vector3.Distance(cameraPosition, targetPosition);

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
        FreeFollowState s_freeFollow = new FreeFollowState(ref c_cameraData, ref cart_angle, ref cart_follow);
        ApproachFollowState s_approachFollow = new ApproachFollowState(ref c_cameraData, ref cart_follow);
        AwayFollowState s_awayFollow = new AwayFollowState(ref c_cameraData, ref cart_follow);

        sm_translation = new StateMachine(s_freeFollow, StateRef.TRACKING);
        sm_translation.AddState(s_approachFollow, StateRef.APPROACHING);
        sm_translation.AddState(s_awayFollow, StateRef.LEAVING);

        LookAtDirectionState s_lookDir = new LookAtDirectionState(ref c_cameraData, ref cart_focus);
        LookAtPositionState s_lookPos = new LookAtPositionState(ref c_cameraData, ref cart_focus);
        LookAtTargetState s_lookTarget = new LookAtTargetState(ref c_cameraData, ref cart_focus);

        sm_orientation = new StateMachine(s_lookPos, StateRef.POSED);
        sm_orientation.AddState(s_lookDir, StateRef.DIRECTED);
        sm_orientation.AddState(s_lookTarget, StateRef.TARGETED);
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

        Vector3 cameraPosition = targetPosition +
            c_cameraData.q_cameraRotation * c_cameraData.v_targetOffsetVector +
            targetRotation * c_cameraData.v_offsetVector;


        c_cameraData.q_cameraRotation = transform.rotation;
        c_cameraData.v_currentDirection = (targetPosition - cameraPosition).normalized;
        c_cameraData.v_targetPosition = targetPosition +
            c_cameraData.q_cameraRotation * c_cameraData.v_targetOffsetVector;

        c_cameraData.v_currentPosition = cameraPosition;
        c_cameraData.v_targetDirection = targetDirection.normalized;
        c_cameraData.f_followDistance = c_cameraData.v_offsetVector.magnitude;
    }
    #endregion
}

/* Notes:
 *
 * 1) When rotating the camera around a stationary target, it needs to more aggressively pull BEHIND the character rather than over it
 * 2) The camera needs to be more aggressive with pointing toward the character's origin
 *     - the problem is it seems to lag too far behind the target when rotating quickly around it
 *
 * 3)
 *
 * Intended behavior
 *
 * 2) The camera should remain a certain distance off the ground
 *     - An obvious edge case is when we "push" the camera when a wall is in the way
 *     - The solution is to "push" until there is only ground below the camera
 * 4) If there is an object in the way of the camera preventing it from being the
 *    intended distance away from the player, it should be pushed forward to
 *    ensure it can still see the player
 *     - The "dragging" of the camera should prevent this function from causing
 *       a "jump" rather than smooth movement.
 * 6) The camera should have a peak angle, where it will simply translate
 *    more aggressively to compensate
 *    - This ensures that the camera will allow for "legibility" of the landing
 *      space while moving down and continue looking smooth going up
 * 7) The camera should dip while landing to add to the "oomph" of a good trick
 *
 * ENHANCEMENTS:
 *  - The camera should not stay closer to the camera when moving
 */ 
