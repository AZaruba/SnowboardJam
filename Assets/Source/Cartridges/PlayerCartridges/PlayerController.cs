using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, iEntityController {

    #region Members
    // Serialized items
    [SerializeField] private PlayerData   c_playerData;
    [SerializeField] private DebugAccessor debugAccessor;

    // private members
    private StateMachine c_StateMachine;

    // cartridge list
    private AccelerationCartridge cart_acceleration;
    private VelocityCartridge cart_velocity;
    private HandlingCartridge cart_handling;
    private AngleCalculationCartridge cart_angleCalc;
    private GravityCartridge cart_gravity;
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
        cart_acceleration = new AccelerationCartridge ();
        cart_handling = new HandlingCartridge();

        StationaryState s_stationary = new StationaryState (ref c_playerData, ref cart_angleCalc);
        AerialState s_aerial = new AerialState (ref c_playerData, ref cart_gravity, ref cart_velocity);
        RidingState s_riding = new RidingState (ref c_playerData, ref cart_angleCalc, ref cart_acceleration, ref cart_velocity);
        SlowingState s_slowing = new SlowingState(ref c_playerData, ref cart_velocity, ref cart_acceleration, ref cart_angleCalc);
        CarvingState s_carving = new CarvingState(ref c_playerData, ref cart_angleCalc, ref cart_acceleration, ref cart_velocity, ref cart_handling);
        SlowingCarvingState s_carvingSlow = new SlowingCarvingState(ref c_playerData, ref cart_angleCalc, ref cart_acceleration, ref cart_velocity, ref cart_handling);


        c_StateMachine = new StateMachine (s_aerial, StateRef.AIRBORNE);
        c_StateMachine.AddState(s_stationary, StateRef.STATIONARY);
        c_StateMachine.AddState(s_riding, StateRef.RIDING);
        c_StateMachine.AddState(s_carving, StateRef.CARVING);
        c_StateMachine.AddState(s_slowing, StateRef.STOPPING);
        c_StateMachine.AddState(s_carvingSlow, StateRef.CARVING_STOPPING);
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

        c_StateMachine.Act();

        EngineUpdate();

        debugAccessor.DisplayState("Current PlayerState: ", c_StateMachine.GetCurrentState());
        debugAccessor.DisplayFloat("Current SlowAxis", c_playerData.f_inputAxisTurn);
        debugAccessor.DisplayVector3("Current Rotated Forward", c_playerData.RotationBuffer * Vector3.forward);
        debugAccessor.DisplayVector3("Current Direction", c_playerData.CurrentDirection, 1);
	}

    /// <summary>
    /// Updates the object's state within the engine.
    /// </summary>
    public void EngineUpdate()
    {
        transform.position = c_playerData.CurrentPosition;
        transform.rotation = c_playerData.RotationBuffer; // transform.Rotate(c_playerData.RotationBuffer.eulerAngles);   
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
        c_playerData.CurrentPosition = transform.position;
        c_playerData.CurrentDirection = transform.forward.normalized;
        c_playerData.CurrentNormal = transform.up.normalized;
        c_playerData.RotationBuffer = transform.rotation;

        RaycastHit hitInfo;
        if (Physics.Raycast(c_playerData.CurrentPosition, c_playerData.CurrentDown, out hitInfo, c_playerData.f_currentRaycastDistance))
        {
            c_playerData.CurrentSurfaceNormal = hitInfo.normal;
            c_playerData.CurrentSurfaceAttachPoint = hitInfo.point;
        }
        else
        {
            c_playerData.CurrentSurfaceNormal = Vector3.zero;
        }
    }

    /// <summary>
    /// Updates the state machine based on whatever input sources are required
    /// </summary>
    public void UpdateStateMachine()
    {
        // current issue, these commands don't work out great
        if (c_playerData.CurrentSurfaceNormal.normalized == Vector3.zero)
        {
            c_StateMachine.Execute(Command.FALL);
        }
        else
        {
            c_StateMachine.Execute(Command.LAND);
        }

        if (Mathf.Abs(c_playerData.f_inputAxisTurn) > 0.0f)
        {
            c_StateMachine.Execute(Command.TURN);
        }
        if (c_playerData.f_inputAxisLVert < 0.0f)
        {
            c_StateMachine.Execute(Command.SLOW);
        }

        if (Mathf.Abs(c_playerData.f_inputAxisTurn) < 0.0f &&
            c_playerData.f_inputAxisLVert >= 0.0f)
        {
            c_StateMachine.Execute(Command.RIDE);
        }
        if (c_playerData.CurrentSpeed <= 0.0f)
        {
            c_StateMachine.Execute(Command.STOP);
        }
    }

    #region StartupFunctions
    /// <summary>
    /// Sets default values for player data that interfaces with the engine, such as the player position
    /// </summary>
    void SetDefaultPlayerData()
    {
        c_playerData.CurrentPosition = transform.position;
        c_playerData.RotationBuffer = transform.rotation;
        c_playerData.CurrentDirection = transform.forward;
        c_playerData.CurrentNormal = transform.up;
        c_playerData.CurrentDown = transform.up * -1;
        c_playerData.CurrentSpeed = 0.0f;
    }
    #endregion
}

/* TODO LIST:
 * 1) Crashing (as in the mechanic, not the failure state!)
 * 2) Improving the surface adherence when moving down
 */ 
