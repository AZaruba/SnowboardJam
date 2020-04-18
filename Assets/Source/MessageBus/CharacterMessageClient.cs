using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMessageClient : iMessageClient
{
    ClientID clientID;
    StateData c_stateData;

    public CharacterMessageClient(ref StateData dataIn)
    {
        clientID = ClientID.CHARACTER_CLIENT;
        c_stateData = dataIn;
    }

    public bool SendMessage(MessageID id, Message message)
    {
        MessageServer.SendMessage(id, message);
        return true;
    }

    public bool RecieveMessage(MessageID id, Message message)
    {
        if (id == MessageID.PAUSE)
        {
            c_stateData.b_updateState = message.getInt() == 0;
        }

        return true;
    }
}
