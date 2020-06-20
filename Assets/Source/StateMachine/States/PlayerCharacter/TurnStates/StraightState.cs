using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightState : iState
{

    SurfaceInfluenceCartridge cart_surfInf;
    PlayerData c_playerData;
    PlayerPositionData c_positionData;

    public StraightState(ref PlayerData playerData,
                              ref PlayerPositionData positionData,
                              ref SurfaceInfluenceCartridge surfInf)
    {
        this.c_playerData = playerData;
        this.c_positionData = positionData;
        this.cart_surfInf = surfInf;
    }

    public void Act()
    {
        Quaternion currentRotation = c_playerData.q_currentRotation;
        bool isReversed = c_positionData.b_modelReversed;

        Quaternion currentModelRotation = currentRotation;
        cart_surfInf.SwitchOrientation(ref currentModelRotation, isReversed);

        c_positionData.q_currentModelRotation = currentModelRotation;
    }

    public void TransitionAct()
    {

    }

    public StateRef GetNextState(Command cmd)
    {
        if (cmd == Command.FALL)
        {
            return StateRef.DISABLED;
        }
        if (cmd == Command.TURN)
        {
            return StateRef.CARVING;
        }
        if (cmd == Command.CRASH)
        {
            return StateRef.DISABLED;
        }
        return StateRef.RIDING;
    }
}
