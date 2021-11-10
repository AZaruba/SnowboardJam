using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneMessageClient : iMessageClient
{
    public bool RecieveMessage(MessageID id, Message message)
    {
        Message sentMessage = new Message(message.getInt());
        MessageServer.SendMessage(MessageID.ERROR_MSG, sentMessage);
        return true;
    }
}
