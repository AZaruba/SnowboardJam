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
	void Start () {
        // Initialize all states
        AcceleratingState s_accelerating = new AcceleratingState(c_playerData, new AccelerationCartridge());

        c_stateMachine = new StateMachine (s_accelerating);
	}
	
	/// <summary>
    /// Update this instance. States perform actions on data, the data is then
    /// used for object-level functions (such as translations) and then the
    /// state is updated.
    /// </summary>
	void FixedUpdate () {
        c_stateMachine.Act();

        Debug.Log(c_playerData.GetCurrentSpeed());
	}
}
