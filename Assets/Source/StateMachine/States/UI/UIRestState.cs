using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRestState : iState
{
    public void Act()
    {

    }

    public void TransitionAct()
    {

    }

    public StateRef GetNextState(Command cmd)
    {
        if (cmd == Command.PAUSE_SCORE)
        {
            return StateRef.SCORE_PAUSED;
        }
        if (cmd == Command.INCREMENT_SCORE)
        {
            return StateRef.SCORE_INCREASING;
        }
        return StateRef.SCORE_STAY;
    }
}
