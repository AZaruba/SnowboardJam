using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationaryState : iState {

    PlayerData c_playerData;
    PlayerPositionData c_playerPositionData;

    public StationaryState(ref PlayerData playerData,
                           ref PlayerPositionData positionData)
    {
        this.c_playerData = playerData;
        this.c_playerPositionData = positionData;
    }

    public void Act()
    {

    }

    public void TransitionAct()
    {
        c_playerData.v_currentDown = c_playerData.v_currentNormal.normalized * -1;
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
            c_playerData.f_currentSpeed += c_playerData.f_startBoost * c_playerPositionData.i_switchStance;
            return StateRef.RIDING;
        }
        return StateRef.STATIONARY;
    }
}
