using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationaryState : iState {

    public StationaryState()
    {

    }

    public void Act(ref PlayerData c_playerData)
    {
        // if stationary, do nothing (potentially add idle animations here)
    }

    /// <summary>
    /// Returns the state after the given command.
    /// </summary>
    /// <returns>An iState following a given Command, or this if none.</returns>
    /// <param name="cmd">The command</param>
    public iState GetNextState(Command cmd)
    {
        // TODO: Replace this with states held by the state machine to prevent constructor calls
        if (cmd == Command.ACCELERATE)
        {
            return new AcceleratingState ();
        }
        return this;
    }
}
