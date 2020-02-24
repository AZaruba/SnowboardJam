using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostselectedState : iState
{
    BasicMenuItemData c_basicData;
    MenuItemActiveData c_activeData;

    public PostselectedState(ref BasicMenuItemData basicData, ref MenuItemActiveData activeData)
    {
        c_basicData = basicData;
        c_activeData = activeData;
    }

    public void Act()
    {
        Vector2 targetPos = c_activeData.v_targetItemPosition;
        Vector2 currentPos = c_activeData.v_itemPosition;

        currentPos = Vector2.Lerp(currentPos, targetPos, c_basicData.TransitionSpeed * Time.deltaTime);

        c_activeData.v_itemPosition = currentPos;
    }

    public void TransitionAct()
    {
        c_activeData.v_targetItemPosition = c_activeData.v_origin;
        c_activeData.c_currentColor = c_basicData.UnselectedColor;
    }

    public StateRef GetNextState(Command cmd)
    {
        if (cmd == Command.END_TRANSITION)
        {
            return StateRef.ITEM_UNSELECTED;
        }
        if (cmd == Command.SELECT)
        {
            return StateRef.ITEM_PRESELECTED;
        }
        return StateRef.ITEM_POSTSELECTED;
    }
}
