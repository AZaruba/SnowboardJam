using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecrementCountdownState : iState
{
    private CountdownData c_data;
    public DecrementCountdownState(ref CountdownData dataIn)
    {
        this.c_data = dataIn;
    }

    public void Act()
    {
        float currentTime = c_data.f_currentCountdownTime;

        IncrementCartridge.Decrement(ref currentTime, Time.deltaTime, c_data.i_targetTime);

        c_data.f_currentCountdownTime = currentTime;

    }

    public StateRef GetNextState(Command cmd)
    {
        if (cmd == Command.START_TIMER_DOWN)
        {
            return StateRef.TIMER_DECR;
        }
        return StateRef.TIMER_STEP;
    }

    public void TransitionAct()
    {
        // nothing to do
    }
}

public class CountdownStepState : iState
{
    private CountdownData c_data;
    public CountdownStepState(ref CountdownData dataIn)
    {
        this.c_data = dataIn;
    }

    public void Act()
    {
        // will never act
    }

    public StateRef GetNextState(Command cmd)
    {
        if (cmd == Command.TICK_TIMER)
        {
            return StateRef.TIMER_STEP;
        }
        return StateRef.TIMER_DECR;
    }

    public void TransitionAct()
    {
        c_data.i_countdownTime--; // always decrement once
    }
}

public class CountdownStartState : iState
{
    public CountdownStartState() { }

    public void Act()
    {
        // will never act
    }

    public StateRef GetNextState(Command cmd)
    {
        if (cmd == Command.START_COUNTDOWN)
        {
            return StateRef.TIMER_DECR;
        }
        return StateRef.START_STATE;
    }

    public void TransitionAct()
    {
        // no transition needed
    }
}
