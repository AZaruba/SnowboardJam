using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMessageClient : iMessageClient
{
    ClientID clientID;
    StateData c_stateData;
    EntityData c_entityData;

    public CharacterMessageClient(ref StateData dataIn, ref EntityData entDataIn)
    {
        clientID = ClientID.CHARACTER_CLIENT;
        c_stateData = dataIn;
        c_entityData = entDataIn;
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
        if (id == MessageID.PLAYER_FINISHED && message.getUint() == c_entityData.u_entityID)
        {
            c_stateData.b_courseFinished = true;
        }

        return true;
    }
}
