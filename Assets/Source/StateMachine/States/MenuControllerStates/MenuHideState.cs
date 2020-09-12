using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuHideState : iState
{
    private ActiveMenuData c_activeData;
    private BasicMenuControllerData c_basicData;
    private LerpCartridge cart_lerp;

    public MenuHideState(ref ActiveMenuData dataIn, ref BasicMenuControllerData basicDataIn, ref LerpCartridge cart_lerp)
    {
        this.c_activeData = dataIn;
        this.c_basicData = basicDataIn;
        this.cart_lerp = cart_lerp;
    }

    public void Act()
    {
        Vector2 targetPos = c_activeData.v_targetPosition;
        Vector2 currentPos = c_activeData.v_currentPosition;

        float currentOpacity = c_activeData.f_currentOpacity;
        float targetOpacity = c_basicData.DisabledOpacity;

        cart_lerp.LerpVector2(ref currentPos, targetPos, Time.deltaTime * 10);
        cart_lerp.LerpFloat(ref currentOpacity, targetOpacity);

        c_activeData.v_currentPosition = currentPos;
        c_activeData.f_currentOpacity = currentOpacity;
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
        c_activeData.v_targetPosition = c_basicData.DisabledPosition;
    }
}
