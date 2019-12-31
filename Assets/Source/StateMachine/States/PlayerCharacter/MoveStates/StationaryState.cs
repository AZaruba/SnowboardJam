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
        Vector3 currentPosition = c_playerData.CurrentPosition;
        Vector3 currentNormal = c_playerData.CurrentNormal;
        Vector3 currentForward = c_playerData.CurrentDirection;
        Vector3 currentSurfaceNormal = c_playerData.CurrentSurfaceNormal;
        Quaternion rotationBuf = c_playerData.RotationBuffer;

        cart_angleCalc.AlignRotationWithSurface(ref currentSurfaceNormal, ref currentNormal, ref currentForward, ref rotationBuf);

        c_playerData.CurrentNormal = currentNormal.normalized;
        c_playerData.CurrentDown = currentNormal.normalized * -1;
        c_playerData.CurrentDirection = currentForward.normalized;
        c_playerData.CurrentPosition = currentPosition;
        c_playerData.RotationBuffer = rotationBuf;
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
