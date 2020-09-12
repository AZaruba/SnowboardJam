using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMessageClient : iMessageClient
{
    // should we just store the message then let classes access that directly?
    ClientID clientID;
    private ActiveMenuData data_out;

    public MenuMessageClient(ref ActiveMenuData menuData)
    {
        clientID = ClientID.PAUSE_MENU_CLIENT;
        data_out = menuData;
    }

    public bool SendMessage(MessageID id, Message message)
    {
        return true;
    }

    public bool RecieveMessage(MessageID id, Message message)
    {
        if (id == MessageID.EDIT_START)
        {
            data_out.b_menuActive = false;
            return true;
        }
        if (id == MessageID.EDIT_END)
        {
            data_out.b_menuActive = true;
            return true;
        }
        return false;
    }
}