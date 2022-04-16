using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrickingState : iState
{

    private TrickData c_trickData;
    private ScoringData c_scoreData;

    public TrickingState(ref TrickData trickIn, ref ScoringData scoreIn)
    {
        c_trickData = trickIn;
        c_scoreData = scoreIn;
    }

    public void Act()
    {
        float trickTime = c_trickData.f_trickTime;

        IncrementCartridge.Increment(ref trickTime, Time.fixedDeltaTime);

        c_trickData.f_trickTime = trickTime;
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
        if (cmd == Command.END_TRICK)
        {
            c_scoreData.l_trickList.Add(c_trickData.t_activeTrickName);
            c_scoreData.l_timeList.Add(c_trickData.f_trickTime);

            c_trickData.f_trickTime = Constants.ZERO_F;
            c_trickData.t_activeTrickName = TrickName.BLANK_TRICK;
            return StateRef.TRICK_TRANSITION;
        }
        return StateRef.TRICKING;
    }
}
