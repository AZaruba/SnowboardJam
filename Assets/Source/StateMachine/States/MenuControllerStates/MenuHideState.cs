using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuHideState : iState
{
    private ActiveMenuData c_activeData;
    private LerpCartridge cart_lerp;

    public MenuHideState(ref ActiveMenuData dataIn, ref LerpCartridge cart_lerp)
    {
        this.c_activeData = dataIn;
        this.cart_lerp = cart_lerp;
    }

    public void Act()
    {
        Vector2 targetPos = c_activeData.v_targetPosition;
        Vector2 currentPos = c_activeData.v_currentPosition;

        cart_lerp.LerpVector2(ref currentPos, targetPos, Time.deltaTime * 10);

        c_activeData.v_currentPosition = currentPos;
    }

    public StateRef GetNextState(Command cmd)
    {
        if (cmd == Command.MENU_SHOW)
        {
            return StateRef.MENU_SHOWN;
        }
        return StateRef.MENU_HIDDEN;
    }

    public void TransitionAct()
    {
        c_activeData.v_targetPosition = new Vector2(-1040, 0);
    }
}
