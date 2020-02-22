using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine {
    
    #region Members
    private iState i_currentState;
    private StateRef sr_currentStateRef;
    private Dictionary<StateRef, iState> l_validStates;
    #endregion

    #region PublicFunctions
    /// <summary>
    /// Default Constructor for StateMachine
    /// </summary>
    public StateMachine()
    {
        l_validStates = new Dictionary<StateRef, iState> ();
    }

    /// <summary>
    /// Initializes a StateMachine with a default state defined by owner. Initialization
    /// includes a reference value identifying the state.
    /// </summary>
    /// <param name="defaultState">Default state.</param>
    /// <param name="stateRef">A StateRef value identifying the state.</param>
    public StateMachine(iState defaultState, StateRef stateRef)
    {
        l_validStates = new Dictionary<StateRef, iState> ();
        l_validStates.Add(stateRef, defaultState);
        i_currentState = defaultState;
        sr_currentStateRef = stateRef;
    }

    /// <summary>
    /// Performs the behavior of the current state
    /// </summary>
    public void Act()
    {
        i_currentState.Act();
    }

    /// <summary>
    /// Executes a command on the state machine, changing the state
    /// </summary>
    public void Execute(Command cmd, bool ForceTransition = false)
    {
        StateRef e_nextState = i_currentState.GetNextState(cmd);
        bool foundState = l_validStates.TryGetValue(e_nextState, out i_currentState);

        if (!foundState)
        {
            Debug.Log("ERROR: State Not Found!");
            return;
        }

        if (e_nextState != sr_currentStateRef || ForceTransition)
        {
            i_currentState.TransitionAct();
            sr_currentStateRef = e_nextState;
        }
    }

    public StateRef GetCurrentState()
    {
        return sr_currentStateRef;
    }

    /// <summary>
    /// Adds a new state to the list with a reference to the state.
    /// </summary>
    /// <returns><c>true</c>, if state was added</returns>
    /// <param name="newState">New state.</param>
    /// <param name="stateRef">State reference.</param>
    public bool AddState(iState newState, StateRef stateRef)
    {
        l_validStates.Add(stateRef, newState);
        return true;
    }
    #endregion
}
