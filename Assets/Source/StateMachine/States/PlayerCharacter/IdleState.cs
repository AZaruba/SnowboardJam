using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class IdleState : iState
{
    public void Act()
    {

    }

    public void TransitionAct()
    {

    }

    public StateRef GetNextState(Command cmd)
    {
        if (cmd == Command.REST)
        {
            // return to ready
            return StateRef.STATIONARY;
        }
        return StateRef.CRASHED;
    }
}
