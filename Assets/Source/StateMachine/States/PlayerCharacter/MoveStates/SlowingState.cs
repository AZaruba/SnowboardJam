﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowingState : iState
{

    PlayerData c_playerData;
    PlayerInputData c_playerInputData;
    PlayerPositionData c_playerPositionData;
    CollisionData c_collisionData;

    public SlowingState(ref PlayerData playerData,
                        ref CollisionData collisionData,
                        ref PlayerInputData inputData,
                        ref PlayerPositionData positionData)
    {
        this.c_playerData = playerData;
        this.c_playerInputData = inputData;
        this.c_playerPositionData = positionData;
        this.c_collisionData = collisionData;
    }
    public void Act()
    {
        // check for angle when implemented
        float currentVelocity = c_playerData.f_currentSpeed;
        float deceleration = c_playerData.f_brakePower;
        float slowScaling = c_playerInputData.f_inputAxisLVert * - 1;
        Vector3 currentPosition = c_playerData.v_currentPosition;
        Vector3 currentNormal = c_playerData.v_currentNormal;
        Quaternion currentRotation = c_playerData.q_currentRotation;

        AccelerationCartridge.Decelerate(ref currentVelocity, 
                                         deceleration * slowScaling);

        AccelerationCartridge.DecelerateFriction(ref currentVelocity,
            0.1f,
            currentRotation);

        VelocityCartridge.UpdatePositionTwo(ref currentPosition, ref currentRotation, ref currentVelocity);

        c_playerData.f_currentSpeed = currentVelocity;
        c_playerData.v_currentPosition = currentPosition;
        c_playerData.v_currentNormal = currentNormal.normalized;
        c_playerData.v_currentDown = currentNormal.normalized * -1;
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
        if (cmd == Command.CHARGE)
        {
            return StateRef.CHARGING;
        }
        if (cmd == Command.STOP)
        {
            return StateRef.STATIONARY;
        }
        if (cmd == Command.CRASH)
        {
            return StateRef.CRASHED;
        }
        return StateRef.STOPPING;
    }
}
