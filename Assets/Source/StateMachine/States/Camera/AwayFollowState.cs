using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwayFollowState : iCameraState
{
    private FocusCartridge cart_focus;
    private VelocityCartridge cart_vel;
    private FollowCartridge cart_follow;

    public AwayFollowState(ref FocusCartridge focus, ref VelocityCartridge vel, ref FollowCartridge follow)
    {
        this.cart_focus = focus;
        this.cart_vel = vel;
        this.cart_follow = follow;
    }

    public void Act(ref CameraData c_cameraData)
    {
        Vector3 currentPosition = c_cameraData.v_currentPosition;
        Vector3 targetPosition = c_cameraData.v_targetPosition;
        Vector3 lookVector = c_cameraData.v_currentDirection;

        cart_follow.LeaveTarget(ref currentPosition, targetPosition);
        cart_focus.PointVectorAt(ref currentPosition, ref targetPosition, ref lookVector);

        c_cameraData.v_currentDirection = lookVector;
        c_cameraData.v_currentPosition = currentPosition;
    }

    public void TransitionAct(ref CameraData c_cameraData)
    {

    }

    public StateRef GetNextState(Command cmd)
    {
        if (cmd == Command.APPROACH)
        {
            return StateRef.APPROACHING;
        }
        else if (cmd == Command.TRACK)
        {
            return StateRef.TRACKING;
        }
        return StateRef.LEAVING;
    }
}
