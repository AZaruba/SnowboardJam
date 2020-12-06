using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMessageClient : iMessageClient
{
    ClientID clientID;
    StateData c_stateData;

    public CameraMessageClient(ref StateData dataIn)
    {
        clientID = ClientID.CAMERA_CLIENT;
        c_stateData = dataIn;
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

        return true;
    }

}
