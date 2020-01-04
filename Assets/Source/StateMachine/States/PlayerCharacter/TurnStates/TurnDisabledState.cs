using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnDisabledState : iState
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
            return StateRef.RIDING;
        }
        if (cmd == Command.READY)
        {
            return StateRef.RIDING;
        }
        return StateRef.DISABLED;
    }
}
