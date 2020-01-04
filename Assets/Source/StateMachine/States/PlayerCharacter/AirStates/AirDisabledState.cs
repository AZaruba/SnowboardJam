using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirDisabledState : iState
{
    public void Act()
    {

    }

    public void TransitionAct()
    {

    }

    public StateRef GetNextState(Command cmd)
    {
        if (cmd == Command.READY)
        {
            return StateRef.GROUNDED;
        }
        return StateRef.DISABLED;
    }
}
