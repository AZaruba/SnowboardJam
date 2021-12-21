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
        Quaternion currentModelRotation = c_positionData.q_currentModelRotation;

        float currentTurnSpeed = c_turnData.f_currentTurnSpeed;
        float currentRealTurnSpeed = c_turnData.f_currentRealTurnSpeed;

        // AccelerationCartridge.DecelerateAbs(ref currentTurnSpeed, c_turnData.f_turnResetSpeed);
        AccelerationCartridge.DecelerateAbs(ref currentRealTurnSpeed, c_turnData.f_turnResetSpeed);

        //HandlingCartridge.Turn(Vector3.up, currentTurnSpeed * Time.fixedDeltaTime, ref currentModelRotation);
        HandlingCartridge.Turn(Vector3.up, currentRealTurnSpeed * Time.fixedDeltaTime, ref currentRotation);

        c_positionData.q_currentModelRotation = currentModelRotation;
        c_playerData.q_currentRotation = currentRotation;
        c_turnData.f_currentTurnSpeed = currentTurnSpeed;
        c_turnData.f_currentRealTurnSpeed = currentRealTurnSpeed;
    }

    public void TransitionAct()
    {

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
        Quaternion currentModelRotation = c_positionData.q_currentModelRotation;

        float currentTurnSpeed = c_turnData.f_currentTurnSpeed;
        float currentRealTurnSpeed = c_turnData.f_currentRealTurnSpeed;

        AccelerationCartridge.DecelerateAbs(ref currentTurnSpeed, c_turnData.f_turnResetSpeed);
        AccelerationCartridge.DecelerateAbs(ref currentRealTurnSpeed, c_turnData.f_turnResetSpeed);

        HandlingCartridge.Turn(Vector3.up, currentRealTurnSpeed * Time.fixedDeltaTime, ref currentRotation);
        HandlingCartridge.Turn(Vector3.up, currentTurnSpeed * Time.fixedDeltaTime, ref currentModelRotation);


        c_positionData.q_currentModelRotation = currentRotation;
        c_playerData.q_currentRotation = currentRotation;
        c_turnData.f_currentTurnSpeed = currentTurnSpeed;
        c_turnData.f_currentRealTurnSpeed = currentRealTurnSpeed;
    }

    public void TransitionAct()
    {
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
