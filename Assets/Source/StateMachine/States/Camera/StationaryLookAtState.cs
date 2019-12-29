using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationaryLookAtState : iState {

    #region Members
    FocusCartridge cart_focus;
    CameraData c_cameraData;
    #endregion

    public StationaryLookAtState(ref CameraData cameraData, ref FocusCartridge focus)
    {
        this.c_cameraData = cameraData;
        this.cart_focus = focus;
    }

    public void Act()
    {
        Vector3 currentPosition = c_cameraData.v_currentPosition;
        Vector3 currentTargetPosition = c_cameraData.v_targetPosition;
        Vector3 lookVector = c_cameraData.v_currentDirection;

        cart_focus.PointVectorAt(ref currentPosition, ref currentTargetPosition, ref lookVector);

        c_cameraData.v_currentDirection = lookVector;
    }

    public void TransitionAct()
    {

    }

    public StateRef GetNextState(Command cmd)
    {
        return StateRef.TRACKING;
    }
}
