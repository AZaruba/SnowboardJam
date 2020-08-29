using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntegerEditController : iEditController
{
    [SerializeField] int DefaultValue;
    [SerializeField] int MinimumValue;
    [SerializeField] int MaximumValue;

    private int i_currentValue;
    private int i_lastStoredValue;

    public override void CancelDataEdit()
    {
        // todo
    }

    public override void ConfirmDataEdit(DataTarget targetIn)
    {
        // todo
    }

    private void Start()
    {
        InitializeData();
        InitializeStateMachine();
        i_currentValue = DefaultValue;
    }

    private void Update()
    {
        if (c_controllerData.b_editorActive == false)
        {
            return;
        }

        if (GlobalInputController.GetInputValue(GlobalInputController.ControllerData.DTrickButton) == KeyValue.PRESSED)
        {
            ConfirmDataEdit(CurrentTarget);
            Deactivate();
        }

        if (GlobalInputController.GetInputValue(GlobalInputController.ControllerData.RTrickButton) == KeyValue.PRESSED)
        {
            CancelDataEdit();
            Deactivate();
        }
    }

    public override void Deactivate()
    {
        MessageServer.SendMessage(MessageID.EDIT_END, new Message(i_currentValue));
        sm_editController.Execute(Command.MENU_HIDE);
    }

    public override void InitializeStateMachine()
    {
        DataEditDisabledState disabledState = new DataEditDisabledState(ref c_controllerData);
        DataEditReadyState readyState = new DataEditReadyState(ref c_controllerData);

        sm_editController = new StateMachine(disabledState, StateRef.MENU_DISABLED);
        sm_editController.AddState(readyState, StateRef.MENU_READY);
    }

    public override void InitializeData()
    {
        c_controllerData = new EditControllerData();
        c_controllerData.b_editorActive = false;
        c_controllerData.b = default;
        c_controllerData.i = DefaultValue;
        c_controllerData.f = default;
        c_controllerData.res = default;

        c_controllerData.f_currentMenuTickCount = Constants.ZERO_F;
        c_controllerData.f_currentMenuWaitCount = ControllerData.ShortTickTime;
    }
}
