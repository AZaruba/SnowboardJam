using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, iEntityController {

    #region Members
    // Serialized items
    [SerializeField] private PlayerData   c_playerData;
    [SerializeField] private CharacterController c_characterController;

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
        cart_velocity = new VelocityCartridge ();
        cart_acceleration = new AccelerationCartridge ();

        StationaryState s_stationary = new StationaryState (ref cart_angleCalc);
        AerialState s_aerial = new AerialState (ref cart_gravity);
        RidingState s_riding = new RidingState (ref cart_angleCalc, ref cart_acceleration, ref cart_velocity);

        c_stateMachine = new PlayerStateMachine (s_aerial, StateRef.AIRBORNE);
        c_stateMachine.AddState(s_stationary, StateRef.STATIONARY);
        c_stateMachine.AddState(s_riding, StateRef.RIDING);
	}
	
	/// <summary>
    /// Update this instance. States perform actions on data, the data is then
    /// used for object-level functions (such as translations) and then the
    /// state is updated.
    /// </summary>
	void FixedUpdate ()
    {
        EngineUpdate();

        c_stateMachine.Act(ref c_playerData);

        EnginePull();

        UpdateStateMachine();
	}

    /// <summary>
    /// Updates the object's state within the engine.
    /// </summary>
    public void EngineUpdate()
    {
        c_characterController.Move(c_playerData.CurrentPosition - c_characterController.transform.position);
        c_characterController.transform.Rotate(c_playerData.RotationBuffer.eulerAngles);
    }

    /// <summary>
    /// Pulls information from the engine into the controller/data structure
    /// </summary>
    public void EnginePull()
    {
        c_playerData.InputAxisTurn = Input.GetAxis("Horizontal");

        // raycast to check for change in ground
        RaycastHit hitInfo;
        if (Physics.Raycast(c_characterController.transform.position, Vector3.down, out hitInfo, c_playerData.f_raycastDistance))
        {
            Vector3 oldSurfaceNormal = c_playerData.CurrentSurfaceNormal;

            c_playerData.CurrentSurfaceNormal = hitInfo.normal;
            c_playerData.CurrentSurfaceAttachPoint = hitInfo.point;
            if (c_playerData.CurrentSurfaceNormal.normalized !=
                oldSurfaceNormal)
            {
                c_stateMachine.Execute(Command.RIDE, ref c_playerData, true);
            }
        }
    }

    /// <summary>
    /// Updates the state machine based on whatever input sources are required
    /// </summary>
    public void UpdateStateMachine()
    {
        c_stateMachine.Execute(Command.RIDE, ref c_playerData);
        if (c_characterController.isGrounded)
        {
            c_stateMachine.Execute(Command.LAND, ref c_playerData);
        }
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
    #endregion

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        /* Known issues here:
         * 
         * 1) How can we differentiate between different types of environment objects? - ANSWER: layers?
         * 2) How can we handle the forces of gravity (i.e. don't make the player go back up a hill
         * 3) If we can cycle back to the same state, how do we ensure a surface change occurs
         */ 

        if (c_stateMachine.GetCurrentState() == StateRef.AIRBORNE)
        {
            c_playerData.CurrentSurfaceAttachPoint = hit.point;
        }
    }
}
