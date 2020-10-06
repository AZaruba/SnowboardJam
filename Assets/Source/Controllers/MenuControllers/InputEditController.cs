using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputEditController : iEditController
{
    [SerializeField] public ControlAction InputAction;
    [SerializeField] public Text ValueDisplay;

    public override void CancelDataEdit()
    {
        MessageServer.SendMessage(MessageID.EDIT_END, new Message());
        c_controllerData.k = GlobalInputController.GetInputKey(InputAction);
        Deactivate();
    }

    public override void ConfirmDataEdit(DataTarget targetIn)
    {
        MessageServer.SendMessage(MessageID.EDIT_END, new Message());
        EnginePush();
        GlobalInputController.UpdateAction(InputAction, c_controllerData.k);
        GlobalGameData.SetActionSetting(InputAction, c_controllerData.k);
        Deactivate();
    }

    public virtual void EnginePush()
    {
        ValueDisplay.text = c_controllerData.k.ToString();
    }

    public override void InitializeData()
    {
        c_controllerData = new EditControllerData();
        c_controllerData.b_editorActive = false;
        c_controllerData.k = GlobalGameData.GetActionSetting(InputAction);
    }

    // Start is called before the first frame update
    void Start()
    {
        InitializeData();
        InitializeStateMachine();
        ValueDisplay.text = c_controllerData.k.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (c_controllerData.b_editorActive == false)
        {
            return;
        }

        KeyCode keyIn = GlobalInputController.GetAnyKey();
        if (keyIn == KeyCode.Escape)
        {
            CancelDataEdit();
        }

        else if (keyIn != KeyCode.None)
        {
            c_controllerData.k = keyIn;
            ConfirmDataEdit(CurrentTarget);
        }

        // no state machine needed, handled by isActive
        EnginePush();
    }
}
