using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeFollowState : iCameraState
{
    private FocusCartridge cart_focus;
    private AngleAdjustmentCartridge cart_angle;
    private FollowCartridge cart_follow;

    public FreeFollowState(ref FocusCartridge focus, ref AngleAdjustmentCartridge ang, ref FollowCartridge follow)
    {
        this.cart_focus = focus;
        this.cart_angle = ang;
        this.cart_follow = follow;
    }

    public void Act(ref CameraData c_cameraData)
    {
        Vector3 currentPosition = c_cameraData.v_currentPosition;
        Vector3 targetPosition = c_cameraData.v_targetPosition;
        Vector3 lookVector = c_cameraData.v_currentDirection;

        cart_follow.ApproachTarget(ref currentPosition, targetPosition);
        cart_angle.AdjustPositionOnRadius(ref currentPosition, c_cameraData.v_surfaceBelowCameraPosition, c_cameraData.f_followHeight);
        cart_focus.PointVectorAt(ref currentPosition, ref targetPosition, ref lookVector);

        c_cameraData.v_currentDirection = lookVector;
        c_cameraData.v_currentPosition = currentPosition;
    }

    public void TransitionAct(ref CameraData c_cameraData)
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
