using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightState : iState
{
    public void Act()
    {

    }

    public void TransitionAct()
    {

    }

    public StateRef GetNextState(Command cmd)
    {
        if (cmd == Command.FALL)
        {
            return StateRef.DISABLED;
        }
        if (cmd == Command.TURN)
        {
            return StateRef.CARVING;
        }
        if (cmd == Command.CRASH)
        {
            return StateRef.DISABLED;
        }
        return StateRef.RIDING;
    }
}
