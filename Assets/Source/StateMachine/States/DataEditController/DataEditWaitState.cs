using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataEditWaitState : iState
{
    private EditControllerData c_ctrlData;

    public DataEditWaitState(ref EditControllerData editIn)
    {
        this.c_ctrlData = editIn;
    }

    public void Act()
    {
        float currentTickTime = c_ctrlData.f_currentTickTime;
        IncrementCartridge.Increment(ref currentTickTime, Time.deltaTime, c_ctrlData.f_maxTickTime);
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
