using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeIncreaseState : iState
{
    TimerData c_timerData;

    public TimeIncreaseState(ref TimerData tData)
    {
        this.c_timerData = tData;
    }
    public void Act()
    {
        float currentTime = c_timerData.f_currentTime;

        IncrementCartridge.Increment(ref currentTime, Time.deltaTime);

        c_timerData.f_currentTime = currentTime;
    }

    public StateRef GetNextState(Command cmd)
    {
        if (cmd == Command.START_TIMER_DOWN)
        {
            return StateRef.TIMER_DECR;
        }
        else if (cmd == Command.STOP_TIMER)
        {
            return StateRef.PAUSED;
        }
        return StateRef.TIMER_INCR;
    }
    public void TransitionAct()
    {

    }
}
