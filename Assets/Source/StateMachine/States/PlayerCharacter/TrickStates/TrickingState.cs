using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrickingState : iState
{
    private IncrementCartridge cart_incr;

    private TrickData trickData;

    public TrickingState(ref TrickData trickIn, ref IncrementCartridge cartIn)
    {
        cart_incr = cartIn;
        trickData = trickIn;
    }

    public void Act()
    {
        float points = trickData.i_trickPoints;
        int pointDelta = trickData.t_activeTrick.pointValue;

        cart_incr.Increment(ref points, Time.deltaTime * pointDelta);

        trickData.i_trickPoints = points;
    }

    public void TransitionAct()
    {
        trickData.t_activeTrick = trickData.trick_data_right;
    }

    public StateRef GetNextState(Command cmd)
    {
        if (cmd == Command.LAND)
        {
            return StateRef.TRICK_DISABLED; // crash!
        }
        if (cmd == Command.END_TRICK)
        {
            return StateRef.TRICK_TRANSITION;
        }
        return StateRef.TRICKING;
    }
}
