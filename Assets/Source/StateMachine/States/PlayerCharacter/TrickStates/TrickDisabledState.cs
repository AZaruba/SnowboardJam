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
        Debug.Log("Points from Trick " + trickData.trick_right + ": " + Mathf.RoundToInt(trickData.i_trickPoints));
        trickData.i_trickPoints = 0;
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
