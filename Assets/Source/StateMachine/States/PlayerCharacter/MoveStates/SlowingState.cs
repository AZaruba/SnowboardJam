using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowingState : iState
{
    AccelerationCartridge cart_f_acceleration;
    VelocityCartridge cart_velocity;
    AngleCalculationCartridge cart_angleCalc;
    PlayerData c_playerData;

    public SlowingState(ref PlayerData playerData, ref VelocityCartridge vel, 
        ref AccelerationCartridge accel, ref AngleCalculationCartridge angleCalc)
    {
        c_playerData = playerData;
        cart_velocity = vel;
        cart_f_acceleration = accel;
        cart_angleCalc = angleCalc;
    }
    public void Act()
    {
        // check for angle when implemented
        float currentVelocity = c_playerData.f_currentSpeed;
        float deceleration = c_playerData.f_brakePower;
        Vector3 currentPosition = c_playerData.v_currentPosition;
        Vector3 currentDir = c_playerData.v_currentDirection;
        Vector3 currentNormal = c_playerData.v_currentNormal;
        Vector3 currentSurfaceNormal = c_playerData.v_currentSurfaceNormal;
        Vector3 currentSurfacePosition = c_playerData.v_currentSurfaceAttachPoint;
        Quaternion currentRotation = c_playerData.q_currentRotation;

        cart_f_acceleration.Decelerate(ref currentVelocity, deceleration);
        cart_velocity.RaycastAdjustment(ref currentSurfacePosition, ref currentPosition, ref currentRotation);
        cart_angleCalc.AlignRotationWithSurface(ref currentSurfaceNormal, ref currentNormal, ref currentDir, ref currentRotation);
        cart_velocity.UpdatePosition(ref currentPosition, ref currentDir, ref currentVelocity);

        c_playerData.f_currentSpeed = currentVelocity;
        c_playerData.v_currentPosition = currentPosition;
        c_playerData.v_currentNormal = currentNormal.normalized;
        c_playerData.v_currentDown = currentNormal.normalized * -1;
        c_playerData.v_currentDirection = currentDir.normalized;
        c_playerData.q_currentRotation = currentRotation;
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
        return StateRef.STOPPING;
    }
}
