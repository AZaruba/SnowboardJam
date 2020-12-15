using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpTextMessageClient : iMessageClient
{
    private Text uiText;

    public HelpTextMessageClient(ref Text uiTextIn)
    {
        this.uiText = uiTextIn;
    }

    public bool RecieveMessage(MessageID id, Message message)
    {
        if (id == MessageID.MENU_ITEM_CHANGED)
        {
            uiText.text = message.getString();
        }
        return false;
    }
}
