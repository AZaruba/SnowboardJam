using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightState : iState
{

    PlayerData c_playerData;
    PlayerHandlingData c_turnData;
    PlayerPositionData c_positionData;

    public StraightState(ref PlayerData playerData,
                         ref PlayerHandlingData turnDataIn,
                         ref PlayerPositionData positionData)
    {
        this.c_playerData = playerData;
        this.c_turnData = turnDataIn;
        this.c_positionData = positionData;
    }

    public void Act()
    {
        Quaternion currentRotation = c_playerData.q_currentRotation;
        c_positionData.q_currentModelRotation = currentRotation;
    }

    public void TransitionAct()
    {
        c_turnData.f_currentTurnSpeed = Constants.ZERO_F;
        c_turnData.f_currentRealTurnSpeed = Constants.ZERO_F;
    }

    public StateRef GetNextState(Command cmd)
    {
        if (cmd == Command.FALL ||
            cmd == Command.JUMP ||
            cmd == Command.CRASH)
        {
            return StateRef.DISABLED;
        }
        if (cmd == Command.CHARGE)
        {
            return StateRef.CHARGING;
        }
        if (cmd == Command.TURN)
        {
            return StateRef.CARVING;
        }
        return StateRef.RIDING;
    }
}

// secondary straightline state to handle turning while charging, ensures "LAND" command doesn't short circuit it
public class TurnChargeState : iState
{
    PlayerData c_playerData;
    PlayerHandlingData c_turnData;
    PlayerPositionData c_positionData;

    public TurnChargeState(ref PlayerData playerData,
                           ref PlayerHandlingData turnDataIn,
                           ref PlayerPositionData positionData)
    {
        this.c_playerData = playerData;
        this.c_turnData = turnDataIn;
        this.c_positionData = positionData;
    }

    public void Act()
    {
        Quaternion currentRotation = c_playerData.q_currentRotation;
        c_positionData.q_currentModelRotation = currentRotation;
    }

    public void TransitionAct()
    {
        c_turnData.f_currentTurnSpeed = Constants.ZERO_F;
        c_turnData.f_currentRealTurnSpeed = Constants.ZERO_F;
    }

    public StateRef GetNextState(Command cmd)
    {
        if (cmd == Command.JUMP)
        {
            return StateRef.DISABLED;
        }
        return StateRef.CHARGING;
    }
}
