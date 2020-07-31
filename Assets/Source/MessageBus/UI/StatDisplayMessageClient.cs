using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class StatDisplayMessageClient : iMessageClient
{
    private Text uiText;
    private CharacterStatDisplayController parent;

    public StatDisplayMessageClient(ref Text uiTextIn, CharacterStatDisplayController parentIn)
    {
        this.uiText = uiTextIn;
        this.parent = parentIn;
    }

    public bool SendMessage(MessageID id, Message message)
    {
        return true;
    }

    public bool RecieveMessage(MessageID id, Message message)
    {
        if (id == MessageID.MENU_ITEM_CHANGED)
        {
            parent.currentIndex = message.getInt();
        }
        return false;
    }
}
