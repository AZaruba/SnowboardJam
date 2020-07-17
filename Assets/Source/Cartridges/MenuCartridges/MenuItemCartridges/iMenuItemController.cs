using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class iMenuItemController :  MonoBehaviour
{
    public abstract void ExecuteMenuCommand();
    public abstract void ExecuteStateMachineCommand(Command cmd);

    public abstract void InitializeStateMachine();
    public abstract void InitializeData();
}
