﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTargetState : iState
{
    private FocusCartridge cart_focus;
    private CameraData c_cameraData;

    public LookAtTargetState(ref CameraData cameraData, ref FocusCartridge focus)
    {
        this.c_cameraData = cameraData;
        this.cart_focus = focus;
    }
    public void Act()
    {
        Vector3 currentPosition = c_cameraData.v_currentPosition;
        // Vector3 currentTargetPosition = c_cameraData.v_targetPosition;
        Vector3 currentTargetDirection = c_cameraData.v_targetDirection;
        Vector3 lookVector = c_cameraData.v_currentDirection;

        // cart_focus.PointVectorAt(ref currentPosition, ref currentTargetPosition, ref lookVector);
        cart_focus.PointVectorAtLerp(ref currentPosition, currentTargetDirection, Time.deltaTime);

        c_cameraData.v_currentDirection = lookVector;
    }

    public void TransitionAct()
    {

    }

    public StateRef GetNextState(Command cmd)
    {
        if (cmd == Command.POINT_IN_DIRECTION)
        {
            return StateRef.DIRECTED;
        }
        else if (cmd == Command.POINT_AT_POSITION)
        {
            return StateRef.POSED;
        }
        return StateRef.TARGETED;
    }
}
