﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarvingState : iState {

    PlayerData                c_playerData;
    PlayerHandlingData        c_turnData;
    PlayerInputData           c_playerInputData;
    PlayerPositionData        c_positionData;

    public CarvingState(ref PlayerData playerData,
                        ref PlayerHandlingData turnDataIn,
                        ref PlayerInputData inputData,
                        ref PlayerPositionData positionData)
    {
        this.c_playerData = playerData;
        this.c_turnData = turnDataIn;
        this.c_playerInputData = inputData;
        this.c_positionData = positionData;
    }

    public void Act()
    {
        Quaternion currentModelRotation = c_positionData.q_currentModelRotation;
        Quaternion currentRotation = c_playerData.q_currentRotation;

        float currentSpeed = c_playerData.f_currentSpeed;
        float currentTurnSpeed = c_turnData.f_currentTurnSpeed;
        float currentRealTurnSpeed = c_turnData.f_currentRealTurnSpeed;
        float targetTurnAccel = c_turnData.f_turnAcceleration * c_playerInputData.f_inputAxisLHoriz;
        float turnSpeedCap = Mathf.Abs(c_turnData.f_turnTopSpeed * c_playerInputData.f_inputAxisLHoriz);

        float turnSign = Mathf.Sign(targetTurnAccel);

        AccelerationCartridge.CalculateInterpolatedAcceleration(out float currentTurnAccel,
                                                                targetTurnAccel,
                                                                turnSpeedCap * turnSign * Constants.NEGATIVE_ONE,
                                                                turnSpeedCap * turnSign,
                                                                currentTurnSpeed);

        AccelerationCartridge.AccelerateAbs(ref currentTurnSpeed, currentTurnAccel, turnSpeedCap);
        AccelerationCartridge.AccelerateAbs(ref currentRealTurnSpeed, currentTurnAccel, turnSpeedCap, c_turnData.f_currentSurfaceFactor);

        HandlingCartridge.Turn(Vector3.up, currentTurnSpeed * Time.fixedDeltaTime, ref currentModelRotation);
        HandlingCartridge.Turn(Vector3.up, currentRealTurnSpeed * Time.fixedDeltaTime, ref currentRotation);

        Debug.Log(currentRealTurnSpeed / turnSpeedCap);
        AccelerationCartridge.Decelerate(ref currentSpeed, 
            Mathf.Abs(currentRealTurnSpeed / c_turnData.f_turnTopSpeed) * c_turnData.f_turnSpeedDeceleration, 
            c_positionData.i_switchStance);

        c_positionData.q_currentModelRotation = currentModelRotation;
        c_playerData.q_currentRotation = currentRotation;
        c_turnData.f_currentTurnSpeed = currentTurnSpeed;
        c_turnData.f_currentRealTurnSpeed = currentRealTurnSpeed;
        c_playerData.f_currentSpeed = currentSpeed;
    }

    public void TransitionAct()
    {

    }

    public StateRef GetNextState(Command cmd)
    {
        if (cmd == Command.CRASH ||
            cmd == Command.JUMP ||
            cmd == Command.FALL)
        {
            return StateRef.DISABLED;
        }
        if (cmd == Command.CHARGE)
        {
            return StateRef.CHARGING;
        }
        if (cmd == Command.RIDE)
        {
            return StateRef.RIDING;
        }
        return StateRef.CARVING;
    }
}
