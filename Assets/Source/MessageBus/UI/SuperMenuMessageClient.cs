using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperMenuMessageClient : iMessageClient
{
    SuperMenuController parent;

    public SuperMenuMessageClient(SuperMenuController parentIn)
    {
        this.parent = parentIn;
    }

    public bool RecieveMessage(MessageID id, Message message)
    {
        if (id == MessageID.MENU_BACK)
        {
            parent.PopMenuStack();
        }
        else if (id == MessageID.MENU_FORWARD)
        {
            parent.PushMenuStack(message.getInt());
        }
        return false;
    }

    public bool SendMessage(MessageID id, Message message)
    {
        return false;
    }
}
