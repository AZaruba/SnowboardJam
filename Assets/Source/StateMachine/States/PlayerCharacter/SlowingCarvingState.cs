using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowingCarvingState : iState
{
    AngleCalculationCartridge cart_angleCalc;
    AccelerationCartridge cart_acceleration;
    VelocityCartridge cart_velocity;
    HandlingCartridge cart_handling;
    PlayerData c_playerData;

    public SlowingCarvingState(ref PlayerData playerData,
        ref AngleCalculationCartridge angleCalc,
        ref AccelerationCartridge acceleration,
        ref VelocityCartridge velocity,
        ref HandlingCartridge handling)
    {
        this.c_playerData = playerData;
        this.cart_angleCalc = angleCalc;
        this.cart_acceleration = acceleration;
        this.cart_velocity = velocity;
        this.cart_handling = handling;
    }

    public void Act()
    {
        // check for angle when implemented
        float currentVelocity = c_playerData.CurrentSpeed;
        float deceleration = c_playerData.f_brakePower;
        Vector3 currentPosition = c_playerData.CurrentPosition;
        Vector3 currentDir = c_playerData.CurrentDirection;
        Vector3 currentNormal = c_playerData.CurrentNormal;
        Vector3 currentSurfaceNormal = c_playerData.CurrentSurfaceNormal;
        Vector3 currentSurfacePosition = c_playerData.CurrentSurfaceAttachPoint;
        Quaternion currentRotation = c_playerData.RotationBuffer;

        float inputAxis = c_playerData.f_inputAxisTurn * c_playerData.f_turnSpeed;

        cart_acceleration.Decelerate(ref currentVelocity, deceleration);
        cart_velocity.RaycastAdjustment(ref currentSurfacePosition, ref currentPosition, ref currentRotation);
        cart_angleCalc.AlignRotationWithSurface(ref currentSurfaceNormal, ref currentNormal, ref currentDir, ref currentRotation);
        cart_handling.Turn(ref currentDir, ref currentNormal, ref inputAxis, ref currentRotation);
        cart_velocity.UpdatePositionTwo(ref currentPosition, ref currentRotation, ref currentVelocity);


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
        if (cmd == Command.SLOW)
        {
            return StateRef.STOPPING;
        }
        if (cmd == Command.STOP)
        {
            return StateRef.STATIONARY;
        }
        return StateRef.CARVING_STOPPING;
    }
}
