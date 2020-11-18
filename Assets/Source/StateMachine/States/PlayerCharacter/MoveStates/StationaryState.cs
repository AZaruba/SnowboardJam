﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationaryState : iState {

    AngleCalculationCartridge cart_angleCalc;
    VelocityCartridge cart_velocity;
    PlayerData c_playerData;
    CollisionData c_collisionData;

    public StationaryState(ref PlayerData playerData,
                           ref CollisionData collisionData,
                           ref AngleCalculationCartridge angleCalc, ref VelocityCartridge velocity)
    {
        this.c_playerData = playerData;
        this.c_collisionData = collisionData;
        this.cart_angleCalc = angleCalc;
        this.cart_velocity = velocity;
    }

    public void Act()
    {

    }

    public void TransitionAct()
    {
        Vector3 currentPosition = c_playerData.v_currentPosition;
        Vector3 currentNormal = c_playerData.v_currentNormal;
        Vector3 currentDir = c_playerData.v_currentDirection;
        Quaternion currentRotation = c_playerData.q_currentRotation;

        // cart_angleCalc.AlignRotationWithSurface(ref currentSurfaceNormal, ref currentNormal, ref currentDir, ref currentRotation, angleDifference);
        cart_angleCalc.AlignToSurfaceByTail(ref currentPosition,
                                            c_collisionData.v_backOffset,
                                            c_collisionData.v_frontOffset,
                                            c_collisionData.v_frontPoint,
                                            c_collisionData.v_frontNormal,
                                            ref currentRotation,
                                            ref currentDir,
                                            ref currentNormal);
        c_playerData.v_currentNormal = currentNormal.normalized;
        c_playerData.v_currentDown = currentNormal.normalized * -1;
        c_playerData.v_currentDirection = currentDir.normalized;
        c_playerData.v_currentPosition = currentPosition;
        c_playerData.q_currentRotation = currentRotation;
        c_playerData.f_currentSpeed = Constants.ZERO_F;
        c_playerData.f_currentCrashTimer = Constants.ZERO_F;
    }

    /// <summary>
    /// Returns the state after the given command.
    /// </summary>
    /// <returns>An iState following a given Command, or this if none.</returns>
    /// <param name="cmd">The command</param>
    public StateRef GetNextState(Command cmd)
    {
        if (cmd == Command.FALL)
        {
            return StateRef.AIRBORNE;
        }
        if (cmd == Command.STARTMOVE)
        {
            return StateRef.RIDING;
        }
        return StateRef.STATIONARY;
    }
}
