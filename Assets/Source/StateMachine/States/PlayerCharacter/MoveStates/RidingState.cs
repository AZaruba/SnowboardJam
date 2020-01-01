using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RidingState : iState {

    AngleCalculationCartridge cart_angleCalc;
    AccelerationCartridge     cart_f_acceleration;
    VelocityCartridge         cart_velocity;
    PlayerData c_playerData;

    public RidingState(ref PlayerData playerData, ref AngleCalculationCartridge angleCalc,
        ref AccelerationCartridge f_acceleration,
        ref VelocityCartridge velocity)
    {
        this.c_playerData = playerData;
        this.cart_angleCalc = angleCalc;
        this.cart_f_acceleration = f_acceleration;
        this.cart_velocity = velocity;
    }

    public void Act()
    {
        // check for angle when implemented
        float currentVelocity = c_playerData.f_currentSpeed;
        float f_acceleration = c_playerData.f_acceleration;
        float topSpeed = c_playerData.f_topSpeed;
        Vector3 currentPosition = c_playerData.v_currentPosition;
        Vector3 currentDir = c_playerData.v_currentDirection;
        Vector3 currentNormal = c_playerData.v_currentNormal;
        Vector3 currentSurfaceNormal = c_playerData.v_currentSurfaceNormal;
        Vector3 currentSurfacePosition = c_playerData.v_currentSurfaceAttachPoint;
        Quaternion currentRotation = c_playerData.q_currentRotation;

        cart_f_acceleration.Accelerate(ref currentVelocity, ref f_acceleration, topSpeed);
        cart_velocity.RaycastAdjustment(ref currentSurfacePosition, ref currentPosition, ref currentRotation);
        cart_angleCalc.AlignRotationWithSurface(ref currentSurfaceNormal, ref currentNormal, ref currentDir, ref currentRotation);
        cart_velocity.UpdatePosition(ref currentPosition, ref currentDir, ref currentVelocity);

        c_playerData.f_currentSpeed = currentVelocity;
        c_playerData.f_acceleration = f_acceleration;
        c_playerData.v_currentPosition = currentPosition;
        c_playerData.v_currentNormal = currentNormal.normalized;
        c_playerData.v_currentDown = currentNormal.normalized * -1;
        c_playerData.v_currentDirection = currentDir.normalized;
        c_playerData.q_currentRotation = currentRotation;
    }

    public void TransitionAct()
    {
        c_playerData.f_currentAirVelocity = 0.0f;
        c_playerData.f_currentRaycastDistance = c_playerData.f_raycastDistance;
        c_playerData.v_currentDown = c_playerData.v_currentSurfaceNormal * -1;
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
