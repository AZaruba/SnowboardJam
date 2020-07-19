using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class iMenuItemController :  MonoBehaviour
{
    [SerializeField] private string HelpText;

    public abstract void ExecuteMenuCommand();
    public abstract void ExecuteStateMachineCommand(Command cmd);

    public abstract void InitializeStateMachine();
    public abstract void InitializeData();

    public void OnItemActive()
    {
        MessageServer.SendMessage(MessageID.MENU_ITEM_CHANGED, new Message(this.HelpText));
    }
}
