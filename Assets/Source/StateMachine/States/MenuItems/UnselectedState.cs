using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnselectedState : iState
{
    BasicMenuItemData c_basicData;
    MenuItemActiveData c_activeData;

    public UnselectedState(ref BasicMenuItemData basicData, ref MenuItemActiveData activeData)
    {
        c_basicData = basicData;
        c_activeData = activeData;
    }

    public void Act()
    {

    }

    public void TransitionAct()
    {
        c_activeData.v_itemPosition = c_activeData.v_targetItemPosition;
    }

    public StateRef GetNextState(Command cmd)
    {
        if (cmd == Command.SELECT)
        {
            return StateRef.ITEM_PRESELECTED;
        }
        return StateRef.ITEM_UNSELECTED;
    }
}
