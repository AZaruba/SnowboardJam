using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RidingState : iState {

    AngleCalculationCartridge cart_angleCalc;
    AccelerationCartridge     cart_acceleration;
    VelocityCartridge         cart_velocity;
    PlayerData c_playerData;

    public RidingState(ref PlayerData playerData, ref AngleCalculationCartridge angleCalc,
        ref AccelerationCartridge acceleration,
        ref VelocityCartridge velocity)
    {
        this.c_playerData = playerData;
        this.cart_angleCalc = angleCalc;
        this.cart_acceleration = acceleration;
        this.cart_velocity = velocity;
    }

    public void Act()
    {
        // check for angle when implemented
        float currentVelocity = c_playerData.CurrentSpeed;
        float acceleration = c_playerData.Acceleration;
        float topSpeed = c_playerData.TopSpeed;
        Vector3 currentPosition = c_playerData.CurrentPosition;
        Vector3 currentDir = c_playerData.CurrentDirection;
        Vector3 currentNormal = c_playerData.CurrentNormal;
        Vector3 currentSurfaceNormal = c_playerData.CurrentSurfaceNormal;
        Vector3 currentSurfacePosition = c_playerData.CurrentSurfaceAttachPoint;
        Quaternion currentRotation = c_playerData.RotationBuffer;

        cart_acceleration.Accelerate(ref currentVelocity, ref acceleration, topSpeed);
        cart_velocity.RaycastAdjustment(ref currentSurfacePosition, ref currentPosition, ref currentRotation);
        cart_angleCalc.AlignRotationWithSurface(ref currentSurfaceNormal, ref currentNormal, ref currentDir, ref currentRotation);
        cart_velocity.UpdatePosition(ref currentPosition, ref currentDir, ref currentVelocity);

        c_playerData.CurrentSpeed = currentVelocity;
        c_playerData.Acceleration = acceleration;
        c_playerData.CurrentPosition = currentPosition;
        c_playerData.CurrentNormal = currentNormal.normalized;
        c_playerData.CurrentDown = currentNormal.normalized * -1;
        c_playerData.CurrentDirection = currentDir.normalized;
        c_playerData.RotationBuffer = currentRotation;
    }

    public void TransitionAct()
    {
        c_playerData.CurrentAirVelocity = 0.0f;
        c_playerData.f_currentRaycastDistance = c_playerData.f_raycastDistance;
        c_playerData.CurrentDown = c_playerData.CurrentSurfaceNormal * -1;
    }

    public StateRef GetNextState(Command cmd)
    {
        if (cmd == Command.FALL)
        {
            return StateRef.AIRBORNE;
        }
        if (cmd == Command.SLOW)
        {
            return StateRef.STOPPING;
        }
        return StateRef.RIDING;
    }
}
