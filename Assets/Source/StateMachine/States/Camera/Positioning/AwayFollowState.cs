using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwayFollowState : iState
{
    private FollowCartridge cart_follow;
    private CameraData c_cameraData;

    public AwayFollowState(ref CameraData cameraData, ref FollowCartridge follow)
    {
        this.c_cameraData = cameraData;
        this.cart_follow = follow;
    }

    // Let's start over!
    public void Act()
    {
        Vector3 currentPosition = c_cameraData.v_currentPosition;
        Vector3 targetPosition = c_cameraData.v_targetPosition;
        Vector3 offsetVector = c_cameraData.v_offsetVector;

        // do the things
        cart_follow.ApproachTarget(ref currentPosition, targetPosition, offsetVector, Vector3.Distance(currentPosition,targetPosition) * Time.deltaTime);

        c_cameraData.v_currentPosition = currentPosition;
    }

    public void TransitionAct()
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
