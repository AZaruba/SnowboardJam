using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrickReadyState : iState
{

    public void Act()
    {

    }

    public void TransitionAct()
    {

    }

    public StateRef GetNextState(Command cmd)
    {
        if (cmd == Command.LAND)
        {
            return StateRef.TRICK_DISABLED; // crash!
        }
        if (cmd == Command.START_TRICK)
        {
            return StateRef.TRICK_TRANSITION;
        }
        return StateRef.TRICK_READY;
    }
}
