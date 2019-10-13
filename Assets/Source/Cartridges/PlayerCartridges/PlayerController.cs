using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, iEntityController {

    #region Members
    // Serialized items
    [SerializeField] private PlayerData   c_playerData;
    [SerializeField] private Rigidbody r_rigidBody;

    // private members
    private PlayerStateMachine c_stateMachine;

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

        StationaryState s_stationary = new StationaryState (ref cart_angleCalc);
        AerialState s_aerial = new AerialState (ref cart_gravity);

        c_stateMachine = new PlayerStateMachine (s_aerial, StateRef.AIRBORNE);
        c_stateMachine.AddState(s_stationary, StateRef.STATIONARY);
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
    public void EngineUpdate()
    {
        transform.SetPositionAndRotation(c_playerData.CurrentPosition, Quaternion.LookRotation(c_playerData.CurrentDirection, c_playerData.CurrentNormal));
    }

    /// <summary>
    /// Pulls information from the engine into the controller/data structure
    /// </summary>
    public void EnginePull()
    {
        c_playerData.InputAxisTurn = Input.GetAxis("Horizontal");

        /* I don't want materials-based physics, setting velocities to zero allows the
         * Rigidbody to prevent clipping while not causing any unwanted forces.
         */
        r_rigidBody.velocity = Vector3.zero;
        r_rigidBody.angularVelocity = Vector3.zero;
    }

    /// <summary>
    /// Updates the state machine based on whatever input sources are required
    /// </summary>
    public void UpdateStateMachine()
    {
        
    }

    #region StartupFunctions
    /// <summary>
    /// Sets default values for player data that interfaces with the engine, such as the player position
    /// </summary>
    void SetDefaultPlayerData()
    {
        c_playerData.CurrentPosition = transform.position;
        c_playerData.CurrentDirection = transform.forward;
        c_playerData.CurrentNormal = transform.up;
        c_playerData.CurrentSpeed = 0.0f;
    }

    void OnCollisionEnter(Collision collision)
    {
        RaycastHit rch_rayHit;
        if (Physics.Raycast(transform.position, Vector3.down, out rch_rayHit))
        {
            c_playerData.CurrentSurfaceNormal = rch_rayHit.normal;
        }

        c_stateMachine.Execute(Command.LAND);
    }
    #endregion
}
