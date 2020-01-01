using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, iEntityController {

    #region Members
    // Serialized items
    [SerializeField] private PlayerData   c_playerData;
    [SerializeField] private DebugAccessor debugAccessor;

    // private members
    private StateMachine c_turnMachine;
    private StateMachine c_airMachine;
    private StateMachine c_accelMachine;

    // cartridge list
    private AccelerationCartridge cart_f_acceleration;
    private VelocityCartridge cart_velocity;
    private HandlingCartridge cart_handling;
    private AngleCalculationCartridge cart_angleCalc;
    private GravityCartridge cart_gravity;
    private IncrementCartridge cart_incr;
    #endregion

	/// <summary>
    /// Start this instance. Initializes all valid states for this object
    /// then adds them to the state machine
    /// </summary>
	void Start ()
    {
        SetDefaultPlayerData();
        cart_gravity = new GravityCartridge ();
        cart_angleCalc = new AngleCalculationCartridge ();
        cart_velocity = new VelocityCartridge ();
        cart_f_acceleration = new AccelerationCartridge ();
        cart_handling = new HandlingCartridge();
        cart_incr = new IncrementCartridge();

        MoveAerialState s_moveAerial = new MoveAerialState();
        StationaryState s_stationary = new StationaryState (ref c_playerData, ref cart_angleCalc, ref cart_velocity);
        RidingState s_riding = new RidingState (ref c_playerData, ref cart_angleCalc, ref cart_f_acceleration, ref cart_velocity);
        SlowingState s_slowing = new SlowingState(ref c_playerData, ref cart_velocity, ref cart_f_acceleration, ref cart_angleCalc);

        StraightState s_straight = new StraightState();
        CarvingState s_carving = new CarvingState(ref c_playerData, ref cart_handling);
        TurnDisabledState s_turnDisabled = new TurnDisabledState();

        AerialState s_aerial = new AerialState(ref c_playerData, ref cart_gravity, ref cart_velocity);
        JumpingState s_jumping = new JumpingState(ref c_playerData, ref cart_gravity, ref cart_velocity);
        GroundedState s_grounded = new GroundedState();
        JumpChargeState s_jumpCharge = new JumpChargeState(ref c_playerData, ref cart_incr);

        c_accelMachine = new StateMachine(s_stationary, StateRef.STATIONARY);
        c_accelMachine.AddState(s_riding, StateRef.RIDING);
        c_accelMachine.AddState(s_slowing, StateRef.STOPPING);
        c_accelMachine.AddState(s_moveAerial, StateRef.AIRBORNE);

        c_turnMachine = new StateMachine(s_straight, StateRef.RIDING);
        c_turnMachine.AddState(s_carving, StateRef.CARVING);
        c_turnMachine.AddState(s_turnDisabled, StateRef.DISABLED);

        c_airMachine = new StateMachine(s_grounded, StateRef.GROUNDED);
        c_airMachine.AddState(s_aerial, StateRef.AIRBORNE);
        c_airMachine.AddState(s_jumping, StateRef.JUMPING);
        c_airMachine.AddState(s_jumpCharge, StateRef.CHARGING);
	}
	
	/// <summary>
    /// Update this instance. States perform actions on data, the data is then
    /// used for object-level functions (such as translations) and then the
    /// state is updated.
    /// </summary>
	void Update ()
    {
        EnginePull();

        UpdateStateMachine();

        c_accelMachine.Act();
        c_turnMachine.Act();
        c_airMachine.Act();

        EngineUpdate();

        debugAccessor.DisplayState("Current AirState: ", c_airMachine.GetCurrentState());
        debugAccessor.DisplayFloat("Current AirVelocity", c_playerData.f_currentAirVelocity);
        debugAccessor.DisplayVector3("Current Down", c_playerData.v_currentDown);
        debugAccessor.DisplayVector3("Current Position", c_playerData.v_currentPosition, 1);
	}

    /// <summary>
    /// Updates the object's state within the engine.
    /// </summary>
    public void EngineUpdate()
    {
        transform.position = c_playerData.v_currentPosition;
        transform.rotation = c_playerData.q_currentRotation; // transform.Rotate(c_playerData.q_currentRotation.eulerAngles);   
    }

    /// <summary>
    /// Pulls information from the engine into the controller/data structure
    /// </summary>
    public void EnginePull()
    {
        c_playerData.f_inputAxisTurn = Input.GetAxis("Horizontal");
        c_playerData.f_inputAxisLVert = Input.GetAxis("Vertical");

        // TODO: ensure that we can pull the direction and the normal from the object
        // OTHERWISE it implies that there is a desync between data and the engine
        c_playerData.v_currentPosition = transform.position;
        c_playerData.v_currentDirection = transform.forward.normalized;
        c_playerData.v_currentNormal = transform.up.normalized;
        c_playerData.q_currentRotation = transform.rotation;

        RaycastHit hitInfo;
        if (Physics.Raycast(c_playerData.v_currentPosition, c_playerData.v_currentDown, out hitInfo, c_playerData.f_currentRaycastDistance))
        {
            c_playerData.v_currentSurfaceNormal = hitInfo.normal;
            c_playerData.v_currentSurfaceAttachPoint = hitInfo.point;
        }
        else
        {
            c_playerData.v_currentSurfaceNormal = Vector3.zero;
        }
    }

    /// <summary>
    /// Updates the state machine based on whatever input sources are required
    /// </summary>
    public void UpdateStateMachine()
    {
        // current issue, these commands don't work out great
        if (c_playerData.v_currentSurfaceNormal.normalized == Vector3.zero)
        {
            c_accelMachine.Execute(Command.FALL);
            c_turnMachine.Execute(Command.FALL);
            c_airMachine.Execute(Command.FALL);
        }
        else
        {
            c_accelMachine.Execute(Command.LAND);
            c_turnMachine.Execute(Command.LAND);
            c_airMachine.Execute(Command.LAND);
        }

        if (Mathf.Abs(c_playerData.f_inputAxisTurn) > 0.0f)
        {
            c_turnMachine.Execute(Command.TURN);
        }
        else
        {
            c_turnMachine.Execute(Command.RIDE);
        }

        if (c_playerData.f_inputAxisLVert < 0.0f)
        {
            c_accelMachine.Execute(Command.SLOW);
        }
        else
        {
            c_accelMachine.Execute(Command.RIDE);
        }

        if (c_playerData.f_currentSpeed <= 0.0f)
        {
            c_accelMachine.Execute(Command.STOP);
        }

        // TODO: integrate this keypress into the player data
        if (Input.GetKey(KeyCode.Space))
        {
            c_airMachine.Execute(Command.CHARGE);
        }
        else
        {
            c_airMachine.Execute(Command.JUMP);
        }
    }

    #region StartupFunctions
    /// <summary>
    /// Sets default values for player data that interfaces with the engine, such as the player position
    /// </summary>
    void SetDefaultPlayerData()
    {
        c_playerData.v_currentPosition = transform.position;
        c_playerData.q_currentRotation = transform.rotation;
        c_playerData.v_currentDirection = transform.forward;
        c_playerData.v_currentNormal = transform.up;
        c_playerData.v_currentDown = transform.up * -1;
        c_playerData.f_currentSpeed = Constants.ZERO_F;
        c_playerData.f_currentJumpCharge = Constants.ZERO_F;
    }
    #endregion
}

/* TODO LIST:
 * 1) Crashing (as in the mechanic, not the failure state!)
 * 2) Improving the surface adherence when moving down
 */ 
