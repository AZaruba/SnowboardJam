using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationaryState : iState {

    AngleCalculationCartridge cart_angleCalc;
    VelocityCartridge cart_velocity;
    PlayerData c_playerData;

    public StationaryState(ref PlayerData playerData, ref AngleCalculationCartridge angleCalc, ref VelocityCartridge velocity)
    {
        this.c_playerData = playerData;
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
        Vector3 currentForward = c_playerData.v_currentDirection;
        Vector3 currentSurfaceNormal = c_playerData.v_currentSurfaceNormal;
        Vector3 currentSurfacePosition = c_playerData.v_currentSurfaceAttachPoint;
        Quaternion currentRotation = c_playerData.q_currentRotation;


        cart_velocity.RaycastAdjustment(ref currentSurfacePosition, ref currentPosition, ref currentRotation);
        cart_angleCalc.AlignRotationWithSurface(ref currentSurfaceNormal, ref currentNormal, ref currentForward, ref currentRotation);

        c_playerData.v_currentNormal = currentNormal.normalized;
        c_playerData.v_currentDown = currentNormal.normalized * -1;
        c_playerData.v_currentDirection = currentForward.normalized;
        c_playerData.v_currentPosition = currentPosition;
        c_playerData.q_currentRotation = currentRotation;
        c_playerData.f_currentSpeed = Constants.ZERO_F;
        c_playerData.f_currentCrashTimer = Constants.ZERO_F;
        // surface normal does not need to be updated
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
