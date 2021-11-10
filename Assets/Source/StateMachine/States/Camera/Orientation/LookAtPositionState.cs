using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPositionState : iState
{
    private CameraData c_cameraData;

    public LookAtPositionState(ref CameraData cameraData)
    {
        this.c_cameraData = cameraData;
    }

    public void Act()
    {
        Vector3 currentPosition = c_cameraData.v_currentPosition;
        Vector3 currentTargetPosition = c_cameraData.v_targetPosition;
        Vector3 lookVector = c_cameraData.v_currentDirection;

        FocusCartridge.PointVectorAt(ref currentPosition, ref currentTargetPosition, ref lookVector);

        c_cameraData.v_currentDirection = lookVector;
        c_cameraData.q_cameraRotation = Quaternion.FromToRotation(Vector3.forward, lookVector);
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
        else if (cmd == Command.POINT_AT_TARGET)
        {
            return StateRef.TARGETED;
        }
        return StateRef.POSED;
    }
}

