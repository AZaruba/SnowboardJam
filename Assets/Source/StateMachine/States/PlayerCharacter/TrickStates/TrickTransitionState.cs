using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrickTransitionState : iState
{
    private TrickData c_trickData;
    private ScoringData c_scoreData;

    public TrickTransitionState(ref TrickData trickIn, ref ScoringData scoreIn)
    {
        c_trickData = trickIn;
        c_scoreData = scoreIn;
    }

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
