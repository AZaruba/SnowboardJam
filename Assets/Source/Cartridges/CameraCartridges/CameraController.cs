using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour, iEntityController {

    #region Members
    [SerializeField] private CameraData c_cameraData;
    [SerializeField] private DebugAccessor debugAccessor;

    private StateMachine c_StateMachine;
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
        sm_orientation.Act();

        EngineUpdate();
    }

    public void EngineUpdate()
    {
        transform.position = c_cameraData.v_currentPosition;
        transform.forward = c_cameraData.v_currentDirection;

        debugAccessor.DisplayState("Camera State", c_StateMachine.GetCurrentState());
    }

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

        // TODO: Find some check for turning, switch to directed. Switch to targeted otherwise
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

/* Notes:
 *
 * 1) When rotating the camera around a stationary target, it needs to more aggressively pull BEHIND the character rather than over it
 * 2) The camera needs to be more aggressive with pointing toward the character's origin
 *     - the problem is it seems to lag too far behind the target when rotating quickly around it
 *
 * 3)
 *
 * Intended behavior
 * 1) Camera should follow behind player's direction of travel
 *     - This is distinct from a generic third person camera controller
 *     - The difference is if the character turns around, the camera should
 *       swing around the radius toward the back of the player
 * 2) The camera should remain a certain distance off the ground
 *     - An obvious edge case is when we "push" the camera when a wall is in the way
 *     - The solution is to "push" until there is only ground below the camera
 * 3) The camera should attempt to be a certain radius from the player at all times
 *     - The radius should be overall distance from the player, which ensures the
 *       camera is always on a sphere around the player
 * 4) If there is an object in the way of the camera preventing it from being the
 *    intended distance away from the player, it should be pushed forward to
 *    ensure it can still see the player
 *     - The "dragging" of the camera should prevent this function from causing
 *       a "jump" rather than smooth movement.
 * 5) The camera should have weight to it, all movement should "lag" behind the
 *    player
 *     - Using linear interpolation, the camera should point to a spot between
 *       the line to the player (dictating its position) and the direction of
 *       the player, providing weight to turns and adding some "spice"
 *     - The position should drag behind the player and drag along any surfaces
 *       it happens to run into, with the intention of making the camera feel
 *       like a separate entity rather than something attached to the player
 *       (which it currently is not)
 * 6) The camera should have a peak angle, where it will simply translate
 *    more aggressively to compensate
 *    - This ensures that the camera will allow for "legibility" of the landing
 *      space while moving down and continue looking smooth going up
 * 7) The camera should dip while landing to add to the "oomph" of a good trick
 * 8) The camera's movement and the camera's direction should be governed by
 *    separate state machines
 */ 
