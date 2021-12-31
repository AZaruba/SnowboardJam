using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerActiveState : iState
{
    // no state actions, just here to interact with pre-start and pausing
    public TimerActiveState()
    {

    }

    public void Act()
    {
    }

    public StateRef GetNextState(Command cmd)
    {
        return StateRef.TIMER_STEP;
    }

    public void TransitionAct()
    {

    }
}
