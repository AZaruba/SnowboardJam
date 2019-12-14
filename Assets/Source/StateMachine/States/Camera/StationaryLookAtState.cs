using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationaryLookAtState : iCameraState {

    #region Members
    FocusCartridge cart_focus;
    #endregion

    public StationaryLookAtState(ref FocusCartridge focus)
    {
        cart_focus = focus;
    }

    public void Act(ref CameraData c_cameraData)
    {
        Vector3 currentPosition = c_cameraData.v_currentPosition;
        Vector3 currentTargetPosition = c_cameraData.v_targetPosition;
        Vector3 lookVector = c_cameraData.v_currentDirection;


        cart_focus.PointVectorAt(ref currentPosition, ref currentTargetPosition, ref lookVector);

        c_cameraData.v_currentDirection =lookVector;
    }

    public void TransitionAct(ref CameraData c_CameraData)
    {

    }

    public StateRef GetNextState(Command cmd)
    {
        return StateRef.TRACKING;
    }
}
