using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIIncrementState : iState
{
    private ScoreDisplayData data_score;

    public UIIncrementState(ref ScoreDisplayData dataIn)
    {
        data_score = dataIn;
    }

    public void Act()
    {
        int displayScore = data_score.i_displayScore;

        IncrementCartridge.Increment(ref displayScore, 1);

        data_score.i_displayScore = displayScore;
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
        if (cmd == Command.STOP_SCORE)
        {
            return StateRef.SCORE_STAY;
        }
        return StateRef.SCORE_INCREASING;
    }
}
