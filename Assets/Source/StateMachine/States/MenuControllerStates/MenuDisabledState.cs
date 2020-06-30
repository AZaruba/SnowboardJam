using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class MenuDisabledState : iState
{
    public MenuDisabledState()
    {

    }

    public void Act()
    {

    }

    public StateRef GetNextState(Command cmd)
    {
        if (cmd == Command.MENU_SHOW)
        {
            return StateRef.MENU_READY;
        }
        return StateRef.MENU_DISABLED;
    }

    public void TransitionAct()
    {

    }
}
