using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMessageClient : iMessageClient
{
    ClientID clientID;
    StateData c_stateData;
    CameraPositionData c_posData;

    public CameraMessageClient(ref StateData dataIn, ref CameraPositionData posDataIn)
    {
        clientID = ClientID.CAMERA_CLIENT;
        c_stateData = dataIn;
        c_posData = posDataIn;
    }

    public bool RecieveMessage(MessageID id, Message message)
    {
        if (id == MessageID.PAUSE)
        {
            c_stateData.b_updateState = message.getInt() == 0;
        }

        if (id == MessageID.COUNTDOWN_START)
        {
            c_stateData.b_preStarted = false;
        }

        if (id == MessageID.PLAYER_POSITION_UPDATED)
        {
            c_posData.v_currentTargetPosition = message.getVector();
            c_posData.q_currentTargetRotation = message.getQuaternion();
        }

        return true;
    }

}
