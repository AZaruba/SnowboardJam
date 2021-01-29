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
        Vector3 currentDir = c_playerData.v_currentDirection;
        Quaternion currentRotation = c_playerData.q_currentRotation;
        Quaternion currentModelRotation = c_positionData.q_currentModelRotation;

        float currentTurnSpeed = c_turnData.f_currentTurnSpeed;
        float currentTurnAccel = c_turnData.f_turnAcceleration * c_playerInputData.f_inputAxisLHoriz;
        float turnSpeedCap = c_turnData.f_turnTopSpeed;


        AccelerationCartridge.Accelerate(ref currentTurnSpeed, currentTurnAccel, turnSpeedCap);
        HandlingCartridge.Turn(ref currentDir, Vector3.up, currentTurnSpeed * Time.fixedDeltaTime, ref currentRotation);

        c_playerData.v_currentDirection = currentDir.normalized;
        c_playerData.q_currentRotation = currentRotation;
        c_positionData.q_currentModelRotation = Quaternion.Lerp(currentRotation, currentModelRotation, Constants.LERP_DEFAULT);

        c_turnData.f_currentTurnSpeed = currentTurnSpeed;
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
