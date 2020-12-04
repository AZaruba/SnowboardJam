using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreStartState : iState
{
    private StateRef nextState;
    public PreStartState(StateRef nextStateIn)
    {
        this.nextState = nextStateIn;
    }

    public void Act()
    {
        
    }

    public StateRef GetNextState(Command cmd)
    {
        if (cmd == Command.COUNTDOWN_OVER)
        {
            return nextState;
        }
        return StateRef.PRESTART_STATE;
    }

    public void TransitionAct()
    {
        throw new System.NotImplementedException();
    }
}
