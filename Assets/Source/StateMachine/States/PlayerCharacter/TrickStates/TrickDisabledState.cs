using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrickDisabledState : iState
{
    private TrickData c_trickData;
    private ScoringData c_scoringData;

    public TrickDisabledState(ref TrickData dataIn, ref ScoringData scoreIn)
    {
        this.c_trickData = dataIn;
        this.c_scoringData = scoreIn;
    }

    public void Act()
    {

    }

    public void TransitionAct()
    {
        c_trickData.i_trickPoints = 0;
        c_trickData.f_trickTime = Constants.ZERO_F;
        c_scoringData.b_sendTrick = true;
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
