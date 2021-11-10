using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTickState : iState
{
    private ActiveMenuData c_activeData;

    public MenuTickState(ref ActiveMenuData activeDataIn)
    {
        this.c_activeData = activeDataIn;
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

        IncrementCartridge.Rotate(ref activeIndex, c_activeData.i_menuDir, c_activeData.i_menuItemCount, 0);

        c_activeData.i_activeMenuItemIndex = activeIndex;

        c_activeData.f_currentMenuTickCount = Constants.ZERO_F;
    }
}

public class MenuJumpState : iState
{
    private ActiveMenuData c_activeData;

    public MenuJumpState(ref ActiveMenuData activeDataIn)
    {
        this.c_activeData = activeDataIn;
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
        return StateRef.MENU_MOUSE;
    }

    public void TransitionAct()
    {
        c_activeData.i_activeMenuItemIndex = c_activeData.i_menuMousePositionItemIndex;

        c_activeData.f_currentMenuTickCount = Constants.ZERO_F;
    }
}
