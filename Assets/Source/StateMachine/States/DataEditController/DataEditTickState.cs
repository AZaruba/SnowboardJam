using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataEditTickState : iState
{
    private ActiveMenuData c_activeData;
    private IncrementCartridge cart_incr;

    public DataEditTickState(ref ActiveMenuData activeDataIn, ref IncrementCartridge incr)
    {
        this.c_activeData = activeDataIn;
        this.cart_incr = incr;
    }

    public void Act()
    {
        // this happens over the course of one frame
    }

    public StateRef GetNextState(Command cmd)
    {
        if (cmd == Command.MENU_HIDE)
        {
            return StateRef.MENU_DISABLED;
        }
        if (cmd == Command.MENU_IDLE)
        {
            return StateRef.MENU_WAIT;
        }
        return StateRef.MENU_TICK;
    }

    public void TransitionAct()
    {
        int activeIndex = c_activeData.i_activeMenuItemIndex;

        cart_incr.Rotate(ref activeIndex, c_activeData.i_menuDir, c_activeData.i_menuItemCount, 0);

        c_activeData.i_activeMenuItemIndex = activeIndex;

        c_activeData.f_currentMenuTickCount = Constants.ZERO_F;
    }
}
