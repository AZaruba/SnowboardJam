using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuMessageClient : iMessageClient
{
    // should we just store the message then let classes access that directly?
    ClientID clientID;
    private ActiveMenuData data_out;

    public PauseMenuMessageClient(ref ActiveMenuData menuData)
    {
        clientID = ClientID.PAUSE_MENU_CLIENT;
        data_out = menuData;
    }

    public bool RecieveMessage(MessageID id, Message message)
    {
        if (id == MessageID.PAUSE)
        {
            data_out.b_showMenu = !data_out.b_showMenu;
            return true;
        }
        return false;
    }
}
