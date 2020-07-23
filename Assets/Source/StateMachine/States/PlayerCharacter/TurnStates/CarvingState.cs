using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: The Turn function causes strange behavior on slopes and angle changes
public class CarvingState : iState {

    HandlingCartridge         cart_handling;
    SurfaceInfluenceCartridge cart_surfInf;
    PlayerData                c_playerData;
    PlayerPositionData        c_positionData;

    public CarvingState(ref PlayerData playerData,
                        ref PlayerPositionData positionData,
                        ref HandlingCartridge handling,
                        ref SurfaceInfluenceCartridge surfInf)
    {
        this.c_playerData = playerData;
        this.c_positionData = positionData;
        this.cart_handling = handling;
        this.cart_surfInf = surfInf;
    }

    public void Act()
    {
        Vector3 currentDir = c_playerData.v_currentDirection;
        Vector3 currentNormal = c_playerData.v_currentSurfaceNormal;
        Quaternion currentRotation = c_playerData.q_currentRotation;
        Quaternion currentModelRotation = c_positionData.q_currentModelRotation;

        float inputAxis = c_playerData.f_inputAxisTurn * c_playerData.f_turnSpeed;

        cart_handling.Turn(ref currentDir, ref currentNormal, ref inputAxis, ref currentRotation);

        c_playerData.v_currentDirection = currentDir.normalized;
        c_playerData.q_currentRotation = currentRotation;
        c_positionData.q_currentModelRotation = Quaternion.Lerp(currentRotation, currentModelRotation, Constants.LERP_DEFAULT);
    }

    public void TransitionAct()
    {

    }

    public StateRef GetNextState(Command cmd)
    {
        if (cmd == Command.CRASH)
        {
            return StateRef.DISABLED;
        }
        if (cmd == Command.FALL)
        {
            return StateRef.DISABLED;
        }
        if (cmd == Command.RIDE)
        {
            return StateRef.RIDING;
        }
        return StateRef.CARVING;
    }
}
