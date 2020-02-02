using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeFollowState : iState
{
    private AngleAdjustmentCartridge cart_angle;
    private FollowCartridge cart_follow;
    private CameraData c_cameraData;

    public FreeFollowState(ref CameraData cameraData, ref AngleAdjustmentCartridge ang, ref FollowCartridge follow)
    {
        this.c_cameraData = cameraData;
        this.cart_angle = ang;
        this.cart_follow = follow;
    }

    public void Act()
    {
    }

    public void TransitionAct()
    {

    }

    public StateRef GetNextState(Command cmd)
    {
        if (cmd == Command.DRAG)
        {
            return StateRef.LEAVING;
        }
        else if (cmd == Command.APPROACH)
        {
            return StateRef.APPROACHING;
        }
        return StateRef.TRACKING;
    }
}
