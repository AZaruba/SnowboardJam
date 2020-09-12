using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrickDisabledState : iState
{
    private TrickData trickData;

    public TrickDisabledState(ref TrickData dataIn)
    {
        trickData = dataIn;
    }

    public void Act()
    {

    }

    public void TransitionAct()
    {
        trickData.i_trickPoints = 0;
        trickData.f_trickTime = Constants.ZERO_F;
    }

    public StateRef GetNextState(Command cmd)
    {
        if (cmd == Command.READY_TRICK)
        {
            return StateRef.TRICK_READY;
        }
        return StateRef.TRICK_DISABLED;
    }
}
