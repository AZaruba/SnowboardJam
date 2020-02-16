using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMessageClient : iMessageClient
{
    ClientID clientID;

    public CharacterMessageClient()
    {
        clientID = ClientID.CHARACTER_CLIENT;
    }

    public bool SendMessage(MessageID id, Message message)
    {
        MessageServer.SendMessage(id, message);
        return true;
    }

    public bool RecieveMessage(MessageID id, Message message)
    {
        return true;
    }
}
