using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostselectedState : iState
{
    BasicMenuItemData c_basicData;
    MenuItemActiveData c_activeData;
    LerpCartridge c_lerpCart;

    public PostselectedState(ref BasicMenuItemData basicData, ref MenuItemActiveData activeData, ref LerpCartridge lerpcart)
    {
        c_basicData = basicData;
        c_activeData = activeData;
        c_lerpCart = lerpcart;
    }

    public void Act()
    {
        Vector2 targetPos = c_activeData.v_targetItemPosition;
        Vector2 currentPos = c_activeData.v_itemPosition;
        Color currentCol = c_activeData.c_targetColor;
        Color targetCol = c_activeData.c_targetColor;

        c_lerpCart.LerpVector2(ref currentPos, targetPos, c_basicData.TransitionSpeed * Time.deltaTime);
        c_lerpCart.LerpColor(ref currentCol, targetCol, c_basicData.TransitionSpeed * Time.deltaTime);

        c_activeData.v_itemPosition = currentPos;
        c_activeData.c_currentColor = currentCol;
    }

    public void TransitionAct()
    {
        c_activeData.v_targetItemPosition = c_activeData.v_origin;
        c_activeData.c_targetColor = c_basicData.UnselectedColor;
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
