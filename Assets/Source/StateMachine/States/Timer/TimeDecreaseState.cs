using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeDecreaseState : iState
{
    IncrementCartridge cart_incr;
    TimerData c_timerData;

    public TimeDecreaseState(ref TimerData tData, ref IncrementCartridge incr)
    {
        this.cart_incr = incr;
        this.c_timerData = tData;
    }
    public void Act()
    {
        float currentTime = c_timerData.f_currentTime;

        cart_incr.Decrement(ref currentTime, Time.deltaTime, Constants.ZERO_F);

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
