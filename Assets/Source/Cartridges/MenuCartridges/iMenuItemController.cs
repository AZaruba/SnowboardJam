using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface iMenuItemController
{
    void ExecuteMenuCommand();
    void ExecuteStateMachineCommand(Command cmd);

    void InitializeStateMachine();
    void InitializeData();
}
