using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataEditReadyState : iState
{
    private EditControllerData c_activeData;

    public DataEditReadyState(ref EditControllerData dataIn)
    {
        this.c_activeData = dataIn;
    }

    public void Act()
    {

    }

    public StateRef GetNextState(Command cmd)
    {
        if (cmd == Command.MENU_HIDE)
        {
            return StateRef.MENU_DISABLED;
        }
        if (cmd == Command.MENU_TICK_INPUT)
        {
            return StateRef.MENU_TICK;
        }
        return StateRef.MENU_READY;
    }

    public void TransitionAct()
    {
        c_activeData.b_editorActive = true;
    }
}

