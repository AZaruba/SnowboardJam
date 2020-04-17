using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausedTimeState : iState
{
    // TODO: create a universal StateData to ensure the stored state is always 
    StateRef storedState;

    public void Act()
    {
        // do nothing, we're paused!
    }

    public StateRef GetNextState(Command cmd)
    {
        return storedState;
    }

    public void TransitionAct()
    {

    }
}
