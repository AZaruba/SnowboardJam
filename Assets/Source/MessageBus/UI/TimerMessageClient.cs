using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerMessageClient : iMessageClient
{
    ClientID clientID;
    private StateData c_stateData;

    public TimerMessageClient(ref StateData dataIn)
    {
        clientID = ClientID.TIMER_CLIENT;
        c_stateData = dataIn;
    }

    public bool SendMessage(MessageID id, Message message)
    {
        return true;
    }

    public bool RecieveMessage(MessageID id, Message message)
    {
        if (id == MessageID.PAUSE)
        {
            c_stateData.b_updateState = message.getInt() == 0;
            return true;
        }
        return false;
    }
}
