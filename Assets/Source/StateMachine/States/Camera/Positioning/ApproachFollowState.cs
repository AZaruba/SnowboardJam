using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApproachFollowState : iState
{
    private FocusCartridge cart_focus;
    private AngleAdjustmentCartridge cart_angle;
    private FollowCartridge cart_follow;
    private CameraData c_cameraData;

    public ApproachFollowState(ref CameraData cameraData, ref FocusCartridge focus, ref AngleAdjustmentCartridge ang, ref FollowCartridge follow)
    {
        this.c_cameraData = cameraData;
        this.cart_focus = focus;
        this.cart_angle = ang;
        this.cart_follow = follow;
    }

    // Let's start over!
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
        else if (cmd == Command.TRACK)
        {
            return StateRef.TRACKING;
        }
        return StateRef.APPROACHING;
    }
}
