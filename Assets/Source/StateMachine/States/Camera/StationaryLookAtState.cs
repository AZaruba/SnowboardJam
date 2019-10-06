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
        Vector3 currentPosition = c_cameraData.GetCurrentPosition();
        Vector3 currentTargetPosition = c_cameraData.GetTargetPosition();
        Vector3 lookVector = c_cameraData.GetCurrentDirection();


        cart_focus.PointVectorAt(ref currentPosition, ref currentTargetPosition, ref lookVector);

        c_cameraData.SetCurrentDirection(lookVector);
    }

    public StateRef GetNextState(Command cmd)
    {
        return StateRef.TRACKING;
    }
}
