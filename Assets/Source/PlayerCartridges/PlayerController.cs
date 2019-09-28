using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    #region Members
    // Serialized items
    [SerializeField] private PlayerData   c_playerData;

    // private members
    private StateMachine c_stateMachine;
    #endregion

	/// <summary>
    /// Start this instance. Initializes all valid states for this object
    /// then adds them to the state machine
    /// </summary>
	void Start ()
    {
        // Initialize all states
        AcceleratingState s_accelerating = new AcceleratingState();
        CoastingState s_coasting = new CoastingState ();
        StationaryState s_stationary = new StationaryState ();

        c_stateMachine = new StateMachine (s_accelerating);
        c_stateMachine.AddState(s_stationary);
        c_stateMachine.AddState(s_coasting);
	}
	
	/// <summary>
    /// Update this instance. States perform actions on data, the data is then
    /// used for object-level functions (such as translations) and then the
    /// state is updated.
    /// </summary>
	void FixedUpdate ()
    {
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
        else
        {
            c_stateMachine.Execute(Command.COAST);
        }
    }

    #region StartupFunctions
    /// <summary>
    /// Sets default values for player data that interfaces with the engine, such as the player position
    /// </summary>
    void SetDefaultPlayerData()
    {
        c_playerData.SetCurrentPosition(transform.position);
        c_playerData.SetCurrentSpeed(0.0f);
    }
    #endregion
}
