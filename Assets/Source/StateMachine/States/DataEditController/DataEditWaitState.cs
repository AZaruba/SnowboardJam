using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataEditWaitState : iState
{
    private EditControllerData c_ctrlData;
    private IncrementCartridge cart_incr;

    public DataEditWaitState(ref EditControllerData editIn, ref IncrementCartridge incr)
    {
        this.c_ctrlData = editIn;
        this.cart_incr = incr;
    }

    public void Act()
    {
        float currentTickTime = c_ctrlData.f_currentTickTime;
        cart_incr.Increment(ref currentTickTime, Time.deltaTime, c_ctrlData.f_maxTickTime);
        c_ctrlData.f_currentTickTime = currentTickTime;
    }

    public StateRef GetNextState(Command cmd)
    {
        if (cmd == Command.MENU_HIDE)
        {
            return StateRef.MENU_DISABLED;
        }
        if (cmd == Command.MENU_READY)
        {
            return StateRef.MENU_READY;
        }
        return StateRef.MENU_WAIT;
    }

    public void TransitionAct()
    {
        c_ctrlData.f_currentTickTime = 0.0f;
    }
}
