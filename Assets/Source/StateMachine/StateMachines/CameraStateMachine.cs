using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraStateMachine {

    #region Members
    private iCameraState i_currentState;
    private Dictionary<StateRef, iCameraState> l_validStates;
    #endregion

    public CameraStateMachine()
    {
        l_validStates = new Dictionary<StateRef, iCameraState> ();
    }

    public CameraStateMachine(iCameraState defaultState, StateRef stateRef)
    {
        l_validStates = new Dictionary<StateRef, iCameraState> ();
        l_validStates.Add(stateRef, defaultState);
        i_currentState = defaultState;
    }

    /// <summary>
    /// Performs the behavior of the current state
    /// </summary>
    public void Act(ref CameraData c_cameraData)
    {
        i_currentState.Act(ref c_cameraData);
    }

    /// <summary>
    /// Executes a command on the state machine, changing the state
    /// </summary>
    public void Execute(Command cmd)
    {
        StateRef e_nextState = i_currentState.GetNextState(cmd);
        bool foundState = l_validStates.TryGetValue(e_nextState, out i_currentState);

        if (!foundState)
        {
            Debug.Log("ERROR: State Not Found!");
        }
    }

    /// <summary>
    /// Adds a new state to the list with a reference to the state.
    /// </summary>
    /// <returns><c>true</c>, if state was added</returns>
    /// <param name="newState">New state.</param>
    /// <param name="stateRef">State reference.</param>
    public bool AddState(iCameraState newState, StateRef stateRef)
    {
        l_validStates.Add(stateRef, newState);
        return true;
    }
}
