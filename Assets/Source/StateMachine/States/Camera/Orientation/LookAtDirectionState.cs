﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtDirectionState : iState
{
    private FocusCartridge cart_focus;
    private CameraData c_cameraData;

    public LookAtDirectionState(ref CameraData cameraData, ref FocusCartridge focus)
    {
        this.c_cameraData = cameraData;
        this.cart_focus = focus;
    }

    public void Act()
    {
        // do nothing
    }

    public void TransitionAct()
    {

    }

    public StateRef GetNextState(Command cmd)
    {
        if (cmd == Command.POINT_AT_POSITION)
        {
            return StateRef.POSED;
        }
        else if (cmd == Command.POINT_AT_TARGET)
        {
            return StateRef.TARGETED;
        }
        return StateRef.DIRECTED;
    }
}
