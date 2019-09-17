using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine {
    
    #region Members
    private iState i_currentState;
    private List<iState> l_validStates;
    #endregion

    #region PublicFunctions
    /// <summary>
    /// Default Constructor for StateMachine
    /// </summary>
    public StateMachine()
    {
        l_validStates = new List<iState> ();
    }

    /// <summary>
    /// Initializes a StateMachine with a default state defined by owner.
    /// </summary>
    /// <param name="defaultState">Default state.</param>
    public StateMachine(iState defaultState)
    {
        l_validStates = new List<iState> ();
        l_validStates.Add(defaultState);
        i_currentState = defaultState;
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
    /// <returns>True if the command is valid</returns>
    public void Execute(Command cmd)
    {
        i_currentState = i_currentState.GetNextState(cmd);
    }

    public bool AddState(iState newState)
    {
        l_validStates.Add(newState);
        return true;
    }
    #endregion
}
