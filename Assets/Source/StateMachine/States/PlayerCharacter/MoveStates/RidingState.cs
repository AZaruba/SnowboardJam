using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RidingState : iState {

    PlayerData c_playerData;
    PlayerPositionData c_playerPositionData;
    CollisionData c_collisionData;

    public RidingState(ref PlayerData playerData, ref PlayerPositionData positionData, ref CollisionData collisionData)
    {
        this.c_playerData = playerData;
        this.c_playerPositionData = positionData;
        this.c_collisionData = collisionData;
    }

    public void Act()
    {
        // check for angle when implemented
        float currentVelocity = c_playerData.f_currentSpeed;
        Vector3 currentPosition = c_playerData.v_currentPosition;
        Quaternion currentRotation = c_playerData.q_currentRotation;
        Quaternion currentModelRotation = c_playerPositionData.q_currentModelRotation;

        AccelerationCartridge.AccelerateGravity(ref currentVelocity,
            c_playerData.f_gravity,
            c_playerData.f_topSpeed,
            ref currentRotation,
            ref currentModelRotation);

        AccelerationCartridge.DecelerateFriction(ref currentVelocity,
            0.1f,
            currentRotation);

        VelocityCartridge.UpdatePositionTwo(ref currentPosition, ref currentRotation, ref currentVelocity);

        c_playerData.f_currentSpeed = currentVelocity;
        c_playerData.v_currentPosition = currentPosition;
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
    PlayerData c_playerData;
    PlayerPositionData c_playerPositionData;
    CollisionData c_collisionData;

    public RidingChargeState(ref PlayerData playerData, ref PlayerPositionData positionData, 
        ref CollisionData collisionData)
    {
        this.c_playerData = playerData;
        this.c_playerPositionData = positionData;
        this.c_collisionData = collisionData;
    }

    public void Act()
    {
        // check for angle when implemented
        float currentVelocity = c_playerData.f_currentSpeed;
        Vector3 currentPosition = c_playerData.v_currentPosition;
        Quaternion currentRotation = c_playerData.q_currentRotation;
        Quaternion currentModelRotation = c_playerPositionData.q_currentModelRotation;

        AccelerationCartridge.AccelerateGravity(ref currentVelocity,
            c_playerData.f_gravity,
            c_playerData.f_topSpeed,
            ref currentRotation,
            ref currentModelRotation);

        AccelerationCartridge.DecelerateFriction(ref currentVelocity,
            0.1f,
            currentRotation);

        VelocityCartridge.UpdatePositionTwo(ref currentPosition, ref currentRotation, ref currentVelocity);

        c_playerData.f_currentSpeed = currentVelocity;
        c_playerData.v_currentPosition = currentPosition;
        c_playerData.q_currentRotation = currentRotation;
        c_playerPositionData.i_switchStance = currentVelocity != Mathf.Abs(currentVelocity) ? Constants.SWITCH_STANCE : Constants.REGULAR_STANCE;
    }

    public void TransitionAct()
    {

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
