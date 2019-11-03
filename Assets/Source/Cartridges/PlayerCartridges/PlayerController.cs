using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, iEntityController {

    #region Members
    // Serialized items
    [SerializeField] private PlayerData   c_playerData;
    [SerializeField] private CharacterController c_characterController;
    [SerializeField] private DebugAccessor debugAccessor;

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
        AerialState s_aerial = new AerialState (ref cart_gravity, ref cart_velocity);
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

        EnginePull();

        UpdateStateMachine();

        c_stateMachine.Act(ref c_playerData);

        EngineUpdate();

        debugAccessor.DisplayFloat("Aerial Velocity", c_playerData.CurrentAirVelocity);
        debugAccessor.DisplayState("Player State", c_stateMachine.GetCurrentState());
        debugAccessor.DisplayVector3("Normal", c_playerData.CurrentNormal * -1);
	}

    /// <summary>
    /// Updates the object's state within the engine.
    /// </summary>
    public void EngineUpdate()
    {
        Vector3 frameTranslation = c_playerData.CurrentPosition - c_characterController.transform.position;
        c_characterController.Move(frameTranslation);
        c_characterController.transform.Rotate(c_playerData.RotationBuffer.eulerAngles);

        c_playerData.v_currentOffset += frameTranslation;
        Vector3 offsetPivot = c_playerData.v_currentOffset - c_characterController.transform.position;
        offsetPivot = c_playerData.RotationBuffer * offsetPivot;
        c_playerData.v_currentOffset = offsetPivot + c_characterController.transform.position;
        
    }

    /// <summary>
    /// Pulls information from the engine into the controller/data structure
    /// </summary>
    public void EnginePull()
    {
        c_playerData.InputAxisTurn = Input.GetAxis("Horizontal");
        c_playerData.CurrentPosition = c_characterController.transform.position;

        RaycastHit hitInfo;
        Vector3 downwardVector = c_playerData.CurrentNormal * -1;
        if (Physics.Raycast(c_playerData.v_currentOffset, downwardVector, out hitInfo, c_playerData.f_raycastDistance))
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
        if (c_playerData.CurrentSurfaceNormal.normalized == Vector3.zero)
        {
            c_stateMachine.Execute(Command.FALL, ref c_playerData);
        }
        else
        {
            c_stateMachine.Execute(Command.LAND, ref c_playerData, true);
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
        c_playerData.v_currentOffset = transform.position + c_playerData.v_surfaceRayOffset;
        c_playerData.CurrentSpeed = 0.0f;
    }
    #endregion
}

        /* Known issues:
         * 
         * 1) If the angle is steep enough, the player will "wobble" on the corner
         * 2) The character will get slowed down if careening down fast enough
         *       - If the max speed is faster than the terminal velocity, we're good!
         *       - We may want to be able to ACCELERATE downardd
         * 3) The player still "floats" above the ground, how to solve it?
         * 4) Add time/frame scaling to make serialized values easier to manage
         */ 
