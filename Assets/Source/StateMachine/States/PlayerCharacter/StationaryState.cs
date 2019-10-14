using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationaryState : iPlayerState {

    AngleCalculationCartridge cart_angleCalc;

    public StationaryState(ref AngleCalculationCartridge angleCalc)
    {
        this.cart_angleCalc = angleCalc;
    }

    public void Act(ref PlayerData c_playerData)
    {
        Vector3 currentPosition = c_playerData.CurrentPosition;
        Vector3 currentNormal = c_playerData.CurrentNormal;
        Vector3 currentForward = c_playerData.CurrentDirection;
        Vector3 currentSurfaceNormal = c_playerData.CurrentSurfaceNormal;
        Vector3 currentSurfaceAttPoint = c_playerData.CurrentSurfaceAttachPoint;

        cart_angleCalc.AlignRotationWithSurface(ref currentSurfaceNormal, ref currentNormal, ref currentForward);
        cart_angleCalc.MoveToAttachPoint(ref currentPosition, ref currentSurfaceAttPoint);

        c_playerData.CurrentNormal = currentNormal;
        c_playerData.CurrentDirection = currentForward;
        c_playerData.CurrentPosition = currentPosition;
        // surface normal does not need to be updated
    }

    /// <summary>
    /// Returns the state after the given command.
    /// </summary>
    /// <returns>An iState following a given Command, or this if none.</returns>
    /// <param name="cmd">The command</param>
    public StateRef GetNextState(Command cmd)
    {
        return StateRef.STATIONARY;
    }
}
