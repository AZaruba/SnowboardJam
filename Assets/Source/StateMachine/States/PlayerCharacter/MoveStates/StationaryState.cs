using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationaryState : iState {

    AngleCalculationCartridge cart_angleCalc;
    PlayerData c_playerData;

    public StationaryState(ref PlayerData playerData, ref AngleCalculationCartridge angleCalc)
    {
        this.c_playerData = playerData;
        this.cart_angleCalc = angleCalc;
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
        Quaternion rotationBuf = c_playerData.q_currentRotation;

        cart_angleCalc.AlignRotationWithSurface(ref currentSurfaceNormal, ref currentNormal, ref currentForward, ref rotationBuf);

        c_playerData.v_currentNormal = currentNormal.normalized;
        c_playerData.v_currentDown = currentNormal.normalized * -1;
        c_playerData.v_currentDirection = currentForward.normalized;
        c_playerData.v_currentPosition = currentPosition;
        c_playerData.q_currentRotation = rotationBuf;
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
        if (cmd == Command.RIDE)
        {
            return StateRef.RIDING;
        }
        return StateRef.STATIONARY;
    }
}
