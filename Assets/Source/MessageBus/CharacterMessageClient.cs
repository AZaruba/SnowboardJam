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

    public bool SendMessage(MessageID id)
    {
        MessageServer.SendMessage(id);
        return true;
    }

    public bool RecieveMessage(MessageID id)
    {
        Debug.Log("Character Recieved " + id);
        return true;
    }
}
