using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeFollowState : iCameraState
{
    public void Act(ref CameraData c_cameraData)
    {

    }

    public void TransitionAct(ref CameraData c_cameraData)
    {

    }

    public StateRef GetNextState(Command cmd)
    {
        return StateRef.TRACKING;
    }
}
