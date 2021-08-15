using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarvingState : iState {

    HandlingCartridge         cart_handling;
    SurfaceInfluenceCartridge cart_surfInf;
    PlayerData                c_playerData;
    PlayerHandlingData        c_turnData;
    CollisionData             c_collisionData;
    PlayerInputData           c_playerInputData;
    PlayerPositionData        c_positionData;

    public CarvingState(ref PlayerData playerData,
                        ref PlayerHandlingData turnDataIn,
                        ref CollisionData collisionData,
                        ref PlayerInputData inputData,
                        ref PlayerPositionData positionData,
                        ref HandlingCartridge handling,
                        ref SurfaceInfluenceCartridge surfInf)
    {
        this.c_playerData = playerData;
        this.c_collisionData = collisionData;
        this.c_turnData = turnDataIn;
        this.c_playerInputData = inputData;
        this.c_positionData = positionData;
        this.cart_handling = handling;
        this.cart_surfInf = surfInf;
    }

    public void Act()
    {
        Quaternion currentModelRotation = c_positionData.q_currentModelRotation;
        Quaternion currentRotation = c_playerData.q_currentRotation;

        float currentTurnSpeed = c_turnData.f_currentTurnSpeed;
        float currentRealTurnSpeed = c_turnData.f_currentRealTurnSpeed;
        float currentTurnAccel = c_turnData.f_turnAcceleration * c_playerInputData.f_inputAxisLHoriz;
        float turnSpeedCap = c_turnData.f_turnTopSpeed;

        AccelerationCartridge.AccelerateAbs(ref currentTurnSpeed, currentTurnAccel, turnSpeedCap);
        AccelerationCartridge.AccelerateAbs(ref currentRealTurnSpeed, currentTurnAccel, turnSpeedCap, c_turnData.f_currentSurfaceFactor);
        HandlingCartridge.Turn(Vector3.up, currentTurnSpeed * Time.fixedDeltaTime, ref currentModelRotation);
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
