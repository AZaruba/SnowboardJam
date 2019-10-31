using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RidingState : iPlayerState {

    AngleCalculationCartridge cart_angleCalc;
    AccelerationCartridge     cart_acceleration;
    VelocityCartridge         cart_velocity;

    public RidingState(ref AngleCalculationCartridge angleCalc,
        ref AccelerationCartridge acceleration,
        ref VelocityCartridge velocity)
    {
        this.cart_angleCalc = angleCalc;
        this.cart_acceleration = acceleration;
        this.cart_velocity = velocity;
    }

    public void Act(ref PlayerData c_playerData)
    {
        // check for angle when implemented
        float currentVelocity = c_playerData.CurrentSpeed;
        float acceleration = c_playerData.Acceleration;
        float topSpeed = c_playerData.TopSpeed;
        Vector3 currentPos = c_playerData.CurrentPosition;
        Vector3 currentDir = c_playerData.CurrentDirection;
        Quaternion currentRotation = c_playerData.RotationBuffer;

        cart_acceleration.Accelerate(ref currentVelocity, ref acceleration, topSpeed);
        cart_velocity.UpdatePosition(ref currentPos, ref currentDir, ref currentVelocity);
        cart_angleCalc.ZeroRotation(ref currentRotation);

        c_playerData.CurrentSpeed = currentVelocity;
        c_playerData.Acceleration = acceleration;
        c_playerData.CurrentPosition = currentPos;
        c_playerData.RotationBuffer = currentRotation;
    }

    public void TransitionAct(ref PlayerData c_playerData)
    {
        Vector3 currentPosition = c_playerData.CurrentPosition;
        Vector3 currentNormal = c_playerData.CurrentNormal;
        Vector3 currentForward = c_playerData.CurrentDirection;
        Vector3 currentSurfaceNormal = c_playerData.CurrentSurfaceNormal;
        Vector3 currentSurfaceAttPoint = c_playerData.CurrentSurfaceAttachPoint;
        Quaternion rotationBuf = c_playerData.RotationBuffer;

        cart_angleCalc.ZeroRotation(ref rotationBuf);
        cart_angleCalc.AlignRotationWithSurface(ref currentSurfaceNormal, ref currentNormal, ref currentForward, ref rotationBuf);
        //cart_angleCalc.MoveToAttachPoint(ref currentPosition, ref currentSurfaceAttPoint);

        c_playerData.CurrentNormal = currentNormal;
        c_playerData.CurrentDirection = currentForward;
        c_playerData.CurrentPosition = currentPosition;
        c_playerData.RotationBuffer = rotationBuf;
        c_playerData.CurrentAirVelocity = 0.0f;
    }

    public StateRef GetNextState(Command cmd)
    {
        if (cmd == Command.FALL)
        {
            return StateRef.AIRBORNE;
        }
        return StateRef.RIDING;
    }
}
