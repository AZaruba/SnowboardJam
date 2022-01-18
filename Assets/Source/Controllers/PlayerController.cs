using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerController : MonoBehaviour, iEntityController {

    #region Members
    // Serialized items
    [SerializeField] private PlayerData c_playerData;
    [SerializeField] private TrickData trickData;
    [SerializeField] private Animator PlayerAnimator;
    [SerializeField] private DebugAccessor debugAccessor;
    [SerializeField] private CharacterAttributeData Attributes;
    [SerializeField] private CharacterCollisionData CollisionData;
    [SerializeField] private AudioSource AudioSource;
    [SerializeField] private AudioBank SoundBank;
    [SerializeField] private BoxCollider PlayerCollider;
    [SerializeField] private LayerMask GroundCollisionMask;
    [SerializeField] private LayerMask ZoneCollisionMask;

    private StateData c_stateData;
    private AerialMoveData c_aerialMoveData;
    private TrickPhysicsData c_trickPhysicsData;
    private PlayerInputData c_inputData;
    private ScoringData c_scoringData;
    private CollisionData c_collisionData;
    private LastFramePositionData c_lastFrameData;

    // private members
    private StateMachine c_turnMachine;
    private StateMachine c_airMachine;
    private StateMachine c_accelMachine;
    private StateMachine sm_tricking;
    private StateMachine sm_trickPhys;
    private StateMachine sm_switch;

    private PlayerPositionData c_positionData;
    private EntityData c_entityData;
    private PlayerHandlingData c_turnData;

    private AudioController c_audioController;
    private AudioDTree t_audioStateTree;

    private CollisionController c_collisionController;

    // cartridge list
    private IncrementCartridge cart_incr;

    // Cached Calculation items
    RaycastHit frontHit;
    RaycastHit backHit;
    RaycastHit centerHit;
    RaycastHit forwardHit;
    RaycastHit obstacleHit;
    List<Vector3> l_barycentricMeshNormals;
    List<int> l_barycentricMeshIdx;

    iMessageClient cl_character;
    #endregion

    /// <summary>
    /// Start this instance. Initializes all valid states for this object
    /// then adds them to the state machine
    /// </summary>
    void Awake()
    {
        SetDefaultPlayerData();
        cart_incr = new IncrementCartridge();

        InitializeStateMachines();
        InitializeAudioController();
        InitializeMessageClient();
        InitializeCachedLists();
        InitializeCollisionController();

        EnginePull();
        MessageServer.SendMessage(MessageID.PLAYER_POSITION_UPDATED, new Message(c_playerData.v_currentPosition, c_playerData.q_currentRotation)); // NOT the model rotation

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

        EnginePull(); // poll for input every frame

        transform.position = Utils.InterpolateFixedVector(c_lastFrameData.v_lastFramePosition, c_playerData.v_currentPosition);
        transform.rotation = Utils.InterpolateFixedQuaternion(c_lastFrameData.q_lastFrameRotation, c_positionData.q_currentModelRotation);
        c_playerData.t_centerOfGravity.rotation = Utils.InterpolateFixedQuaternion(c_lastFrameData.q_lastFrameCoGRotation, c_positionData.q_currentModelRotation * c_positionData.q_centerOfGravityRotation);
    }

    void FixedUpdate()
    {
        debugAccessor.DisplayFloat("speed", c_playerData.f_currentSpeed);
        debugAccessor.DisplayState("air state", c_airMachine.GetCurrentState());
        if (!c_stateData.b_updateState)
        {
            return;
        }

        c_lastFrameData.v_lastFramePosition = c_playerData.v_currentPosition;
        c_lastFrameData.q_lastFrameRotation = c_positionData.q_currentModelRotation;
        c_lastFrameData.q_lastFrameCoGRotation = c_positionData.q_currentModelRotation * c_positionData.q_centerOfGravityRotation;

        UpdateStateMachine();

        c_airMachine.Act();
        c_accelMachine.Act();
        c_turnMachine.Act();
        sm_tricking.Act();
        sm_trickPhys.Act();

        EngineUpdate();

        LateEnginePull();
        // send normal 
        MessageServer.SendMessage(MessageID.PLAYER_POSITION_UPDATED, new Message(c_playerData.v_currentPosition, c_playerData.q_currentRotation, c_playerData.f_currentSpeed)); // NOT the model rotation INCLUDE AIR STATE
    }

    /// <summary>
    /// Updates the object's state within the engine.
    /// </summary>
    public void EngineUpdate()
    {
        UpdateAnimator();
        UpdateAudio();
    }

    /// <summary>
    /// Pulls data that may be influenced by changes during the current frame.
    /// 
    /// Currently: Collision detection.
    /// </summary>
    public void LateEnginePull()
    {

        float speedRatio = c_playerData.f_currentSpeed;

        c_collisionData.f_obstacleAngle = 20f + CollisionData.BaseObstacleCollisionAngle * (speedRatio / c_playerData.f_topSpeed); // TODO: add "upward motion" weight
        c_collisionData.f_frontRayLengthUp = Mathf.Tan(c_collisionData.f_obstacleAngle*(Mathf.PI/180.0f)) * Time.fixedDeltaTime;
        c_collisionData.f_frontRayLengthDown = (Mathf.Tan(c_collisionData.f_obstacleAngle * (Mathf.PI / 180.0f)) * Time.fixedDeltaTime + c_aerialMoveData.f_verticalVelocity * -1) * Time.fixedDeltaTime;

        c_collisionData.f_obstacleRayLength = speedRatio * Time.fixedDeltaTime; // the expected travel amount next frame
        // CheckForWall();
        CheckForGround3();
    }

    /// <summary>
    /// Updates the animator's states using information from the player data
    /// </summary>
    private void UpdateAnimator()
    {
        PlayerAnimator.SetFloat("TurnAnalogue", c_turnData.f_currentRealTurnSpeed / c_turnData.f_turnTopSpeed);
        PlayerAnimator.SetFloat("SlowAnalogue", c_inputData.f_inputAxisLVert);
        PlayerAnimator.SetBool("JumpPressed", c_airMachine.GetCurrentState() == StateRef.CHARGING);
    }

    /// <summary>
    /// Updates the audio controller with the current state, playing the associated sounds
    /// </summary>
    private void UpdateAudio()
    {
        AudioRef audio = GetAudioState();
        //c_audioController.PlayAudio(GetAudioState());
    }

    /// <summary>
    /// Pulls information from the engine into the controller/data structure
    /// </summary>
    public void EnginePull()
    {
        c_inputData.f_inputAxisLHoriz = GlobalInputController.GetAnalogInputAction(ControlAction.SPIN_AXIS);
        c_inputData.f_inputAxisLVert = GlobalInputController.GetAnalogInputAction(ControlAction.FLIP_AXIS);

        CheckForZone();
    }

    /// <summary>
    /// Updates the state machine based on whatever input sources are required
    /// </summary>
    public void UpdateStateMachine()
    {
        if (c_stateData.b_preStarted == false)
        {
            c_accelMachine.Execute(Command.COUNTDOWN_OVER);
            c_turnMachine.Execute(Command.COUNTDOWN_OVER);
            c_airMachine.Execute(Command.COUNTDOWN_OVER);
            sm_tricking.Execute(Command.COUNTDOWN_OVER);
            sm_trickPhys.Execute(Command.COUNTDOWN_OVER);
            sm_switch.Execute(Command.COUNTDOWN_OVER);
            c_stateData.b_preStarted = true; // we want to execute this only once
        }

        if (c_stateData.b_courseFinished == true)
        {
            c_accelMachine.Execute(Command.SLOW);
            c_turnMachine.Execute(Command.RIDE);
            if (c_playerData.f_currentSpeed < 0.0f)
            {
                c_accelMachine.Execute(Command.STOP);
                // we should completely stop do something now that we're done
            }
            return;
        }
        if (Mathf.Abs(c_inputData.f_inputAxisLHoriz) > 0.0f)
        {
            // catch changing direction between two FixedUpdates
            if (Mathf.Sign(c_turnData.f_currentTurnSpeed) != Mathf.Sign(c_inputData.f_inputAxisLHoriz))
            {
                c_turnMachine.Execute(Command.RIDE);
            }
            c_turnMachine.Execute(Command.TURN);
        }
        
        else
        {
            c_turnMachine.Execute(Command.RIDE);
        }

        if (Mathf.Abs(c_inputData.f_inputAxisLHoriz) <= 0.0f &&
            Mathf.Abs(c_inputData.f_inputAxisLVert) <= 0.0f)
        {
            sm_trickPhys.Execute(Command.SPIN_STOP);
        }

        if (c_inputData.f_inputAxisLVert < 0.0f)
        {
            c_accelMachine.Execute(Command.SLOW);
        }
        else
        {
            c_accelMachine.Execute(Command.RIDE);
        }
        if (c_inputData.f_inputAxisLVert > 0.0f)
        {
            c_accelMachine.Execute(Command.STARTMOVE);
        }

        if (c_playerData.f_currentSpeed <= 0.02f)
        {
            c_accelMachine.Execute(Command.STOP);
        }

        if (GlobalInputController.GetInputAction(ControlAction.CROUCH, KeyValue.PRESSED))
        {
            c_airMachine.Execute(Command.START_BOOST);
        }
        else if (GlobalInputController.GetInputAction(ControlAction.CROUCH, KeyValue.UP))
        {
            c_airMachine.Execute(Command.STOP_BOOST);
        }

        // TODO: split out the sm_trickPhysics machine to ensure that after correcting we can charge the spin again
        if (GlobalInputController.GetInputAction(ControlAction.JUMP, KeyValue.PRESSED))
        {
            c_accelMachine.Execute(Command.CHARGE);
            c_airMachine.Execute(Command.CHARGE);
            c_turnMachine.Execute(Command.CHARGE);
            sm_trickPhys.Execute(Command.CHARGE);
        }
        else if (GlobalInputController.GetInputAction(ControlAction.JUMP, KeyValue.UP))
        {
            c_turnMachine.Execute(Command.JUMP);
            c_airMachine.Execute(Command.JUMP);
            sm_trickPhys.Execute(Command.JUMP);
        }

        UpdateTrickStateMachine();

        if (c_playerData.f_currentCrashTimer > c_playerData.f_crashRecoveryTime)
        {
            c_accelMachine.Execute(Command.READY);
            c_turnMachine.Execute(Command.READY);
            c_airMachine.Execute(Command.READY);
        }
        else if (c_playerData.v_currentObstacleNormal.magnitude > Constants.ZERO_F) // nonzero obstacle normal implies collision
        {
            //c_accelMachine.Execute(Command.CRASH);
            //c_turnMachine.Execute(Command.CRASH);
            //c_airMachine.Execute(Command.CRASH);
            //sm_trickPhys.Execute(Command.CRASH);
        }

        if (c_trickPhysicsData.f_groundResetTarget.Equals(Constants.ZERO_F))
        {
            sm_trickPhys.Execute(Command.SPIN_CORRECT_END);
        }

        UpdateSwitchStateMachine();

    }

    void UpdateSwitchStateMachine()
    {
        float modelAndMotionAngleDifference = Vector3.Angle(c_playerData.q_currentRotation * Vector3.forward,
            c_positionData.q_currentModelRotation * Vector3.forward * c_positionData.i_switchStance);

        if (modelAndMotionAngleDifference > Constants.SWITCH_ANGLE)
        {
            sm_switch.Execute(Command.SWITCH_STANCE);
        }
    }

    #region StartupFunctions
    /// <summary>
    /// Sets default values for player data that interfaces with the engine, such as the player position
    /// </summary>
    void SetDefaultPlayerData()
    {
        c_trickPhysicsData = new TrickPhysicsData(Attributes.Tricks, Attributes.MaxStats);
        c_positionData = new PlayerPositionData(transform.position, transform.forward, transform.rotation);
        c_scoringData = new ScoringData();
        c_inputData = new PlayerInputData();
        c_stateData = new StateData();
        c_aerialMoveData = new AerialMoveData();
        c_entityData = new EntityData();
        c_collisionData = new CollisionData(CollisionData.FrontRayOffset, CollisionData.BackRayOffset);
        c_lastFrameData = new LastFramePositionData();
        c_turnData = new PlayerHandlingData(c_playerData.f_turnSpeed, c_playerData.f_turnAcceleration, c_playerData.f_turnSpeedDeceleration, c_playerData.f_turnAcceleration * 2, this.Attributes.Balance);

        c_playerData.v_currentPosition = transform.position;
        c_playerData.q_currentRotation = transform.rotation;
        c_playerData.q_targetRotation = transform.rotation;
        c_playerData.v_currentAirDirection = transform.forward;
        c_playerData.v_currentNormal = transform.up;
        c_playerData.v_currentDown = transform.up * -1;
        c_playerData.f_currentSpeed = Constants.ZERO_F;
        c_playerData.f_currentAcceleration = c_playerData.f_acceleration;
        c_playerData.f_currentTopSpeed = c_playerData.f_topSpeed;
        c_playerData.f_currentJumpCharge = Constants.ZERO_F;
        c_playerData.f_currentForwardRaycastDistance = c_playerData.f_forwardRaycastDistance;
        c_playerData.f_currentRaycastDistance = c_playerData.f_raycastDistance; 
        c_playerData.f_surfaceAngleDifference = 0.0f;
        c_playerData.b_obstacleInRange = false;

        c_lastFrameData.v_lastFramePosition = transform.position;
        c_lastFrameData.q_lastFrameRotation = transform.rotation;

        c_stateData.b_updateState = true;
        c_stateData.b_courseFinished = false;

    }
    #endregion

    private void OnDrawGizmos()
    {
        //Gizmos.matrix = Matrix4x4.TRS(c_playerData.v_currentPosition, c_playerData.q_currentRotation, transform.lossyScale);
        //Gizmos.color = Color.red;
        //Gizmos.DrawWireCube(CollisionData.CenterOffset, CollisionData.HalfExtents * 2);

        //Gizmos.color = c_airMachine.GetCurrentState() == StateRef.AIRBORNE ? Color.green : Color.blue;
        //Gizmos.DrawWireCube(CollisionData.CenterOffset + (c_playerData.q_currentRotation * Vector3.down * c_aerialMoveData.f_verticalVelocity * Time.fixedDeltaTime * -1), CollisionData.HalfExtents * 2);

    }

    /// <summary>
    /// When we appear to collide with a wall in CheckForGround2(), this handles the behavior from there
    /// </summary>
    private void HandleWallCollision(RaycastHit obstacleHit)
    {
        if (Vector3.Angle(c_playerData.v_currentNormal, obstacleHit.normal) < c_collisionData.f_obstacleAngle)
        {
            return;
        }

        c_collisionData.v_obstacleNormal = Vector3.ProjectOnPlane(obstacleHit.normal.normalized, c_playerData.v_currentNormal).normalized;
        c_collisionData.v_obstaclePoint = obstacleHit.point;

        Vector3 newDir = c_playerData.q_currentRotation * Vector3.forward;
        newDir = Vector3.Reflect(newDir, c_collisionData.v_obstacleNormal).normalized;
        float dirAngle = Vector3.Angle(newDir, c_playerData.q_currentRotation * Vector3.forward);

        c_playerData.q_currentRotation = Quaternion.LookRotation(newDir, c_playerData.q_currentRotation * Vector3.up);

        newDir.y = 0;
        c_aerialMoveData.v_lateralDirection = newDir;
        c_aerialMoveData.f_lateralVelocity *= Mathf.Max((Constants.HALF_ROTATION_F - dirAngle) / Constants.HALF_ROTATION_F, Constants.WALL_BOUNCE_ADJUSTMENT);
        c_playerData.f_currentSpeed *= Mathf.Max((Constants.HALF_ROTATION_F - dirAngle) / Constants.HALF_ROTATION_F, Constants.WALL_BOUNCE_ADJUSTMENT);
    }

    private void CheckForWall()
    {

        /* The ground check goes up f_frontRayLengthUp.
         * We should push the origin UP by q_currentRotation * f_FrontRayLengthUp
         * Then lower the vertical extents by half, effectively keeping the TOP
         * consistent but only pushing the bottom up
         */
        Vector3 playerBox = CollisionData.BodyHalfExtents;
        Vector3 originCenter = Vector3.up * (c_collisionData.f_obstacleRayLength + playerBox.y);

        playerBox.y -= c_collisionData.f_frontRayLengthUp / 2;
        Vector3 offsetOrigin = c_playerData.v_currentPosition + c_playerData.q_currentRotation * originCenter;

        if (Physics.BoxCast(offsetOrigin,
                            playerBox,
                            c_playerData.q_currentRotation * Vector3.forward,
                            out obstacleHit,
                            c_playerData.q_currentRotation,
                            c_collisionData.f_obstacleRayLength,
                            GroundCollisionMask))
        {
            HandleWallCollision(obstacleHit);
        }
    }

    /* 
     * ISSUES:
     * Why are we wiggling to one side or the other on the ground?
     * 
     * If we're accurately moving on the ground, it seems to work great!
     * Down the road: wall collisions
     */  
    private void CheckForGround3()
    {
        if (c_collisionController.CheckForGround4())
        {
            c_playerData.v_currentPosition += c_collisionData.v_attachPoint;

            c_accelMachine.Execute(Command.LAND);
            c_turnMachine.Execute(Command.LAND);
            c_airMachine.Execute(Command.LAND);
            sm_tricking.Execute(Command.LAND);
            sm_trickPhys.Execute(Command.LAND);
        }
        /*
         * Up next, the case that we're on the gorund, not IN it
         */
        else if (c_collisionController.CheckForAir())
        {
            c_playerData.v_currentPosition += c_collisionData.v_attachPoint;
            debugAccessor.DisplayVector3("attachpoint", Quaternion.Inverse(c_playerData.q_currentRotation) * c_collisionData.v_attachPoint);

            // if we hit the range where there's less than a "frame" of motion until the ground, we should still consider that landing
            c_accelMachine.Execute(Command.LAND);
            c_turnMachine.Execute(Command.LAND);
            c_airMachine.Execute(Command.LAND);
            sm_tricking.Execute(Command.LAND);
            sm_trickPhys.Execute(Command.LAND);
        }
        else
        {
            c_collisionData.v_attachPoint = Vector3.zero;

            c_accelMachine.Execute(Command.FALL);
            c_turnMachine.Execute(Command.FALL);
            c_airMachine.Execute(Command.FALL);
            sm_trickPhys.Execute(Command.FALL);
            sm_tricking.Execute(Command.READY_TRICK);
        }
        /*
        else if (!c_collisionController.CheckForAir())
        {
            debugAccessor.DisplayString("NOT DETECTIN'");
            debugAccessor.DisplayVector3("AIR CHECK AERIAL", Vector3.zero);
            c_collisionData.v_attachPoint = Vector3.zero;

            c_accelMachine.Execute(Command.FALL);
            c_turnMachine.Execute(Command.FALL);
            c_airMachine.Execute(Command.FALL);
            sm_trickPhys.Execute(Command.FALL);
            sm_tricking.Execute(Command.READY_TRICK);
        }
        else
        {
            debugAccessor.DisplayString("NOT DETECTIN'");
            debugAccessor.DisplayVector3("AIR CHECK GROUNDED", Vector3.zero);
            c_playerData.v_currentPosition += c_collisionData.v_attachPoint;

            c_accelMachine.Execute(Command.LAND);
            c_turnMachine.Execute(Command.LAND);
            c_airMachine.Execute(Command.LAND);
            sm_tricking.Execute(Command.LAND);s
            sm_trickPhys.Execute(Command.LAND);

        }
        */
        if (c_collisionController.CheckGroundRotation())
        {
            // force player rotation before checking

            // is this wrong?
            Vector3 projectedRotation = Vector3.ProjectOnPlane(c_positionData.q_currentModelRotation * Vector3.forward, c_collisionData.v_surfaceNormal);
            c_positionData.q_currentModelRotation = Quaternion.LookRotation(projectedRotation, c_collisionData.v_surfaceNormal);
            c_playerData.q_currentRotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(c_playerData.q_currentRotation * Vector3.forward, c_collisionData.v_surfaceNormal));
        }
    }

    private void CheckForZone()
    {
        float distance = c_playerData.f_currentForwardRaycastDistance + (c_playerData.f_currentSpeed * Time.fixedDeltaTime);
        if (Physics.Raycast(c_playerData.v_currentPosition, c_playerData.q_currentRotation * Vector3.forward, out forwardHit, distance, ZoneCollisionMask))
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
        InitializeAccelMachine();
        InitializeAirMachine();
        InitializeTurnMachine();
        InitializeTrickMachine();
        InitializeTrickPhysicsMachine();
        InitializeSwitchMachine();
    }

    private void InitializeAccelMachine()
    {
        MoveAerialState s_moveAerial = new MoveAerialState();
        StationaryState s_stationary = new StationaryState(ref c_playerData, ref c_positionData);
        RidingState s_riding = new RidingState(ref c_playerData, ref c_positionData, ref c_collisionData);
        RidingChargeState s_ridingCharge = new RidingChargeState(ref c_playerData, ref c_positionData, ref c_collisionData);
        SlowingState s_slowing = new SlowingState(ref c_playerData, ref c_collisionData, ref c_inputData, ref c_positionData);
        CrashedState s_crashed = new CrashedState(ref c_playerData, ref cart_incr);

        c_accelMachine = new StateMachine(StateRef.STATIONARY);
        c_accelMachine.AddState(s_stationary, StateRef.STATIONARY);
        c_accelMachine.AddState(s_riding, StateRef.RIDING);
        c_accelMachine.AddState(s_slowing, StateRef.STOPPING);
        c_accelMachine.AddState(s_moveAerial, StateRef.AIRBORNE);
        c_accelMachine.AddState(s_crashed, StateRef.CRASHED);
        c_accelMachine.AddState(s_ridingCharge, StateRef.CHARGING);


    }

    private void InitializeAirMachine()
    {
        AerialState s_aerial = new AerialState(ref c_playerData, ref c_collisionData, ref c_aerialMoveData, ref c_positionData);
        GroundedState s_grounded = new GroundedState(ref c_playerData, ref c_aerialMoveData, ref c_collisionData, ref c_positionData);
        BoostState s_boost = new BoostState(ref c_playerData, ref c_aerialMoveData, ref c_collisionData, ref c_positionData, ref c_turnData);
        JumpChargeState s_jumpCharge = new JumpChargeState(ref c_playerData, ref c_positionData, ref c_collisionData, ref c_aerialMoveData, ref cart_incr);
        AirDisabledState s_airDisabled = new AirDisabledState();

        c_airMachine = new StateMachine(StateRef.GROUNDED);
        c_airMachine.AddState(s_grounded, StateRef.GROUNDED);
        c_airMachine.AddState(s_aerial, StateRef.AIRBORNE);
        c_airMachine.AddState(s_jumpCharge, StateRef.CHARGING);
        c_airMachine.AddState(s_airDisabled, StateRef.DISABLED);
        c_airMachine.AddState(s_boost, StateRef.GROUNDED_BOOSTING);
    }

    private void InitializeTurnMachine()
    {
        StraightState s_straight = new StraightState(ref c_playerData, ref c_turnData, ref c_positionData);
        CarvingState s_carving = new CarvingState(ref c_playerData, ref c_turnData, ref c_inputData, ref c_positionData);
        TurnDisabledState s_turnDisabled = new TurnDisabledState();
        TurnChargeState s_turnCharge = new TurnChargeState(ref c_playerData, ref c_turnData, ref c_positionData);

        c_turnMachine = new StateMachine(StateRef.RIDING);
        c_turnMachine.AddState(s_straight, StateRef.RIDING);
        c_turnMachine.AddState(s_carving, StateRef.CARVING);
        c_turnMachine.AddState(s_turnCharge, StateRef.CHARGING);
        c_turnMachine.AddState(s_turnDisabled, StateRef.DISABLED);

    }

    private void InitializeTrickPhysicsMachine()
    {
        SpinIdleState s_spinIdle = new SpinIdleState(ref c_trickPhysicsData, ref c_positionData);
        SpinChargeState s_spinCharge = new SpinChargeState(ref c_trickPhysicsData, ref c_inputData, ref cart_incr);
        SpinningState s_spinning = new SpinningState(ref c_trickPhysicsData, ref c_positionData, ref cart_incr);
        SpinSnapState s_spinSnap = new SpinSnapState(ref c_aerialMoveData, ref c_positionData, ref c_trickPhysicsData, ref c_scoringData);
        SpinCorrectState s_spinCorrect = new SpinCorrectState(ref c_trickPhysicsData, ref c_playerData, ref c_positionData);

        sm_trickPhys = new StateMachine(StateRef.SPIN_IDLE);
        sm_trickPhys.AddState(s_spinIdle, StateRef.SPIN_IDLE);
        sm_trickPhys.AddState(s_spinCharge, StateRef.SPIN_CHARGE);
        sm_trickPhys.AddState(s_spinning, StateRef.SPINNING);
        sm_trickPhys.AddState(s_spinSnap, StateRef.SPIN_RESET);
        sm_trickPhys.AddState(s_spinCorrect, StateRef.SPIN_CORRECT);
    }

    private void InitializeTrickMachine()
    {
        TrickDisabledState s_trickDisabled = new TrickDisabledState(ref trickData, ref c_scoringData);
        TrickReadyState s_trickReady = new TrickReadyState();
        TrickTransitionState s_trickTransition = new TrickTransitionState(ref trickData, ref c_scoringData);
        TrickingState s_tricking = new TrickingState(ref trickData, ref c_scoringData, ref cart_incr);

        sm_tricking = new StateMachine(StateRef.TRICK_DISABLED);
        sm_tricking.AddState(s_trickDisabled, StateRef.TRICK_DISABLED);
        sm_tricking.AddState(s_trickReady, StateRef.TRICK_READY);
        sm_tricking.AddState(s_tricking, StateRef.TRICKING);
        sm_tricking.AddState(s_trickTransition, StateRef.TRICK_TRANSITION);
    }

    private void InitializeSwitchMachine()
    {
        ForwardState s_forward = new ForwardState(ref c_positionData);
        SwitchState s_switch = new SwitchState(ref c_positionData);

        sm_switch = new StateMachine(StateRef.FORWARD_STANCE);
        sm_switch.AddState(s_forward, StateRef.FORWARD_STANCE);
        sm_switch.AddState(s_switch, StateRef.SWITCH_STANCE);
    }

    private void InitializeMessageClient()
    {
        cl_character = new CharacterMessageClient(ref c_stateData, ref c_entityData, ref c_audioController);
        MessageServer.Subscribe(ref cl_character, MessageID.PAUSE);
        MessageServer.Subscribe(ref cl_character, MessageID.PLAYER_FINISHED);
        MessageServer.Subscribe(ref cl_character, MessageID.COUNTDOWN_OVER);
    }

    private void InitializeCachedLists()
    {
        l_barycentricMeshIdx = new List<int>();
        l_barycentricMeshNormals = new List<Vector3>();
    }

    private void InitializeCollisionController()
    {
        this.c_collisionController = new CollisionController(ref c_playerData,
            ref c_positionData,
            ref c_collisionData,
            ref CollisionData,
            ref c_aerialMoveData,
            ref PlayerCollider,
            GroundCollisionMask,
            ZoneCollisionMask);
    }

    private void InitializeAudioController()
    {
        c_audioController = new AudioController(SoundBank.AudioReferences, SoundBank.AudioClips, ref AudioSource);
        BuildAudioStateTree();
    }
    #endregion

    #region DataSharingFunctions
    public PlayerPositionData SharePositionData()
    {
        return this.c_positionData;
    }

    public PlayerData SharePlayerData()
    {
        return this.c_playerData;
    }
    #endregion

    #region MessageSendFunctions
    private void CompileAndSendScore()
    {
        if (c_scoringData.l_trickList.Count == 0 &&
            c_scoringData.f_currentFlipTarget.Equals(Constants.ZERO_F) &&
            c_scoringData.f_currentSpinTarget.Equals(Constants.ZERO_F))
        {
            ResetScoringData();
            return;
        }
        TrickMessageData trickDataOut = new TrickMessageData();
        trickDataOut.FlipDegrees = c_scoringData.f_currentFlipTarget;
        trickDataOut.SpinDegrees = c_scoringData.f_currentSpinTarget;
        trickDataOut.FlipAngle = 0.0f;
        trickDataOut.grabs = c_scoringData.l_trickList;
        trickDataOut.grabTimes = c_scoringData.l_timeList;
        trickDataOut.Success = true; // TODO: implement bails

        MessageServer.SendMessage(MessageID.TRICK_FINISHED, new Message(trickDataOut));
        MessageServer.SendMessage(MessageID.SCORE_EDIT, new Message(0));

        ResetScoringData();
    }

    private void ResetScoringData()
    {
        c_scoringData.f_currentFlipTarget = 0.0f;
        c_scoringData.f_currentSpinTarget = 0.0f;
        c_scoringData.l_timeList.Clear();
        c_scoringData.l_trickList.Clear();
        c_scoringData.b_sendTrick = false;
    }
    #endregion

    private void UpdateTrickStateMachine()
    {
        bool TrickHit = false;
        if (!TrickHit && GlobalInputController.GetInputAction(ControlAction.LEFT_GRAB, KeyValue.PRESSED))
        {
            TrickHit = true;
            trickData.k_activeTrickAction = ControlAction.LEFT_GRAB;
            trickData.t_activeTrickName = trickData.trick_left;
        }
        if (!TrickHit && GlobalInputController.GetInputAction(ControlAction.UP_GRAB, KeyValue.PRESSED))
        {
            TrickHit = true;
            trickData.k_activeTrickAction = ControlAction.UP_GRAB;
            trickData.t_activeTrickName = trickData.trick_up;
        }

        if (!TrickHit && GlobalInputController.GetInputAction(ControlAction.RIGHT_GRAB, KeyValue.PRESSED))
        {
            TrickHit = true;
            trickData.k_activeTrickAction = ControlAction.RIGHT_GRAB;
            trickData.t_activeTrickName = trickData.trick_right;
        }

        if (!TrickHit && GlobalInputController.GetInputAction(ControlAction.DOWN_GRAB, KeyValue.PRESSED))
        {
            TrickHit = true;
            trickData.k_activeTrickAction = ControlAction.DOWN_GRAB;
            trickData.t_activeTrickName = trickData.trick_down;
        }

        if (TrickHit)
        {
            sm_tricking.Execute(Command.START_TRICK);
            sm_tricking.Execute(Command.SCORE_TRICK);
        }
        else if (GlobalInputController.GetInputAction(trickData.k_activeTrickAction, KeyValue.UP))
        {
            sm_tricking.Execute(Command.END_TRICK);
        }

    }

    public AudioRef GetAudioState()
    {
        AudioRef foundClip = AudioRef.ERROR_CLIP;

        StateRef airState = c_airMachine.GetCurrentState();
        StateRef rideState = c_accelMachine.GetCurrentState();
        StateRef turnState = c_turnMachine.GetCurrentState();
        StateRef groundState = StateRef.TERR_SNOW; // currently always snow

        // obey the order of the tree: air, ground, turn, terre
        AudioDTree currentBranch = t_audioStateTree.GetChild(airState);
        currentBranch = currentBranch.GetChild(rideState);
        currentBranch = currentBranch.GetChild(turnState);
        currentBranch = currentBranch.GetChild(groundState);

        foundClip = currentBranch.GetLeaf();

        return foundClip;
    }

    public void BuildAudioStateTree()
    {
        t_audioStateTree = new AudioDTree(StateRef.TREE_ROOT);

        // terrain state, end
        AudioDTree powderRideLeaf = new AudioDTree(StateRef.TERR_POWDER, AudioRef.RIDE_POWDER);
        AudioDTree snowRideLeaf = new AudioDTree(StateRef.TERR_SNOW, AudioRef.RIDE_SNOW);
        AudioDTree iceRideLeaf = new AudioDTree(StateRef.TERR_ICE, AudioRef.RIDE_ICE);

        AudioDTree powderTurnLeaf = new AudioDTree(StateRef.TERR_POWDER, AudioRef.TURN_POWDER);
        AudioDTree snowTurnLeaf = new AudioDTree(StateRef.TERR_SNOW, AudioRef.TURN_SNOW);
        AudioDTree iceTurnLeaf = new AudioDTree(StateRef.TERR_ICE, AudioRef.TURN_ICE);

        AudioDTree powderSlowLeaf = new AudioDTree(StateRef.TERR_POWDER, AudioRef.SLOW_POWDER);
        AudioDTree snowSlowLeaf = new AudioDTree(StateRef.TERR_SNOW, AudioRef.SLOW_SNOW);
        AudioDTree iceSlowLeaf = new AudioDTree(StateRef.TERR_ICE, AudioRef.SLOW_ICE);

        // turn state
        AudioDTree turnStraightBranch = new AudioDTree(StateRef.RIDING);
        AudioDTree turnCarvingBranch = new AudioDTree(StateRef.CARVING);

        // ground state
        AudioDTree ridingBranch = new AudioDTree(StateRef.RIDING);
        AudioDTree slowingBranch = new AudioDTree(StateRef.STOPPING);
        AudioDTree stoppedBranch = new AudioDTree(StateRef.STOPPING, AudioRef.NO_AUDIO);

        // airState
        AudioDTree groundedBranch = new AudioDTree(StateRef.GROUNDED);
        AudioDTree airborneBranch = new AudioDTree(StateRef.AIRBORNE, AudioRef.NO_AUDIO);

        slowingBranch.AssignChild(ref powderSlowLeaf);
        slowingBranch.AssignChild(ref snowSlowLeaf);
        slowingBranch.AssignChild(ref iceSlowLeaf);

        turnStraightBranch.AssignChild(ref powderRideLeaf);
        turnStraightBranch.AssignChild(ref snowRideLeaf);
        turnStraightBranch.AssignChild(ref iceRideLeaf);

        turnCarvingBranch.AssignChild(ref powderTurnLeaf);
        turnCarvingBranch.AssignChild(ref snowTurnLeaf);
        turnCarvingBranch.AssignChild(ref iceTurnLeaf);

        ridingBranch.AssignChild(ref turnStraightBranch);
        ridingBranch.AssignChild(ref turnCarvingBranch);

        groundedBranch.AssignChild(ref ridingBranch);
        groundedBranch.AssignChild(ref slowingBranch);
        groundedBranch.AssignChild(ref stoppedBranch);

        t_audioStateTree.AssignChild(ref groundedBranch);
        t_audioStateTree.AssignChild(ref airborneBranch);
    }
}
