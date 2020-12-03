﻿using System.Collections;
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

    private StateData c_stateData;
    private AerialMoveData c_aerialMoveData;
    private TrickPhysicsData c_trickPhysicsData;
    private PlayerInputData c_inputData;
    private ScoringData c_scoringData;
    private CollisionData c_collisionData;

    // private members
    private StateMachine c_turnMachine;
    private StateMachine c_airMachine;
    private StateMachine c_accelMachine;
    private StateMachine sm_tricking;
    private StateMachine sm_trickPhys;

    private PlayerPositionData c_positionData;
    private EntityData c_entityData;

    private AudioController c_audioController;
    private AudioDTree t_audioStateTree;

    // cartridge list
    private AccelerationCartridge cart_f_acceleration;
    private VelocityCartridge cart_velocity;
    private HandlingCartridge cart_handling;
    private AngleCalculationCartridge cart_angleCalc;
    private GravityCartridge cart_gravity;
    private IncrementCartridge cart_incr;
    private SurfaceInfluenceCartridge cart_surfInf;
    private QuaternionCartridge cart_quatern;

    // Cached Calculation items
    RaycastHit frontHit;
    RaycastHit backHit;
    RaycastHit centerHit;
    RaycastHit forwardHit;
    RaycastHit obstacleHit;

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
        cart_quatern = new QuaternionCartridge();

        InitializeStateMachines();
        InitializeAudioController();
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
        Vector3 startPosition = c_playerData.v_currentPosition;

        UpdateStateMachine();

        c_airMachine.Act();
        c_accelMachine.Act();
        c_turnMachine.Act();
        sm_tricking.Act();
        sm_trickPhys.Act();

        LateEnginePull(startPosition);
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
        debugAccessor.DisplayVector3("Current Dir", c_playerData.v_currentDirection);

        UpdateAnimator();
        UpdateAudio();
    }

    /// <summary>
    /// Pulls data that may be influenced by changes during the current frame.
    /// 
    /// Currently: Collision detection.
    /// </summary>
    public void LateEnginePull(Vector3 oldPosition)
    {

        float speedRatio = c_playerData.f_currentSpeed;

        c_collisionData.f_obstacleAngle = 20f + CollisionData.BaseObstacleCollisionAngle * (speedRatio / c_playerData.f_topSpeed); // TODO: add "upward motion" weight
        c_collisionData.f_frontRayLengthUp = Mathf.Tan(c_collisionData.f_obstacleAngle*(Mathf.PI/180.0f)) * Time.deltaTime;
        c_collisionData.f_frontRayLengthDown = (Mathf.Tan(c_collisionData.f_obstacleAngle * (Mathf.PI / 180.0f)) * Time.deltaTime + c_aerialMoveData.f_verticalVelocity * -1) * Time.deltaTime;

        c_collisionData.f_obstacleRayLength = speedRatio * Time.deltaTime; // the expected travel amount next frame
        CheckForWall();
        CheckForGround2();
    }

    /// <summary>
    /// Updates the animator's states using information from the player data
    /// </summary>
    private void UpdateAnimator()
    {
        PlayerAnimator.SetFloat("TurnAnalogue", c_inputData.f_inputAxisLHoriz);
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
        if (c_stateData.b_courseFinished == true)
        {
            if (c_collisionData.b_collisionDetected)
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
        if (!c_collisionData.b_collisionDetected)
        {
            c_accelMachine.Execute(Command.FALL);
            c_turnMachine.Execute(Command.FALL);
            c_airMachine.Execute(Command.FALL);
            sm_tricking.Execute(Command.READY_TRICK);
        }
        else
        {
            // trick should only send when we land a trick
            if (c_scoringData.b_sendTrick)
            {
                CompileAndSendScore();
            }

            c_accelMachine.Execute(Command.LAND);
            c_turnMachine.Execute(Command.LAND);
            c_airMachine.Execute(Command.LAND);
            sm_tricking.Execute(Command.LAND);
            sm_trickPhys.Execute(Command.LAND);

        }

        if (Mathf.Abs(c_inputData.f_inputAxisLHoriz) > 0.0f)
        {
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

        if (GlobalInputController.GetInputAction(ControlAction.JUMP) == KeyValue.PRESSED)
        {
            c_accelMachine.Execute(Command.CHARGE);
            c_airMachine.Execute(Command.CHARGE);
            c_turnMachine.Execute(Command.CHARGE);
            sm_trickPhys.Execute(Command.CHARGE);
        }
        else if (GlobalInputController.GetInputAction(ControlAction.JUMP) == KeyValue.UP)
        {
            c_airMachine.Execute(Command.JUMP);
            c_accelMachine.Execute(Command.JUMP);
            c_turnMachine.Execute(Command.JUMP);
            sm_trickPhys.Execute(Command.JUMP);
            sm_tricking.Execute(Command.READY_TRICK);
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
    }

    #region StartupFunctions
    /// <summary>
    /// Sets default values for player data that interfaces with the engine, such as the player position
    /// </summary>
    void SetDefaultPlayerData()
    {
        c_trickPhysicsData = new TrickPhysicsData(Attributes.Tricks, Attributes.MaxStats);
        c_positionData = new PlayerPositionData(transform.position, transform.forward);
        c_scoringData = new ScoringData();
        c_inputData = new PlayerInputData();
        c_stateData = new StateData();
        c_aerialMoveData = new AerialMoveData();
        c_entityData = new EntityData();
        c_collisionData = new CollisionData(CollisionData.FrontRayOffset, CollisionData.BackRayOffset);

        c_playerData.v_currentPosition = transform.position;
        c_playerData.q_currentRotation = transform.rotation;
        c_playerData.q_targetRotation = transform.rotation;
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

        c_stateData.b_updateState = true;
        c_stateData.b_courseFinished = false;
    }
    #endregion

    private void OnDrawGizmos()
    {
        Vector3 offsetFront = c_playerData.v_currentPosition + c_playerData.q_currentRotation.normalized * CollisionData.FrontRayOffset;
        Vector3 offsetBack = c_playerData.v_currentPosition + c_playerData.q_currentRotation.normalized * CollisionData.BackRayOffset;

        Gizmos.DrawRay(offsetBack, offsetFront - offsetBack);
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

        c_playerData.q_currentRotation = Quaternion.LookRotation(newDir, c_playerData.v_currentNormal);
        c_playerData.v_currentDirection = newDir;

        newDir.y = 0;
        c_aerialMoveData.v_lateralDirection = newDir;

        // TODO: velocity adjustment on collision
    }

    private void CheckForWall()
    {
        LayerMask lm_env = LayerMask.GetMask("Environment");

        /* The ground check goes up f_frontRayLengthUp.
         * We should push the origin UP by q_currentRotation * f_FrontRayLengthUp
         * Then lower the vertical extents by half, effectively keeping the TOP
         * consistent but only pushing the bottom up
         */ 

        Vector3 playerBox = CollisionData.BodyHalfExtents;

        playerBox.y -= c_collisionData.f_frontRayLengthUp / 2;
        Vector3 offsetOrigin = c_playerData.v_currentPosition + c_playerData.q_currentRotation * new Vector3(0, c_collisionData.f_obstacleRayLength + playerBox.y, 0);
        Debug.DrawLine(offsetOrigin, offsetOrigin + c_playerData.v_currentDirection, Color.red);

        if (Physics.BoxCast(offsetOrigin,
                            playerBox,
                            c_playerData.v_currentDirection,
                            out obstacleHit,
                            c_playerData.q_currentRotation,
                            c_collisionData.f_obstacleRayLength,
                            lm_env))
        {
            HandleWallCollision(obstacleHit);
        }
    }

    private void CheckForGround2()
    {

        float offsetDist = CollisionData.CenterOffset.magnitude;
        Vector3 upwardVector = new Vector3(0, c_collisionData.f_frontRayLengthUp, 0);

        // the vector giving the raycast distance AND direction
        LayerMask lm_env = LayerMask.GetMask("Environment");

        c_collisionData.b_collisionDetected = false;

        c_collisionData.v_previousPosition = c_playerData.v_currentPosition;

        // safety detection: use old-style raycast to check if we want to stay grounded
        if (Physics.BoxCast(c_playerData.v_currentPosition + (c_playerData.q_currentRotation * CollisionData.CenterOffset),
                            CollisionData.HalfExtents,
                            c_playerData.v_currentDown,
                            out centerHit,
                            c_playerData.q_currentRotation,
                            (c_aerialMoveData.f_verticalVelocity * Time.deltaTime * -1) + offsetDist,
                            lm_env))
        {
            Debug.DrawLine(c_playerData.v_currentPosition + (c_playerData.q_currentRotation * CollisionData.CenterOffset), centerHit.point, Color.blue, 5f);
            c_collisionData.b_collisionDetected = true;
            c_collisionData.v_surfaceNormal = centerHit.normal;
        }
        else
        {
            c_collisionData.b_collisionDetected = false;
        }

        if (c_collisionData.b_collisionDetected)
        {
            c_collisionData.v_frontOffset = c_playerData.v_currentPosition + c_playerData.q_currentRotation.normalized * (CollisionData.FrontRayOffset + upwardVector);
            c_collisionData.v_backOffset = c_playerData.v_currentPosition + c_playerData.q_currentRotation.normalized * (CollisionData.BackRayOffset + upwardVector);

            if (Physics.Raycast(c_playerData.v_currentPosition + (c_playerData.q_currentRotation * CollisionData.CenterOffset),
                                c_playerData.v_currentDown,
                                out centerHit,
                                (c_aerialMoveData.f_verticalVelocity * Time.deltaTime * -1) + offsetDist,
                                lm_env))
            {
                c_collisionData.v_attachPoint = centerHit.point;
            }
            else
            {
                c_collisionData.v_attachPoint = c_playerData.v_currentPosition;
            }

            if (Physics.Raycast(c_collisionData.v_frontOffset, c_playerData.v_currentDown, out frontHit, c_collisionData.f_frontRayLengthUp + c_collisionData.f_frontRayLengthDown, lm_env)) // double to check up and DOWN
            {
                // validate angle 
                if (Vector3.Angle(c_playerData.v_currentNormal, frontHit.normal) > c_collisionData.f_obstacleAngle)
                {
                    c_collisionData.v_frontNormal = Vector3.zero;
                    c_collisionData.v_frontPoint = c_collisionData.v_frontOffset;
                }
                else
                {
                    c_collisionData.v_frontNormal = GetBaryCentricNormal(frontHit);
                    c_collisionData.v_frontPoint = frontHit.point;
                }
            }
            else
            {
                c_collisionData.v_frontNormal = Vector3.zero;
                c_collisionData.v_frontPoint = c_collisionData.v_frontOffset;
            }

            if (Physics.Raycast(c_collisionData.v_backOffset, c_playerData.v_currentDown, out backHit, c_collisionData.f_frontRayLengthUp + c_collisionData.f_frontRayLengthDown, lm_env))
            {
                if (Vector3.Angle(c_playerData.v_currentNormal, backHit.normal) > c_collisionData.f_obstacleAngle)
                {
                    c_collisionData.v_backNormal = Vector3.zero;
                    c_collisionData.v_backPoint = c_collisionData.v_backOffset;
                }
                else
                {
                    c_collisionData.v_backNormal = GetBaryCentricNormal(backHit);
                    c_collisionData.v_backPoint = backHit.point;
                }
            }
            else
            {
                c_collisionData.v_backNormal = Vector3.zero;
                c_collisionData.v_backPoint = c_collisionData.v_backOffset;
            }

            if ((c_collisionData.v_backNormal + c_collisionData.v_frontNormal) != Vector3.zero)
            {
                c_collisionData.v_surfaceNormal = (c_collisionData.v_backNormal + c_collisionData.v_frontNormal).normalized;
            }
        }

        c_collisionData.v_frontOffset = c_playerData.v_currentPosition + c_playerData.q_currentRotation.normalized * (CollisionData.FrontRayOffset);
        c_collisionData.v_backOffset = c_playerData.v_currentPosition + c_playerData.q_currentRotation.normalized * (CollisionData.BackRayOffset);

    }

    private Vector3 GetBaryCentricNormal(RaycastHit hitIn)
    {
        Vector3 BarycentricNormal = Vector3.zero;
        Vector3 BarycentricCoords = hitIn.barycentricCoordinate;

        MeshCollider meshCol = hitIn.collider as MeshCollider;
        if (meshCol == null || meshCol.sharedMesh == null)
        {
            return BarycentricNormal;
        }

        Mesh mesh = (hitIn.collider as MeshCollider).sharedMesh;
        Vector3[] normals = mesh.normals;
        int[] triangles = mesh.triangles;

        Vector3 n0 = normals[triangles[hitIn.triangleIndex * 3 + 0]];
        Vector3 n1 = normals[triangles[hitIn.triangleIndex * 3 + 1]];
        Vector3 n2 = normals[triangles[hitIn.triangleIndex * 3 + 2]];

        BarycentricNormal = n0 * BarycentricCoords.x +
                            n1 * BarycentricCoords.y +
                            n2 * BarycentricCoords.z;

        BarycentricNormal = BarycentricNormal.normalized;

        // Transform local space normals to world space
        Transform hitTransform = hitIn.collider.transform;
        BarycentricNormal = hitTransform.TransformDirection(BarycentricNormal);

        return BarycentricNormal.normalized;
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
        InitializeAccelMachine();
        InitializeAirMachine();
        InitializeTurnMachine();
        InitializeTrickMachine();
        InitializeTrickPhysicsMachine();
    }

    private void InitializeAccelMachine()
    {
        MoveAerialState s_moveAerial = new MoveAerialState();
        StationaryState s_stationary = new StationaryState(ref c_playerData, ref c_collisionData, ref cart_angleCalc, ref cart_velocity);
        RidingState s_riding = new RidingState(ref c_playerData, ref c_positionData, ref c_collisionData, ref cart_angleCalc, ref cart_f_acceleration, ref cart_velocity, ref cart_surfInf);
        RidingChargeState s_ridingCharge = new RidingChargeState(ref c_playerData, ref c_positionData, ref c_collisionData, ref cart_angleCalc, ref cart_f_acceleration, ref cart_velocity, ref cart_surfInf);
        SlowingState s_slowing = new SlowingState(ref c_playerData, ref c_collisionData, ref c_inputData, ref c_positionData, ref cart_velocity, ref cart_f_acceleration, ref cart_angleCalc, ref cart_surfInf);
        CrashedState s_crashed = new CrashedState(ref c_playerData, ref cart_incr);

        c_accelMachine = new StateMachine(s_stationary, StateRef.STATIONARY);
        c_accelMachine.AddState(s_riding, StateRef.RIDING);
        c_accelMachine.AddState(s_slowing, StateRef.STOPPING);
        c_accelMachine.AddState(s_moveAerial, StateRef.AIRBORNE);
        c_accelMachine.AddState(s_crashed, StateRef.CRASHED);
        c_accelMachine.AddState(s_ridingCharge, StateRef.CHARGING);


    }

    private void InitializeAirMachine()
    {
        AerialState s_aerial = new AerialState(ref c_playerData, ref c_collisionData, ref c_aerialMoveData, ref cart_gravity, ref cart_velocity);
        GroundedState s_grounded = new GroundedState(ref c_playerData, ref c_aerialMoveData, ref c_collisionData, ref c_positionData, ref cart_velocity, ref cart_angleCalc, ref cart_surfInf);
        JumpChargeState s_jumpCharge = new JumpChargeState(ref c_playerData, ref c_collisionData, ref cart_incr);
        AirDisabledState s_airDisabled = new AirDisabledState();

        c_airMachine = new StateMachine(s_grounded, StateRef.GROUNDED);
        c_airMachine.AddState(s_aerial, StateRef.AIRBORNE);
        c_airMachine.AddState(s_jumpCharge, StateRef.CHARGING);
        c_airMachine.AddState(s_airDisabled, StateRef.DISABLED);
    }

    private void InitializeTurnMachine()
    {
        StraightState s_straight = new StraightState(ref c_playerData, ref c_positionData, ref cart_surfInf);
        CarvingState s_carving = new CarvingState(ref c_playerData, ref c_collisionData, ref c_inputData, ref c_positionData, ref cart_handling, ref cart_surfInf);
        TurnDisabledState s_turnDisabled = new TurnDisabledState();
        TurnChargeState s_turnCharge = new TurnChargeState(ref c_playerData, ref c_positionData, ref cart_surfInf);

        c_turnMachine = new StateMachine(s_straight, StateRef.RIDING);
        c_turnMachine.AddState(s_carving, StateRef.CARVING);
        c_turnMachine.AddState(s_turnCharge, StateRef.CHARGING);
        c_turnMachine.AddState(s_turnDisabled, StateRef.DISABLED);

    }

    private void InitializeTrickPhysicsMachine()
    {
        SpinIdleState s_spinIdle = new SpinIdleState(ref c_trickPhysicsData, ref c_scoringData);
        SpinChargeState s_spinCharge = new SpinChargeState(ref c_trickPhysicsData, ref c_inputData, ref cart_incr);
        SpinningState s_spinning = new SpinningState(ref c_trickPhysicsData, ref c_positionData, ref cart_handling, ref cart_incr, ref c_scoringData);
        SpinSnapState s_spinSnap = new SpinSnapState(ref c_aerialMoveData, ref c_positionData, ref c_trickPhysicsData, ref cart_handling, ref c_scoringData);

        sm_trickPhys = new StateMachine(s_spinIdle, StateRef.SPIN_IDLE);
        sm_trickPhys.AddState(s_spinCharge, StateRef.SPIN_CHARGE);
        sm_trickPhys.AddState(s_spinning, StateRef.SPINNING);
        sm_trickPhys.AddState(s_spinSnap, StateRef.SPIN_RESET);
    }

    private void InitializeTrickMachine()
    {
        TrickDisabledState s_trickDisabled = new TrickDisabledState(ref trickData, ref c_scoringData);
        TrickReadyState s_trickReady = new TrickReadyState();
        TrickTransitionState s_trickTransition = new TrickTransitionState(ref trickData, ref c_scoringData);
        TrickingState s_tricking = new TrickingState(ref trickData, ref c_scoringData, ref cart_incr);

        sm_tricking = new StateMachine(s_trickDisabled, StateRef.TRICK_DISABLED);
        sm_tricking.AddState(s_trickReady, StateRef.TRICK_READY);
        sm_tricking.AddState(s_tricking, StateRef.TRICKING);
        sm_tricking.AddState(s_trickTransition, StateRef.TRICK_TRANSITION);
    }

    private void InitializeMessageClient()
    {
        cl_character = new CharacterMessageClient(ref c_stateData, ref c_entityData, ref c_audioController);
        MessageServer.Subscribe(ref cl_character, MessageID.PAUSE);
        MessageServer.Subscribe(ref cl_character, MessageID.PLAYER_FINISHED);
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
        c_scoringData.l_timeList = new List<float>();
        c_scoringData.l_trickList = new List<TrickName>();
        c_scoringData.b_sendTrick = false;
    }
    #endregion

    private void UpdateTrickStateMachine()
    {
        bool TrickHit = false;
        if (!TrickHit && GlobalInputController.GetInputAction(ControlAction.LEFT_GRAB) == KeyValue.PRESSED)
        {
            TrickHit = true;
            trickData.k_activeTrickAction = ControlAction.LEFT_GRAB;
            trickData.t_activeTrickName = trickData.trick_left;
        }
        if (!TrickHit && GlobalInputController.GetInputAction(ControlAction.UP_GRAB) == KeyValue.PRESSED)
        {
            TrickHit = true;
            trickData.k_activeTrickAction = ControlAction.UP_GRAB;
            trickData.t_activeTrickName = trickData.trick_up;
        }

        if (!TrickHit && GlobalInputController.GetInputAction(ControlAction.RIGHT_GRAB) == KeyValue.PRESSED)
        {
            TrickHit = true;
            trickData.k_activeTrickAction = ControlAction.RIGHT_GRAB;
            trickData.t_activeTrickName = trickData.trick_right;
        }

        if (!TrickHit && GlobalInputController.GetInputAction(ControlAction.DOWN_GRAB) == KeyValue.PRESSED)
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
        else if (GlobalInputController.GetInputAction(trickData.k_activeTrickAction) == KeyValue.UP)
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