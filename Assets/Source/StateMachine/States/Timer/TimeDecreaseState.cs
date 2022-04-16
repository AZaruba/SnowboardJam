using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeDecreaseState : iState
{
    TimerData c_timerData;

    public TimeDecreaseState(ref TimerData tData)
    {
        this.c_timerData = tData;
    }
    public void Act()
    {
        float currentTime = c_timerData.f_currentTime;

        IncrementCartridge.Decrement(ref currentTime, Time.deltaTime, Constants.ZERO_F);

        c_timerData.f_currentTime = currentTime;
    }

    public StateRef GetNextState(Command cmd)
    {
        if (cmd == Command.START_TIMER_UP)
        {
            return StateRef.TIMER_INCR;
        }
        else if (cmd == Command.STOP_TIMER)
        {
            return StateRef.PAUSED;
        }
        return StateRef.TIMER_DECR;
    }
    public void TransitionAct()
    {

    }
}
