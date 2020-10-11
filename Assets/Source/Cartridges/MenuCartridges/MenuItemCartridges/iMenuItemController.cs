using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class iMenuItemController :  MonoBehaviour
{
    [SerializeField] private string HelpText;

    private MenuItemActiveData c_itemActiveData;
    private StateMachine sm_menuItem;
    protected MenuController c_parent;

    public abstract void ExecuteMenuCommand();

    public virtual void MenuCommandBack()
    {
        MessageServer.SendMessage(MessageID.MENU_BACK, new Message());
    }

    public virtual void ExecuteStateMachineCommand(Command cmd)
    {
        sm_menuItem.Execute(cmd);
    }

    public virtual void UpdateEditor()
    {

    }

    public abstract void InitializeStateMachine();
    public abstract void InitializeData();

    public void OnItemActive(int thisId = -1)
    {
        MessageServer.SendMessage(MessageID.MENU_ITEM_CHANGED, new Message(this.HelpText, thisId));
    }

    public void SetParent(MenuController parentIn)
    {
        this.c_parent = parentIn;
    }
}
