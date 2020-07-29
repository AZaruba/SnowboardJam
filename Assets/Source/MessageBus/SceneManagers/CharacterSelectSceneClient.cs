using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectSceneClient : iMessageClient
{
    CharacterSelectSceneController parent;
    public CharacterSelectSceneClient(CharacterSelectSceneController parentIn)
    {
        this.parent = parentIn;
    }

    public bool RecieveMessage(MessageID id, Message message)
    {
        if (id == MessageID.CHARACTER_SELECTED)
        {
            PlayerID idIn = (PlayerID)message.getUint();
            if (!parent.playersReady.ContainsKey(idIn))
            {
                return false;
            }
            parent.playersReady[idIn] = true;
            parent.b_messageRecieved = true;
            return true;
        }
        else if (id == MessageID.CHARACTER_UNSELECTED)
        {
            PlayerID idIn = (PlayerID)message.getUint();
            if (!parent.playersReady.ContainsKey(idIn))
            {
                return false;
            }
            parent.playersReady[idIn] = false;
            return true;
        }
        return false;
    }

    // TODO: message clients don't need to send messages, or if they do it should be at the iMessageClient level
    public bool SendMessage(MessageID id, Message message)
    {
        return false;
    }
}
