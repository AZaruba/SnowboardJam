using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, iEntityController {

    #region Members
    // Serialized items
    [SerializeField] private PlayerData   c_playerData;

    // private members
    private StateMachine c_stateMachine;

    // cartridge list
    private AccelerationCartridge cart_acceleration;
    private VelocityCartridge cart_velocity;
    private HandlingCartridge cart_handling;
    #endregion

	/// <summary>
    /// Start this instance. Initializes all valid states for this object
    /// then adds them to the state machine
    /// </summary>
	void Start ()
    {
        SetDefaultPlayerData();

        // Initialize all cartridges
        cart_acceleration = new AccelerationCartridge();
        cart_velocity = new VelocityCartridge ();
        cart_handling = new HandlingCartridge ();

        // Initialize all states
        AcceleratingState s_accelerating = new AcceleratingState(ref cart_acceleration, ref cart_velocity);
        AcceleratingTurningState s_acceleratingTurning = new AcceleratingTurningState (ref cart_acceleration, ref cart_velocity, ref cart_handling);
        CoastingState s_coasting = new CoastingState (ref cart_acceleration, ref cart_velocity);
        CoastingTurningState s_coastingTurning = new CoastingTurningState (ref cart_acceleration, ref cart_velocity, ref cart_handling);
        StationaryState s_stationary = new StationaryState ();

        c_stateMachine = new StateMachine (s_accelerating, StateRef.ACCELERATING);
        c_stateMachine.AddState(s_stationary, StateRef.STATIONARY);
        c_stateMachine.AddState(s_coasting, StateRef.COASTING);
        c_stateMachine.AddState(s_coastingTurning, StateRef.COASTING_TURNING);
        c_stateMachine.AddState(s_acceleratingTurning, StateRef.ACCELERATING_TURNING);
	}
	
	/// <summary>
    /// Update this instance. States perform actions on data, the data is then
    /// used for object-level functions (such as translations) and then the
    /// state is updated.
    /// </summary>
	void FixedUpdate ()
    {
        EnginePull();

        c_stateMachine.Act(ref c_playerData);

        EngineUpdate();

        UpdateStateMachine();
	}

    /// <summary>
    /// Updates the object's state within the engine.
    /// </summary>
    void EngineUpdate()
    {
        transform.position = c_playerData.GetCurrentPosition();
        transform.forward = c_playerData.GetCurrentDirection();
    }

    /// <summary>
    /// Pulls information from the engine into the controller/data structure
    /// </summary>
    void EnginePull()
    {
        c_playerData.SetInputAxisTurn(Input.GetAxis("Horizontal"));
    }

    /// <summary>
    /// Updates the state machine based on whatever input sources are required
    /// </summary>
    private void UpdateStateMachine()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            c_stateMachine.Execute(Command.ACCELERATE);
        } 
        else if (c_playerData.GetCurrentSpeed() <= 0.0f)
        {
            c_stateMachine.Execute(Command.STOP);
        }
        else // Coasting, stopping, and accelerating are all mutually exclusive
        {
            c_stateMachine.Execute(Command.COAST);
        }

        if (Mathf.Abs(c_playerData.GetInputAxisTurn()) > 0.0f) // TODO: implement dead zone
        {
            c_stateMachine.Execute(Command.TURN);
        }
        else
        {
            c_stateMachine.Execute(Command.TURN_END);
        }
    }

    #region StartupFunctions
    /// <summary>
    /// Sets default values for player data that interfaces with the engine, such as the player position
    /// </summary>
    void SetDefaultPlayerData()
    {
        c_playerData.SetCurrentPosition(transform.position);
        c_playerData.SetCurrentDirection(transform.forward);
        c_playerData.SetCurrentSpeed(0.0f);
    }
    #endregion
}
