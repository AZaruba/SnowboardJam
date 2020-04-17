using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeIncreaseState : iState
{
    IncrementCartridge cart_incr;
    TimerData c_timerData;

    public TimeIncreaseState(ref TimerData tData, ref IncrementCartridge incr)
    {
        this.cart_incr = incr;
        this.c_timerData = tData;
    }
    public void Act()
    {
        float currentTime = c_timerData.f_currentTime;

        cart_incr.Increment(ref currentTime, Time.deltaTime);

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
