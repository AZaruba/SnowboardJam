using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RidingState : iState {

    AngleCalculationCartridge cart_angleCalc;
    AccelerationCartridge     cart_acceleration;
    VelocityCartridge         cart_velocity;
    SurfaceInfluenceCartridge cart_surfInf;

    PlayerData c_playerData;
    PlayerPositionData c_playerPositionData;
    CollisionData c_collisionData;

    public RidingState(ref PlayerData playerData, ref PlayerPositionData positionData, ref CollisionData collisionData,
        ref AngleCalculationCartridge angleCalc, ref AccelerationCartridge acceleration, 
        ref VelocityCartridge velocity,ref SurfaceInfluenceCartridge surfInf)
    {
        this.c_playerData = playerData;
        this.c_playerPositionData = positionData;
        this.c_collisionData = collisionData;
        this.cart_angleCalc = angleCalc;
        this.cart_acceleration = acceleration;
        this.cart_velocity = velocity;
        this.cart_surfInf = surfInf;
    }

    public void Act()
    {
        // check for angle when implemented
        float currentVelocity = c_playerData.f_currentSpeed;
        float f_acceleration = c_playerData.f_currentAcceleration;
        // float topSpeed = c_playerData.f_currentTopSpeed;
        Vector3 currentPosition = c_playerData.v_currentPosition;
        Vector3 currentDir = c_playerData.v_currentDirection;
        Vector3 currentNormal = c_playerData.v_currentNormal;
        Quaternion currentRotation = c_playerData.q_currentRotation;

        AccelerationCartridge.NewSoftCapAccelerate(ref currentVelocity,
                                                   c_playerData.f_gravity,
                                                   c_playerData.f_currentTopSpeed,
                                                   (currentRotation * Vector3.forward).y,
                                                   c_playerPositionData.i_switchStance);

        cart_velocity.UpdatePositionTwo(ref currentPosition, ref currentRotation, ref currentVelocity);

        c_playerData.f_currentSpeed = currentVelocity;
        c_playerData.v_currentPosition = currentPosition;
        c_playerData.v_currentNormal = currentNormal.normalized;
        c_playerData.v_currentDown = currentNormal.normalized * -1;
        c_playerData.v_currentDirection = currentDir.normalized;
        c_playerData.q_currentRotation = currentRotation;
        c_playerPositionData.i_switchStance = currentVelocity != Mathf.Abs(currentVelocity) ? Constants.SWITCH_STANCE : Constants.REGULAR_STANCE;
    }

    public void TransitionAct()
    {
        c_playerData.f_currentAirVelocity = 0.0f;
        c_playerData.f_currentRaycastDistance = c_playerData.f_raycastDistance;
        c_playerData.v_currentDown = c_collisionData.v_surfaceNormal * -1;
    }

    public StateRef GetNextState(Command cmd)
    {
        if (cmd == Command.FALL || cmd == Command.JUMP)
        {
            return StateRef.AIRBORNE;
        }
        if (cmd == Command.CHARGE)
        {
            return StateRef.CHARGING;
        }
        if (cmd == Command.SLOW)
        {
            return StateRef.STOPPING;
        }
        if (cmd == Command.CRASH)
        {
            return StateRef.CRASHED;
        }
        return StateRef.RIDING;
    }
}

// secondary riding state for jump charge
public class RidingChargeState : iState
{

    AngleCalculationCartridge cart_angleCalc;
    AccelerationCartridge cart_acceleration;
    VelocityCartridge cart_velocity;
    SurfaceInfluenceCartridge cart_surfInf;

    PlayerData c_playerData;
    PlayerPositionData c_playerPositionData;
    CollisionData c_collisionData;

    public RidingChargeState(ref PlayerData playerData, ref PlayerPositionData positionData, ref CollisionData collisionData,
        ref AngleCalculationCartridge angleCalc, ref AccelerationCartridge acceleration,
        ref VelocityCartridge velocity, ref SurfaceInfluenceCartridge surfInf)
    {
        this.c_playerData = playerData;
        this.c_playerPositionData = positionData;
        this.c_collisionData = collisionData;
        this.cart_angleCalc = angleCalc;
        this.cart_acceleration = acceleration;
        this.cart_velocity = velocity;
        this.cart_surfInf = surfInf;
    }

    public void Act()
    {
        // check for angle when implemented
        float currentVelocity = c_playerData.f_currentSpeed;
        float f_acceleration = c_playerData.f_currentAcceleration;
        //float topSpeed = c_playerData.f_currentTopSpeed;
        Vector3 currentPosition = c_playerData.v_currentPosition;
        Vector3 currentDir = c_playerData.v_currentDirection;
        Vector3 currentNormal = c_playerData.v_currentNormal;
        Quaternion currentRotation = c_playerData.q_currentRotation;

        AccelerationCartridge.NewSoftCapAccelerate(ref currentVelocity,
                                                   c_playerData.f_gravity,
                                                   c_playerData.f_currentTopSpeed,
                                                   (currentRotation * Vector3.forward).y,
                                                   c_playerPositionData.i_switchStance);

        cart_velocity.UpdatePositionTwo(ref currentPosition, ref currentRotation, ref currentVelocity);

        c_playerData.f_currentSpeed = currentVelocity;
        c_playerData.v_currentPosition = currentPosition;
        c_playerData.v_currentNormal = currentNormal.normalized;
        c_playerData.v_currentDown = currentNormal.normalized * -1;
        c_playerData.v_currentDirection = currentDir.normalized;
        c_playerData.q_currentRotation = currentRotation;
        c_playerPositionData.i_switchStance = currentVelocity != Mathf.Abs(currentVelocity) ? Constants.SWITCH_STANCE : Constants.REGULAR_STANCE;
    }

    public void TransitionAct()
    {
        c_playerData.f_currentAirVelocity = 0.0f;
        c_playerData.f_currentRaycastDistance = c_playerData.f_raycastDistance;
        c_playerData.v_currentDown = c_collisionData.v_surfaceNormal * -1;
    }

    public StateRef GetNextState(Command cmd)
    {
        if (cmd == Command.FALL || cmd == Command.JUMP)
        {
            return StateRef.AIRBORNE;
        }
        if (cmd == Command.CRASH)
        {
            return StateRef.CRASHED;
        }
        return StateRef.CHARGING;
    }
}
