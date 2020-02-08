using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrickTransitionState : iState
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
        if (cmd == Command.SCORE_TRICK)
        {
            return StateRef.TRICKING;
        }
        if (cmd == Command.END_TRICK)
        {
            return StateRef.TRICK_READY;
        }
        return StateRef.TRICK_TRANSITION;
    }
}
