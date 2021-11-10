using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataEditDisabledState : iState
{
    private EditControllerData data;

    public DataEditDisabledState(ref EditControllerData dataIn)
    {
        this.data = dataIn;
    }

    public void Act()
    {

    }

    public StateRef GetNextState(Command cmd)
    {
        if (cmd == Command.MENU_SHOW)
        {
            return StateRef.MENU_READY;
        }
        return StateRef.MENU_DISABLED;
    }

    public void TransitionAct()
    {
        data.b_editorActive = false;
    }
}
