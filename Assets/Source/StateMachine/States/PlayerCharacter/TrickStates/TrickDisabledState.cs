using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrickDisabledState : iState
{

    public void Act()
    {

    }

    public void TransitionAct()
    {

    }

    public StateRef GetNextState(Command cmd)
    {
        if (cmd == Command.READY_TRICK)
        {
            return StateRef.TRICK_READY;
        }
        return StateRef.TRICK_DISABLED;
    }
}
