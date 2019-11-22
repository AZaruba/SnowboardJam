using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarvingState : iPlayerState {

    AngleCalculationCartridge cart_angleCalc;
    AccelerationCartridge     cart_acceleration;
    VelocityCartridge         cart_velocity;
    HandlingCartridge         cart_handling;

    //TODO: Investigate breaking out certain actions into a separate state machine
    //TODO: Give player data to the state machine and possibly the states by reference
    public CarvingState(ref AngleCalculationCartridge angleCalc,
        ref AccelerationCartridge acceleration,
        ref VelocityCartridge velocity,
        ref HandlingCartridge handling)
    {
        this.cart_angleCalc = angleCalc;
        this.cart_acceleration = acceleration;
        this.cart_velocity = velocity;
        this.cart_handling = handling;
    }

    public void Act(ref PlayerData c_playerData)
    {
        // check for angle when implemented
        float currentVelocity = c_playerData.CurrentSpeed;
        float acceleration = c_playerData.Acceleration;
        float topSpeed = c_playerData.TopSpeed;
        Vector3 currentPos = c_playerData.CurrentPosition;
        Vector3 currentDir = c_playerData.CurrentDirection;
        Vector3 currentNormal = c_playerData.CurrentNormal;
        Vector3 currentSurfaceNormal = c_playerData.CurrentSurfaceNormal;
        Quaternion currentRotation = c_playerData.RotationBuffer;

        float inputAxis = c_playerData.f_inputAxisTurn * c_playerData.f_turnSpeed;

        cart_acceleration.Accelerate(ref currentVelocity, ref acceleration, topSpeed);
        cart_angleCalc.AlignRotationWithSurface(ref currentSurfaceNormal, ref currentNormal, ref currentDir, ref currentRotation);
        cart_handling.Turn(ref currentDir, ref currentNormal, ref inputAxis, ref currentRotation);
        cart_velocity.UpdatePosition(ref currentPos, ref currentDir, ref currentVelocity);

        c_playerData.CurrentSpeed = currentVelocity;
        c_playerData.Acceleration = acceleration;
        c_playerData.CurrentPosition = currentPos;
        c_playerData.CurrentNormal = currentNormal;
        c_playerData.CurrentDirection = currentDir;
        c_playerData.RotationBuffer = currentRotation;
    }

    public void TransitionAct(ref PlayerData c_playerData)
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
        return StateRef.CARVING;
    }
}
