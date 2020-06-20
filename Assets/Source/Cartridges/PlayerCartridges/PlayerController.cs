using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerController : MonoBehaviour, iEntityController {

    #region Members
    // Serialized items
    [SerializeField] private PlayerData c_playerData;
    [SerializeField] private TrickData trickData;
    [SerializeField] private ControllerInputData c_inputData;
    [SerializeField] private Animator PlayerAnimator;
    [SerializeField] private DebugAccessor debugAccessor;

    private StateData c_stateData;
    private AerialMoveData c_aerialMoveData;

    // private members
    private StateMachine c_turnMachine;
    private StateMachine c_airMachine;
    private StateMachine c_accelMachine;
    private StateMachine sm_tricking;

    private PlayerPositionData c_positionData;
    private EntityData c_entityData;

    // cartridge list
    private AccelerationCartridge cart_f_acceleration;
    private VelocityCartridge cart_velocity;
    private HandlingCartridge cart_handling;
    private AngleCalculationCartridge cart_angleCalc;
    private GravityCartridge cart_gravity;
    private IncrementCartridge cart_incr;
    private SurfaceInfluenceCartridge cart_surfInf;

    // Cached Calculation items
    RaycastHit frontHit;
    RaycastHit backHit;
    RaycastHit centerHit;
    RaycastHit forwardHit;

    // TEST REMOVE THIS
    iMessageClient cl_character;
    #endregion

    /// <summary>
    /// Start this instance. Initializes all valid states for this object
    /// then adds them to the state machine
    /// </summary>
    void Start()
    {
        SetDefaultPlayerData();
        cart_gravity = new GravityCartridge();
        cart_angleCalc = new AngleCalculationCartridge();
        cart_velocity = new VelocityCartridge();
        cart_f_acceleration = new AccelerationCartridge();
        cart_handling = new HandlingCartridge();
        cart_incr = new IncrementCartridge();
        cart_surfInf = new SurfaceInfluenceCartridge();

        MoveAerialState s_moveAerial = new MoveAerialState();
        StationaryState s_stationary = new StationaryState(ref c_playerData, ref cart_angleCalc, ref cart_velocity);
        RidingState s_riding = new RidingState(ref c_playerData, ref c_positionData, ref cart_angleCalc, ref cart_f_acceleration, ref cart_velocity, ref cart_surfInf);
        SlowingState s_slowing = new SlowingState(ref c_playerData, ref c_positionData, ref cart_velocity, ref cart_f_acceleration, ref cart_angleCalc, ref cart_surfInf);
        CrashedState s_crashed = new CrashedState(ref c_playerData, ref cart_incr);

        StraightState s_straight = new StraightState(ref c_playerData, ref c_positionData, ref cart_surfInf);
        CarvingState s_carving = new CarvingState(ref c_playerData, ref c_positionData, ref cart_handling, ref cart_surfInf);
        TurnDisabledState s_turnDisabled = new TurnDisabledState();

        AerialState s_aerial = new AerialState(ref c_playerData, ref c_aerialMoveData, ref cart_gravity, ref cart_velocity);
        JumpingState s_jumping = new JumpingState(ref c_playerData, ref cart_gravity, ref cart_velocity);
        GroundedState s_grounded = new GroundedState(ref c_playerData, ref c_positionData, ref cart_velocity, ref cart_angleCalc, ref cart_surfInf);
        JumpChargeState s_jumpCharge = new JumpChargeState(ref c_playerData, ref cart_incr);
        AirDisabledState s_airDisabled = new AirDisabledState();

        c_accelMachine = new StateMachine(s_stationary, StateRef.STATIONARY);
        c_accelMachine.AddState(s_riding, StateRef.RIDING);
        c_accelMachine.AddState(s_slowing, StateRef.STOPPING);
        c_accelMachine.AddState(s_moveAerial, StateRef.AIRBORNE);
        c_accelMachine.AddState(s_crashed, StateRef.CRASHED);

        c_turnMachine = new StateMachine(s_straight, StateRef.RIDING);
        c_turnMachine.AddState(s_carving, StateRef.CARVING);
        c_turnMachine.AddState(s_turnDisabled, StateRef.DISABLED);

        c_airMachine = new StateMachine(s_grounded, StateRef.GROUNDED);
        c_airMachine.AddState(s_aerial, StateRef.AIRBORNE);
        c_airMachine.AddState(s_jumping, StateRef.JUMPING);
        c_airMachine.AddState(s_jumpCharge, StateRef.CHARGING);
        c_airMachine.AddState(s_airDisabled, StateRef.DISABLED);

        InitializeStateMachines();
        InitializeMessageClient();
        EnginePull();
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

        UpdateStateMachine();

        c_accelMachine.Act();
        c_turnMachine.Act();
        c_airMachine.Act();
        sm_tricking.Act();

        EngineUpdate();
    }

    /// <summary>
    /// Updates the object's state within the engine.
    /// </summary>
    public void EngineUpdate()
    {
        transform.position = c_playerData.v_currentPosition;
        transform.rotation = c_positionData.q_currentModelRotation;

        debugAccessor.DisplayState("Ground State", c_accelMachine.GetCurrentState());
        debugAccessor.DisplayVector3("Target dir", c_playerData.v_currentDirection);
        debugAccessor.DisplayFloat("Current Jump Charge", c_playerData.f_currentJumpCharge);

        UpdateAnimator();
    }

    /// <summary>
    /// Updates the animator's states using information from the player data
    /// </summary>
    private void UpdateAnimator()
    {
        PlayerAnimator.SetFloat("TurnAnalogue", c_playerData.f_inputAxisTurn);
        PlayerAnimator.SetBool("JumpPressed", c_airMachine.GetCurrentState() == StateRef.CHARGING);
    }

    /// <summary>
    /// Pulls information from the engine into the controller/data structure
    /// </summary>
    public void EnginePull()
    {
        c_playerData.f_inputAxisTurn = GlobalInputController.GetInputValue(c_inputData.LeftHorizontalAxis);
        c_playerData.f_inputAxisLVert = GlobalInputController.GetInputValue(c_inputData.LeftVerticalAxis);

        // TODO: ensure that we can pull the direction and the normal from the object
        // OTHERWISE it implies that there is a desync between data and the engine
        c_playerData.v_currentPosition = transform.position;
        c_playerData.v_currentNormal = transform.up.normalized;
        c_positionData.q_currentModelRotation = transform.rotation;

        CheckForGround();
        CheckForZone();
        CheckForObstacle();
    }

    /// <summary>
    /// Updates the state machine based on whatever input sources are required
    /// </summary>
    public void UpdateStateMachine()
    {
        if (c_stateData.b_courseFinished == true)
        {
            if (c_playerData.v_currentSurfaceNormal.normalized != Vector3.zero)
            {
                c_accelMachine.Execute(Command.LAND);
                c_turnMachine.Execute(Command.LAND);
                c_airMachine.Execute(Command.LAND);
                sm_tricking.Execute(Command.LAND);
            }
            c_accelMachine.Execute(Command.SLOW);
            c_turnMachine.Execute(Command.RIDE);
            if (c_playerData.f_currentSpeed < 0.0f)
            {
                c_accelMachine.Execute(Command.STOP);
                // we should completely stop do something now that we're done
            }
            return;
        }
        // current issue, these commands don't work out great
        if (c_playerData.v_currentSurfaceNormal.normalized == Vector3.zero)
        {
            c_accelMachine.Execute(Command.FALL);
            c_turnMachine.Execute(Command.FALL);
            c_airMachine.Execute(Command.FALL);
            sm_tricking.Execute(Command.READY_TRICK);
        }
        else
        {
            // should happen before we execute
            if (trickData.i_trickPoints > 0)
            {
                // TODO: Fix rounding
                cl_character.SendMessage(MessageID.SCORE_EDIT, new Message(Mathf.RoundToInt(trickData.i_trickPoints)));
            }
            c_accelMachine.Execute(Command.LAND);
            c_turnMachine.Execute(Command.LAND);
            c_airMachine.Execute(Command.LAND);
            sm_tricking.Execute(Command.LAND);

        }

        if (Mathf.Abs(c_playerData.f_inputAxisTurn) > 0.0f)
        {
            c_turnMachine.Execute(Command.TURN);
        }
        else
        {
            c_turnMachine.Execute(Command.RIDE);
        }

        if (GlobalInputController.GetInputValue(c_inputData.LeftVerticalAxis) < 0.0f)
        {
            c_accelMachine.Execute(Command.SLOW);
        }
        else
        {
            c_accelMachine.Execute(Command.RIDE);
        }
        if (GlobalInputController.GetInputValue(c_inputData.LeftVerticalAxis) > 0.0f)
        {
            c_accelMachine.Execute(Command.STARTMOVE);
        }

        if (c_playerData.f_currentSpeed <= 0.02f)
        {
            c_accelMachine.Execute(Command.STOP);
        }

        // TODO: integrate this keypress into the player data
        if (GlobalInputController.GetInputValue(c_inputData.JumpButton) == KeyValue.PRESSED)
        {
            c_airMachine.Execute(Command.CHARGE);
        }
        else if (GlobalInputController.GetInputValue(c_inputData.JumpButton) == KeyValue.UP)
        {
            c_airMachine.Execute(Command.JUMP);
            c_accelMachine.Execute(Command.JUMP);
            c_turnMachine.Execute(Command.JUMP);
            sm_tricking.Execute(Command.READY_TRICK);
        }

        if (GlobalInputController.GetInputValue(c_inputData.LTrickButton) == KeyValue.PRESSED)
        {
            sm_tricking.Execute(Command.START_TRICK);
            sm_tricking.Execute(Command.SCORE_TRICK);
        }
        else if (GlobalInputController.GetInputValue(c_inputData.LTrickButton) == KeyValue.UP)
        {
            sm_tricking.Execute(Command.END_TRICK);
        }

        if (c_playerData.f_currentCrashTimer > c_playerData.f_crashRecoveryTime)
        {
            c_accelMachine.Execute(Command.READY);
            c_turnMachine.Execute(Command.READY);
            c_airMachine.Execute(Command.READY);
        }
        else if (c_playerData.v_currentObstacleNormal.magnitude > Constants.ZERO_F) // nonzero obstacle normal implies collision
        {
            c_accelMachine.Execute(Command.CRASH);
            c_turnMachine.Execute(Command.CRASH);
            c_airMachine.Execute(Command.CRASH);
        }
    }

    #region StartupFunctions
    /// <summary>
    /// Sets default values for player data that interfaces with the engine, such as the player position
    /// </summary>
    void SetDefaultPlayerData()
    {
        c_positionData = new PlayerPositionData(transform.position, transform.forward);
        c_stateData = new StateData();
        c_aerialMoveData = new AerialMoveData();
        c_entityData = new EntityData();

        c_playerData.v_currentPosition = transform.position;
        c_playerData.q_currentRotation = transform.rotation;
        c_playerData.v_currentDirection = transform.forward;
        c_playerData.v_currentAirDirection = transform.forward;
        c_playerData.v_currentNormal = transform.up;
        c_playerData.v_currentDown = transform.up * -1;
        c_playerData.f_currentSpeed = Constants.ZERO_F;
        c_playerData.f_currentJumpCharge = Constants.ZERO_F;
        c_playerData.f_currentForwardRaycastDistance = c_playerData.f_forwardRaycastDistance;
        c_playerData.f_currentRaycastDistance = c_playerData.f_raycastDistance; 
        c_playerData.f_surfaceAngleDifference = 0.0f;
        c_playerData.b_obstacleInRange = false;
        c_playerData.v_currentForwardNormal = transform.up;

        c_stateData.b_updateState = true;
        c_stateData.b_courseFinished = false;
    }
    #endregion

    /// <summary>
    /// Checks in front of the character for an obstacle in the way.
    /// </summary>
    private void CheckForObstacle()
    {
        // TODO: if above a certain angle, reorient player. If else, crash
        LayerMask lm_env = LayerMask.GetMask("Environment");
        float distance = c_playerData.f_currentForwardRaycastDistance + (c_playerData.f_currentSpeed * Time.deltaTime);
        if (Physics.Raycast(c_playerData.v_currentPosition, c_playerData.v_currentDirection, out forwardHit, distance, lm_env))
        {
            c_playerData.b_obstacleInRange = true;
            c_playerData.v_currentObstacleNormal = forwardHit.normal;
        }
        else
        {
            c_playerData.b_obstacleInRange = false;
            c_playerData.v_currentObstacleNormal = Vector3.zero;
        }
    }
    private void CheckForGround()
    {
        LayerMask lm_env = LayerMask.GetMask("Environment");
        if (Physics.Raycast(c_playerData.v_currentPosition, c_playerData.v_currentDown, out centerHit, c_playerData.f_currentRaycastDistance, lm_env))
        {
            if (Vector3.SignedAngle(centerHit.normal, c_playerData.v_currentSurfaceNormal, transform.right * -1) >
                Mathf.Max(20f / (0.01f + (c_playerData.f_currentSpeed / c_playerData.f_topSpeed)), 0.0f)) // angle should get smaller as we get faster
            {
                c_playerData.v_currentSurfaceNormal = Vector3.zero;
                return;
            }
            else
            {
                c_playerData.v_currentSurfaceAttachPoint = centerHit.point;
                c_playerData.v_currentSurfaceNormal = centerHit.normal;
            }

        }
        else
        {
            // surface normal is vector3.zero if there are no collisions
            c_playerData.v_currentSurfaceNormal = Vector3.zero;
            return;
        }

        Vector3 fwdVec = c_playerData.v_currentDown.normalized + c_playerData.v_currentDirection.normalized;
        if (Physics.Raycast(c_playerData.v_currentPosition, fwdVec, out frontHit, 2f, lm_env))
        {
            // if we have a hit, check to see the difference between sqrt(2) and the hit divided by player height
            float frontHitDist = ((frontHit.point - c_playerData.v_currentPosition).magnitude) / 1.1f; // Height is posing an issue with getting the angle accurate
            c_playerData.v_currentForwardPoint = frontHit.point;
            c_playerData.v_currentForwardNormal = frontHit.normal;
            if (Mathf.Abs(Mathf.Sqrt(2) - frontHitDist) > 0.005f)
            {
                Vector3 resultDir = frontHit.point - c_playerData.v_currentSurfaceAttachPoint;
                c_playerData.f_surfaceAngleDifference = Vector3.SignedAngle(c_playerData.v_currentDirection, resultDir, Vector3.Cross(c_playerData.v_currentNormal, c_playerData.v_currentDirection));
            }
            else
            {
                c_playerData.f_surfaceAngleDifference = 0.0f;
            }
        }
    }

    private void CheckForZone()
    {
        LayerMask lm_zoneMask = LayerMask.GetMask("Zones");
        float distance = c_playerData.f_currentForwardRaycastDistance + (c_playerData.f_currentSpeed * Time.deltaTime);
        if (Physics.Raycast(c_playerData.v_currentPosition, c_playerData.v_currentDirection, out forwardHit, distance, lm_zoneMask))
        {
            // notify that we have collided with a zone, grab the zone's ID and send corresponding message
            ZoneController controller = GameMasterController.LookupZoneController(forwardHit.transform);
            if (controller != null && c_positionData.u_zone != controller.u_zoneId)
            {
                c_positionData.u_zone = controller.u_zoneId;
                ZoneAction();
            }
        }
    }

    private void ZoneAction()
    {
        if (c_positionData.u_zone == uint.MaxValue)
        {
            return;
        }

        if (ZoneController.GetZoneType(c_positionData.u_zone) == ZoneType.FINISH_LINE)
        {
            Message playerFinished = new Message(c_entityData.u_entityID);
            MessageServer.SendMessage(MessageID.PLAYER_FINISHED, playerFinished);
        }
    }

    #region StartupFunctions
    private void InitializeStateMachines()
    {
        InitializeTrickMachine();
    }

    private void InitializeTrickMachine()
    {
        IncrementCartridge cart_increment = new IncrementCartridge();

        TrickDisabledState s_trickDisabled = new TrickDisabledState(ref trickData);
        TrickReadyState s_trickReady = new TrickReadyState();
        TrickTransitionState s_trickTransition = new TrickTransitionState();
        TrickingState s_tricking = new TrickingState(ref trickData, ref cart_increment);

        sm_tricking = new StateMachine(s_trickDisabled, StateRef.TRICK_DISABLED);
        sm_tricking.AddState(s_trickReady, StateRef.TRICK_READY);
        sm_tricking.AddState(s_tricking, StateRef.TRICKING);
        sm_tricking.AddState(s_trickTransition, StateRef.TRICK_TRANSITION);
    }

    private void InitializeMessageClient()
    {
        cl_character = new CharacterMessageClient(ref c_stateData, ref c_entityData);
        MessageServer.Subscribe(ref cl_character, MessageID.PAUSE);
        MessageServer.Subscribe(ref cl_character, MessageID.PLAYER_FINISHED);
    }
    #endregion
}
