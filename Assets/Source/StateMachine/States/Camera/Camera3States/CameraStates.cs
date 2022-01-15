using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowState2 : iState
{
    private CameraTrackingData c_trackingData;
    private CameraMotionData c_motionData;
    private EntityPositionData c_positionData;

    public CameraFollowState2(ref CameraTrackingData trackingData,
        ref CameraMotionData motionData,
        ref EntityPositionData positionData)
    {
        this.c_trackingData = trackingData;
        this.c_motionData = motionData;
        this.c_positionData = positionData;
    }

    public void Act()
    {
        Vector3 currentPosition = c_positionData.v_position;
        Quaternion currentRotation = c_positionData.q_rotation;

        float currentVertVel = c_motionData.f_currentVerticalVelocity;
        float currentLatVel = c_motionData.f_currentLateralVelocity;
        float currentRotVel = c_motionData.f_currentRotationalVelocity;

        Vector3 axis = Vector3.Cross(currentRotation * Vector3.forward, Vector3.up);

        CameraCartridge.AccelerateVerticalVelocity(ref currentVertVel,
            c_motionData.f_maxVerticalVelocity,
            c_motionData.f_maxVerticalAngle,
            c_motionData.f_minVerticalAngle,
            axis,
            currentPosition,
            c_trackingData.v_position,
            c_motionData.v_targetOffset,
            c_trackingData.v_position - c_trackingData.v_position_lastFrame);

        CameraCartridge.AccelerateHorizontalVelocity(ref currentLatVel,
            c_motionData.f_maxLateralVelocity,
            c_motionData.f_maxFollowDistance,
            c_motionData.f_minFollowDistance,
            currentPosition,
            c_trackingData.v_position);

        // test
        currentRotation = Quaternion.LookRotation(c_trackingData.v_position - currentPosition, Vector3.up);
        Vector3 dir = Vector3.ProjectOnPlane(currentRotation * Vector3.forward, Vector3.up);
        currentPosition += Vector3.up * currentVertVel * Time.deltaTime;
        currentPosition += dir * currentLatVel * Time.deltaTime;

        c_positionData.v_position = currentPosition;
        c_positionData.q_rotation = currentRotation;

        c_motionData.f_currentVerticalVelocity = currentVertVel;
        c_motionData.f_currentLateralVelocity = currentLatVel;
        c_motionData.f_currentRotationalVelocity = currentRotVel;
    }

    public StateRef GetNextState(Command cmd)
    {
        return StateRef.FOLLOWING;
    }

    public void TransitionAct()
    {

    }
}

public class CameraSnapState : iState
{
    private CameraTrackingData c_trackingData;
    private EntityPositionData c_positionData;

    public CameraSnapState(ref CameraTrackingData trackingData,
        ref EntityPositionData positionData)
    {
        this.c_trackingData = trackingData;
        this.c_positionData = positionData;
    }

    public void Act()
    {

    }

    public StateRef GetNextState(Command cmd)
    {
        if (cmd == Command.FOLLOW)
        {
            return StateRef.FOLLOWING;
        }
        return StateRef.SNAPPING;
    }

    public void TransitionAct()
    {
        throw new System.NotImplementedException();
    }
}
