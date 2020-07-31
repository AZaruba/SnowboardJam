using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class iMenuItemController :  MonoBehaviour
{
    [SerializeField] private string HelpText;
    private StateMachine sm_menuItem;

    public abstract void ExecuteMenuCommand();
    public virtual void ExecuteStateMachineCommand(Command cmd)
    {
        sm_menuItem.Execute(cmd);
    }

    public abstract void InitializeStateMachine();
    public abstract void InitializeData();

    public void OnItemActive(int thisId = -1)
    {
        MessageServer.SendMessage(MessageID.MENU_ITEM_CHANGED, new Message(this.HelpText, thisId));
    }
}
