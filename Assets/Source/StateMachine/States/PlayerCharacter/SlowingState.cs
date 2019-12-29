using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowingState : iState
{
    AccelerationCartridge cart_acceleration;
    VelocityCartridge cart_velocity;
    AngleCalculationCartridge cart_angleCalc;
    PlayerData c_playerData;

    public SlowingState(ref PlayerData playerData, ref VelocityCartridge vel, 
        ref AccelerationCartridge accel, ref AngleCalculationCartridge angleCalc)
    {
        c_playerData = playerData;
        cart_velocity = vel;
        cart_acceleration = accel;
        cart_angleCalc = angleCalc;
    }
    public void Act()
    {
        // check for angle when implemented
        float currentVelocity = c_playerData.CurrentSpeed;
        float deceleration = c_playerData.f_brakePower;
        float topSpeed = c_playerData.TopSpeed;
        Vector3 currentPosition = c_playerData.CurrentPosition;
        Vector3 currentDir = c_playerData.CurrentDirection;
        Vector3 currentNormal = c_playerData.CurrentNormal;
        Vector3 currentSurfaceNormal = c_playerData.CurrentSurfaceNormal;
        Vector3 currentSurfacePosition = c_playerData.CurrentSurfaceAttachPoint;
        Quaternion currentRotation = c_playerData.RotationBuffer;

        cart_acceleration.Decelerate(ref currentVelocity, deceleration);
        cart_velocity.RaycastAdjustment(ref currentSurfacePosition, ref currentPosition, ref currentRotation);
        cart_angleCalc.AlignRotationWithSurface(ref currentSurfaceNormal, ref currentNormal, ref currentDir, ref currentRotation);
        cart_velocity.UpdatePosition(ref currentPosition, ref currentDir, ref currentVelocity);

        c_playerData.CurrentSpeed = currentVelocity;
        c_playerData.CurrentPosition = currentPosition;
        c_playerData.CurrentNormal = currentNormal.normalized;
        c_playerData.CurrentDown = currentNormal.normalized * -1;
        c_playerData.CurrentDirection = currentDir.normalized;
        c_playerData.RotationBuffer = currentRotation;
    }

    public void TransitionAct()
    {

    }

    public StateRef GetNextState(Command cmd)
    {
        if (cmd == Command.FALL)
        {
            return StateRef.AIRBORNE;
        }
        if (cmd == Command.RIDE)
        {
            return StateRef.RIDING;
        }
        if (cmd == Command.STOP)
        {
            return StateRef.STATIONARY;
        }
        if (cmd == Command.TURN)
        {
            return StateRef.STOPPING;
        }
        return StateRef.STOPPING;
    }
}
