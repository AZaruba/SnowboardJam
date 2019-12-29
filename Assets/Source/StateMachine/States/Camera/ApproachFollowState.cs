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

    public void Act()
    {
        Vector3 currentPosition = c_cameraData.v_currentPosition;
        Vector3 targetPosition = c_cameraData.v_targetPosition;
        Vector3 lookVector = c_cameraData.v_currentDirection;

        cart_follow.ApproachTarget(ref currentPosition, targetPosition, Vector3.Distance(currentPosition, targetPosition));
        cart_angle.AdjustPositionOnRadius(ref currentPosition, c_cameraData.v_surfaceBelowCameraPosition, c_cameraData.f_followHeight);
        cart_focus.PointVectorAt(ref currentPosition, ref targetPosition, ref lookVector);

        c_cameraData.v_currentDirection = lookVector;
        c_cameraData.v_currentPosition = currentPosition;
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
