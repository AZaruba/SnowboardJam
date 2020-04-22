using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuReadyState : iState
{
    private ActiveMenuData c_activeData;

    public MenuReadyState(ref ActiveMenuData dataIn)
    {
        this.c_activeData = dataIn;
    }

    public void Act()
    {
        
    }

    public StateRef GetNextState(Command cmd)
    {
        if (cmd == Command.MENU_TICK_INPUT)
        {
            return StateRef.MENU_TICK;
        }
        return StateRef.MENU_READY;
    }

    public void TransitionAct()
    {

    }
}
