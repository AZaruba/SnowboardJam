using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuWaitState : iState
{
    private BasicMenuControllerData c_data;
    private ActiveMenuData c_activeData;
    private IncrementCartridge cart_incr;

    public MenuWaitState(ref BasicMenuControllerData dataIn, ref ActiveMenuData activeDataIn, ref IncrementCartridge incr)
    {
        this.c_data = dataIn;
        this.c_activeData = activeDataIn;
        this.cart_incr = incr;
    }

    public void Act()
    {
        float currentTickTime = c_activeData.f_currentMenuTickCount;

        cart_incr.Increment(ref currentTickTime, Time.deltaTime, c_activeData.f_currentMenuWaitCount);

        c_activeData.f_currentMenuTickCount = currentTickTime;
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

    }
}
