using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMessageClient : iMessageClient
{
    ClientID clientID;
    StateData c_stateData;
    CameraTrackingData c_trackingData;

    public CameraMessageClient(ref StateData dataIn, ref CameraTrackingData trackingData)
    {
        this.clientID = ClientID.CAMERA_CLIENT;
        this.c_stateData = dataIn;
        this.c_trackingData = trackingData;
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
            c_trackingData.v_position_lastFrame = c_trackingData.v_position;
            c_trackingData.v_position = message.getVector();
        }

        return true;
    }

}

public class CameraMessageClient_Old : iMessageClient
{
    ClientID clientID;
    StateData c_stateData;
    NewCameraTargetData c_posData;

    public CameraMessageClient_Old(ref StateData dataIn, ref NewCameraTargetData posDataIn)
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
            c_posData.f_currentTargetVelocity = message.getFloat();
        }

        return true;
    }

}
