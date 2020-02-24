using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedState : iState
{
    BasicMenuItemData c_basicData;
    MenuItemActiveData c_activeData;

    public SelectedState(ref BasicMenuItemData basicData, ref MenuItemActiveData activeData)
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
        if (cmd == Command.UNSELECT)
        {
            return StateRef.ITEM_POSTSELECTED;
        }
        return StateRef.ITEM_SELECTED;
    }
}
