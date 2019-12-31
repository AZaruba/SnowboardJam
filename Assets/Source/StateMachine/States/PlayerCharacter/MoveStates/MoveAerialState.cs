using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAerialState : iState
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
        return StateRef.AIRBORNE;
    }
}
