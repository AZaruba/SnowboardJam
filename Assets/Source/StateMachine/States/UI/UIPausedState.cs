using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPausedState : iState
{
    public void Act()
    {

    }

    public void TransitionAct()
    {

    }

    public StateRef GetNextState(Command cmd)
    {
        if (cmd == Command.INCREMENT_SCORE)
        {
            return StateRef.SCORE_INCREASING;
        }
        if (cmd == Command.STOP_SCORE)
        {
            return StateRef.SCORE_STAY;
        }
        return StateRef.SCORE_PAUSED;
    }
}
